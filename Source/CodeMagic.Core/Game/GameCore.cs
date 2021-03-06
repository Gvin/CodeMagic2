using System;
using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Area;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Game.PlayerActions;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Saving;
using CodeMagic.Core.Statuses;
using Microsoft.Extensions.Logging;

namespace CodeMagic.Core.Game
{
    public sealed class CurrentGame
    {
        private static ILogger<CurrentGame> _logger;
        private static readonly object GameLockObject = new();
        private static IGameCore _game;

        public static IGameCore Game
        {
            get
            {
                lock (GameLockObject)
                {
                    return _game;
                }
            }
            private set
            {
                lock (GameLockObject)
                {
                    _logger.LogDebug("Setting game instance");
                    _game = value;
                }
            }
        }

        public static IAreaMap Map => Game?.Map;

        public static IJournal Journal => Game?.Journal;

        public static IPlayer Player => Game?.Player;

        public static Point PlayerPosition => Game?.PlayerPosition;

        public static void Initialize(IAreaMap map, IPlayer player, Point playerPosition, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<CurrentGame>();
            Game = new GameCore(map, player, playerPosition, loggerFactory.CreateLogger<GameCore>());
        }

        public static void Load(IGameCore loadedGame)
        {
            Game = loadedGame;
        }
    }

    public class GameCore : IGameCore, ITurnProvider
    {
        private readonly ILogger<GameCore> _logger;

        private const string SaveKeyMap = "Map";
        private const string SaveKeyPlayer = "Player";
        private const string SaveKeyPlayerPosition = "PlayerPosition";
        private const string SaveKeyJournal = "Journal";
        private const string SaveKeyCurrentTurn = "CurrentTurn";

        private AreaMapFragment _cachedVisibleArea;

        public GameCore(SaveData dataBuilder, ILogger<GameCore> logger)
        {
            _logger = logger;
            Map = dataBuilder.GetObject<AreaMap>(SaveKeyMap);
            PlayerPosition = dataBuilder.GetObject<Point>(SaveKeyPlayerPosition);
            Player = Map.GetCell(PlayerPosition).Objects.OfType<IPlayer>().Single();
            Journal = new Journal();
            CurrentTurn = dataBuilder.GetIntValue(SaveKeyCurrentTurn);
            _cachedVisibleArea = null;
        }

        public GameCore(IAreaMap map, IPlayer player, Point playerPosition, ILogger<GameCore> logger)
        {
            _logger = logger;
            Map = map;
            PlayerPosition = playerPosition;
            Player = player;

            Map.AddObject(PlayerPosition, Player);

            Journal = new Journal();

            CurrentTurn = 1;

            _cachedVisibleArea = null;
        }

        public event EventHandler TurnEnded;
        public event EventHandler MapUpdated;

        public int CurrentTurn { get; private set; }

        public IAreaMap Map { get; private set; }

        public IPlayer Player { get; }

        public Journal Journal { get; }

        public Point PlayerPosition { get; private set; }

        public void ChangeMap(IAreaMap newMap, Point playerPosition)
        {
            _logger.LogDebug("Changing map");
            Map = newMap;
            Map.AddObject(playerPosition, Player);
            PlayerPosition = playerPosition;

            MapUpdated?.Invoke(this, EventArgs.Empty);
        }

        public void PerformPlayerAction(IPlayerAction action)
        {
            _logger.LogDebug($"Performing player action: {action.GetType().Name}");

            Map.PreUpdate();

            var endsTurn = action.Perform(out var newPosition);
            PlayerPosition = newPosition;

            if (endsTurn)
            {
                ProcessSystemTurn();

                if (GetIfPlayerIsFrozen())
                {
                    ProcessSystemTurn();
                }

                TurnEnded?.Invoke(this, EventArgs.Empty);
            }

            _cachedVisibleArea = null;
        }

        private void ProcessSystemTurn()
        {
            CurrentTurn++;
            Map.Update(this);
            MapUpdated?.Invoke(this, EventArgs.Empty);
        }

        private bool GetIfPlayerIsFrozen()
        {
            return Player.Statuses.Contains(FrozenObjectStatus.StatusType);
        }

        public AreaMapFragment GetVisibleArea()
        {
            if (_cachedVisibleArea != null)
                return _cachedVisibleArea;

            var visibleArea = VisibilityHelper.GetVisibleArea(Player.VisibilityRange, PlayerPosition);
            if (visibleArea == null)
                return null;

            if (Player.VisibilityRange == Player.MaxVisibilityRange)
                return visibleArea;

            var visibilityDifference = Player.MaxVisibilityRange - Player.VisibilityRange;
            var visibleAreaDiameter = Player.MaxVisibilityRange * 2 + 1;
            var result = new IAreaMapCell[visibleAreaDiameter][];

            for (int y = 0; y < visibleAreaDiameter; y++)
            {
                result[y] = new IAreaMapCell[visibleAreaDiameter];
            }

            for (int y = 0; y < visibleArea.Height; y++)
            {
                for (int x = 0; x < visibleArea.Width; x++)
                {
                    var visibleAreaY = y + visibilityDifference;
                    var visibleAreaX = x + visibilityDifference;
                    result[visibleAreaY][visibleAreaX] = visibleArea.GetCell(x, y);
                }
            }

            _cachedVisibleArea = new AreaMapFragment(result, visibleAreaDiameter, visibleAreaDiameter);
            return _cachedVisibleArea;
        }

        public void Dispose()
        {
        }

        public SaveDataBuilder GetSaveData()
        {
            var data = new Dictionary<string, object>
                {
                    {SaveKeyMap, Map},
                    {SaveKeyPlayer, Player},
                    {SaveKeyPlayerPosition, PlayerPosition},
                    {SaveKeyJournal, Journal},
                    {SaveKeyCurrentTurn, CurrentTurn}
                };
            return new SaveDataBuilder(GetType(), data);
        }
    }
}
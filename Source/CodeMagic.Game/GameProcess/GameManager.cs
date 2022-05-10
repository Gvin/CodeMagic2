using System;
using System.Threading.Tasks;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Core.Objects;
using CodeMagic.Game.Items.Custom;
using CodeMagic.Game.Items.ItemsGeneration;
using CodeMagic.Game.JournalMessages;
using CodeMagic.Game.JournalMessages.Scenario;
using CodeMagic.Game.MapGeneration.Dungeon;
using CodeMagic.Game.Objects.Creatures;
using Microsoft.Extensions.Logging;

namespace CodeMagic.Game.GameProcess
{
    public interface IGameManager
    {
        GameCore StartGame();

        void LoadGame();
    }

    public class GameManager : IGameManager
    {
        private Task _saveGameTask;
        private int _turnsSinceLastSaving;
        private readonly ISaveService _saveService;
        private readonly int _savingInterval;
        private readonly IDungeonMapGenerator _dungeonMapGenerator;
        private readonly ILoggerFactory _loggerFactory;

        public GameManager(ISaveService saveService, int savingInterval, ILoggerFactory loggerFactory, IDungeonMapGenerator dungeonMapGenerator)
        {
            _loggerFactory = loggerFactory;
            _dungeonMapGenerator = dungeonMapGenerator;
            _saveService = saveService;
            _savingInterval = savingInterval;
        }

        public GameCore StartGame()
        {
            if (CurrentGame.Game is GameCore oldGame)
            {
                oldGame.TurnEnded -= game_TurnEnded;
            }

            GameData.Initialize(new GameData());

            var player = CreatePlayer();

            var (startMap, playerPosition) = _dungeonMapGenerator.GenerateNewMap(1);
            CurrentGame.Initialize(startMap, player, playerPosition, _loggerFactory);
            var game = (GameCore) CurrentGame.Game;
            startMap.Refresh();

            player.Inventory.ItemAdded += (_, args) =>
            {
                game.Journal.Write(new ItemReceivedMessage(args.Item));
            };
            player.Inventory.ItemRemoved += (_, args) =>
            {
                game.Journal.Write(new ItemLostMessage(args.Item));
            };

            game.Journal.Write(new StartGameMessage());

            game.TurnEnded += game_TurnEnded;
            _saveService.SaveGame();

            _turnsSinceLastSaving = 0;

            return game;
        }

        public void LoadGame()
        {
            var (game, data) = _saveService.LoadGame();

            GameData.Initialize(data);
            CurrentGame.Load(game);

            if (game == null)
                return;

            game.Player.Inventory.ItemAdded += (_, args) =>
            {
                game.Journal.Write(new ItemReceivedMessage(args.Item));
            };
            game.Player.Inventory.ItemRemoved += (_, args) =>
            {
                game.Journal.Write(new ItemLostMessage(args.Item));
            };

            game.TurnEnded += game_TurnEnded;

            _turnsSinceLastSaving = 0;
        }

        private void game_TurnEnded(object sender, EventArgs args)
        {
            _turnsSinceLastSaving++;

            if (_turnsSinceLastSaving >= _savingInterval)
            {
                _saveGameTask?.Wait();
                _saveGameTask = _saveService.SaveGameAsync();
                _turnsSinceLastSaving = 0;
            }

            if (CurrentGame.Player.Health <= 0)
            {
                _saveGameTask?.Wait();
                ((GameCore)CurrentGame.Game).TurnEnded -= game_TurnEnded;
                CurrentGame.Load(null);
                _saveService.DeleteSave();
            }
        }

        private IPlayer CreatePlayer()
        {
            var player = new Player();
            player.Initialize();

            var itemsGenerator = ItemsGeneratorManager.Generator;

            var weapon = new TorchItem();
            player.Inventory.AddItem(weapon);
            player.Equipment.EquipHoldable(weapon, true);

            var spellBook = itemsGenerator.GenerateSpellBook(ItemRareness.Trash);
            player.Inventory.AddItem(spellBook);
            player.Equipment.EquipItem(spellBook);

            player.Inventory.AddItem(GenerateStartingUsable());
            player.Inventory.AddItem(GenerateStartingUsable());

#if DEBUG
            player.Inventory.AddItem(new BanHammer());
#endif

            return player;
        }

        private IItem GenerateStartingUsable()
        {
            while (true)
            {
                var usable = ItemsGeneratorManager.Generator.GenerateUsable(ItemRareness.Common);
                if (usable != null)
                    return usable;
            }

        }
    }
}
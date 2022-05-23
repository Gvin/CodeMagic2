using System;
using CodeMagic.Core.Area;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Game.PlayerActions;
using CodeMagic.Core.Objects;
using Microsoft.Extensions.Logging;

namespace CodeMagic.Core.Game
{
    public interface IGameCore : IDisposable
    {
        event EventHandler TurnEnded;

        event EventHandler MapUpdated;

        IAreaMap Map { get; }

        int CurrentTurn { get; }

        void ChangeMap(IAreaMap newMap, Point playerPosition);

        Journal Journal { get; }

        Point PlayerPosition { get; }

        IPlayer Player { get; }

        AreaMapFragment GetVisibleArea();

        void PerformPlayerAction(IPlayerAction action);

        void Initialize(ILogger<IGameCore> logger);
    }
}
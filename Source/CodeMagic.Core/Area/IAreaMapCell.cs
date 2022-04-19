﻿using CodeMagic.Core.Objects;
using CodeMagic.Core.Saving;

namespace CodeMagic.Core.Area
{
    public interface IAreaMapCell : ISaveable
    {
        LightLevel LightLevel { get; set; }

        IMapObject[] Objects { get; }

        int GetVolume<T>() where T : IVolumeObject;

        void RemoveVolume<T>(int volume) where T : IVolumeObject;

        bool BlocksMovement { get; }

        bool BlocksEnvironment { get; }

        bool BlocksVisibility { get; }

        bool BlocksProjectiles { get; }

        IDestroyableObject GetBiggestDestroyable();

        IEnvironment Environment { get; }
    }
}
﻿using CodeMagic.Core.Game;
using CodeMagic.Core.Objects;

namespace CodeMagic.Core.Statuses
{
    public interface IObjectStatus
    {
        bool Update(IDestroyableObject owner, Point position);

        IObjectStatus Merge(IObjectStatus oldStatus);

        string Type { get; }
    }
}
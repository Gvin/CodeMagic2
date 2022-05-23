﻿using System;
using CodeMagic.Core.Statuses;

namespace CodeMagic.Game.Statuses
{
    [Serializable]
    public class BlindObjectStatus : PassiveObjectStatusBase
    {
        public const string StatusType = "blind";
        private const int MaxLifeTime = 4;
        public const double HitChanceMultiplier = 0.1d;

        public BlindObjectStatus()
            : base(MaxLifeTime)
        {
        }

        public BlindObjectStatus(int timeToLive)
            : base(timeToLive)
        {
        }

        public override string Type => StatusType;
    }
}
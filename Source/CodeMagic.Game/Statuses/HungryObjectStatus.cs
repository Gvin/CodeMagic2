﻿using System;
using CodeMagic.Core.Statuses;

namespace CodeMagic.Game.Statuses
{
    [Serializable]
    public class HungryObjectStatus : PassiveObjectStatusBase
    {
        public const string StatusType = "hungry";

        public HungryObjectStatus() 
            : base(1)
        {
        }

        public override string Type => StatusType;
    }
}
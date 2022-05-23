using System;
using CodeMagic.Core.Statuses;

namespace CodeMagic.Game.Statuses
{
    [Serializable]
    public class OverweightObjectStatus : PassiveObjectStatusBase
    {
        public const string StatusType = "overweight";

        public OverweightObjectStatus() 
            : base(1)
        {
        }

        public override string Type => StatusType;
    }
}
using System;

namespace CodeMagic.Core.Statuses
{
    [Serializable]
    public class FrozenObjectStatus : PassiveObjectStatusBase
    {
        public const string StatusType = "frozen";
        private const int MaxLifeTime = 4;
        public const float SpeedMultiplier = 2f;

        public FrozenObjectStatus() 
            : base(MaxLifeTime)
        {
        }

        public FrozenObjectStatus(int timeToLive)
            : base(timeToLive)
        {
        }

        public override string Type => StatusType;
    }
}
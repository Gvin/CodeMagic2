using System;
using CodeMagic.Core.Statuses;

namespace CodeMagic.Game.Statuses
{
    [Serializable]
    public class ManaDisturbedObjectStatus : PassiveObjectStatusBase
    {
        public const string StatusType = "mana_disturbed";
        private const int StartTimeToLive = 4;

        public ManaDisturbedObjectStatus() 
            : base(StartTimeToLive)
        {
        }

        public override string Type => StatusType;
    }
}
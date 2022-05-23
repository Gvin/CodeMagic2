using System;
using CodeMagic.Core.Game;
using CodeMagic.Core.Objects;

namespace CodeMagic.Core.Statuses
{
    [Serializable]
    public abstract class PassiveObjectStatusBase : IObjectStatus
    {
        protected PassiveObjectStatusBase(int timeToLive)
        {
            this.TimeToLive = timeToLive;
        }

        public int TimeToLive { get; set; }

        public bool Update(IDestroyableObject owner, Point position)
        {
            if (TimeToLive <= 0)
            {
                return false;
            }

            TimeToLive--;
            return true;
        }

        public IObjectStatus Merge(IObjectStatus oldStatus)
        {
            if (!(oldStatus is PassiveObjectStatusBase passiveStatus) || !string.Equals(oldStatus.Type, Type))
                throw new InvalidOperationException($"Unable to merge {GetType().Name} status with {oldStatus.GetType().Name}");

            if (passiveStatus.TimeToLive > TimeToLive)
                return passiveStatus;
            return this;
        }

        public abstract string Type { get; }
    }
}
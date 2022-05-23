using System;
using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Game;
using CodeMagic.Core.Statuses;
using CodeMagic.Game.JournalMessages;

namespace CodeMagic.Game.Statuses
{
    [Serializable]
    public class ObjectStatusesCollection : IObjectStatusesCollection
    {
        private string _ownerId;
        private Func<string, bool> _statusFilter;

        public ObjectStatusesCollection()
        {
        }

        public ObjectStatusesCollection(string ownerId, Func<string, bool> statusFilter)
        {
            _statusFilter = statusFilter;
            _ownerId = ownerId;
            Statuses = new Dictionary<string, IObjectStatus>();
        }

        public Dictionary<string, IObjectStatus> Statuses { get; set; }

        public void Initialize(string ownerId, Func<string, bool> statusFilter)
        {
            _ownerId = ownerId;
            _statusFilter = statusFilter;
        }

        public void Add(IObjectStatus status)
        {
            if (_statusFilter(status.Type))
                return;

            if (Statuses.ContainsKey(status.Type))
            {
                Statuses[status.Type] = status.Merge(Statuses[status.Type]);
            }
            else
            {
                var owner = CurrentGame.Map.GetDestroyableObject(_ownerId);
                CurrentGame.Journal.Write(new StatusAddedMessage(owner, status.Type), owner);
                Statuses.Add(status.Type, status);
            }
        }

        public TStatus[] GetStatuses<TStatus>() where TStatus : IObjectStatus
        {
            return Statuses.Values.OfType<TStatus>().ToArray();
        }

        public TStatus GetStatus<TStatus>()
        {
            return Statuses.Values.OfType<TStatus>().FirstOrDefault();
        }

        public void Remove(string statusType)
        {
            if (Statuses.ContainsKey(statusType))
                Statuses.Remove(statusType);
        }

        public bool Contains(string statusType)
        {
            return Statuses.ContainsKey(statusType);
        }

        public void Update(Point position)
        {
            foreach (var status in Statuses.Values.ToArray())
            {
                var keepStatus = status.Update(CurrentGame.Map.GetDestroyableObject(_ownerId), position);
                if (!keepStatus)
                {
                    Statuses.Remove(status.Type);
                }
            }
        }
    }
}
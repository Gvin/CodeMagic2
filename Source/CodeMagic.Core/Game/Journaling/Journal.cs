using System;
using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Objects;

namespace CodeMagic.Core.Game.Journaling
{
    [Serializable]
    public class Journal : IJournal
    {
        public Journal()
        {
            Messages = new List<JournalMessageData>();
        }

        public List<JournalMessageData> Messages { get; set; }

        public void Write(IJournalMessage message)
        {
            lock (Messages)
            {
                Messages.Add(new JournalMessageData(message, CurrentGame.Game.CurrentTurn));
            }
        }

        public void Write(IJournalMessage message, IMapObject source)
        {
            lock (Messages)
            {
                if (IsObjectVisible(source))
                {
                    Messages.Add(new JournalMessageData(message, CurrentGame.Game.CurrentTurn));
                }
            }
        }

        private static bool IsObjectVisible(IMapObject source)
        {
            var visibleArea = CurrentGame.Game.GetVisibleArea();
            for (int x = 0; x < visibleArea.Width; x++)
            {
                for (int y = 0; y < visibleArea.Height; y++)
                {
                    if (visibleArea.GetCell(x, y)?.Objects.Any(obj => ReferenceEquals(obj, source)) ?? false)
                        return true;
                }
            }

            return false;
        }

        IJournalMessageData[] IJournal.Messages
        {
            get
            {
                lock (Messages)
                {
                    return Messages.ToArray<IJournalMessageData>();
                }
            }
        }
    }
}
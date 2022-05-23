using System;

namespace CodeMagic.Core.Game.Journaling;

public interface IJournalMessageData
{
    IJournalMessage Message { get; }

    int Turn { get; }
}

[Serializable]
public class JournalMessageData : IJournalMessageData
{
    public JournalMessageData(IJournalMessage message, int turn)
    {
        Message = message;
        Turn = turn;
    }

    public IJournalMessage Message { get; set; }

    public int Turn { get; set; }
}
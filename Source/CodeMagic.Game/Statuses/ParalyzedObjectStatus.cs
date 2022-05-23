using System;
using CodeMagic.Core.Statuses;

namespace CodeMagic.Game.Statuses;

[Serializable]
public class ParalyzedObjectStatus : PassiveObjectStatusBase
{
    public const string StatusType = "paralyzed";
    private const int MaxLifeTime = 4;

    public ParalyzedObjectStatus():
        base(MaxLifeTime)
    {
    }

    public ParalyzedObjectStatus(int lifeTimeToLive) :
        base(lifeTimeToLive)
    {
    }

    public override string Type => StatusType;
}
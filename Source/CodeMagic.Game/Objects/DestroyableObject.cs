using System;
using System.Collections.Generic;
using CodeMagic.Core.Common;
using CodeMagic.Core.Game;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Objects.ObjectEffects;
using CodeMagic.Core.Statuses;
using CodeMagic.Game.JournalMessages;
using CodeMagic.Game.Objects.ObjectEffects;
using CodeMagic.Game.Statuses;

namespace CodeMagic.Game.Objects;

[Serializable]
public abstract class DestroyableObject : MapObjectBase, IDestroyableObject
{
    private string _id;

    protected DestroyableObject()
    {
        Id = Guid.NewGuid().ToString();

        BaseProtection = new Dictionary<Element, int>();
        StatusesImmunity = new List<string>();
        ObjectEffects = new List<IObjectEffect>();
    }

    public Dictionary<Element, int> BaseProtection { get; set; }

    public List<string> StatusesImmunity { get; set; }

    public int HealthInternal { get; set; }

    public string Id
    {
        get => _id;
        set
        {
            _id = value;
            Statuses = new ObjectStatusesCollection(_id, status => Statuses.Contains(status));
        }
    }

    public virtual int DodgeChance => 0;

    public IObjectStatusesCollection Statuses { get; set; }

    protected virtual double SelfExtinguishChance => 15;

    public List<IObjectEffect> ObjectEffects { get; set; }

    public int Health
    {
        get => HealthInternal;
        set => HealthInternal = Math.Max(0, Math.Min(MaxHealth, value));
    }

    public abstract int MaxHealth { get; set; }

    protected virtual double CatchFireChanceMultiplier => 1;

    public virtual int GetProtection(Element element)
    {
        if (BaseProtection.ContainsKey(element))
            return BaseProtection[element];
        return 0;
    }

    public double GetSelfExtinguishChance()
    {
        var result = SelfExtinguishChance;

        var burningRelatesStatuses = Statuses.GetStatuses<IBurningRelatedStatus>();
        foreach (var burningRelatesStatus in burningRelatesStatuses)
        {
            result += burningRelatesStatus.SelfExtinguishChanceModifier;
        }

        return result;
    }

    public virtual void MeleeDamage(Point position, Direction attackDirection, int damage, Element element)
    {
        Damage(position, damage, element);
    }

    public virtual void Damage(Point position, int damage, Element element)
    {
        if (damage < 0)
            throw new ArgumentException($"Damage should be greater or equal 0. Got {damage}.");

        var protection = GetProtection(element);
        var protectionMultiplier = (100 - protection) / 100f;

        var realDamage = (int) Math.Round(damage * protectionMultiplier);
        realDamage = Math.Max(realDamage, 0);
        if (realDamage < damage)
        {
            CurrentGame.Journal.Write(new DamageBlockedMessage(this, damage - realDamage, element), this);
        }

        if (realDamage == 0)
            return;

        ApplyRealDamage(realDamage, element, position);

        ObjectEffects.Add(new DamageEffect(realDamage, element));
    }

    protected virtual void ApplyRealDamage(int damage, Element element, Point position)
    {
        Health -= damage;

        if (element == Element.Fire)
        {
            CheckCatchFire(damage);
        }
    }

    public void ClearDamageRecords()
    {
        ObjectEffects.Clear();
    }

    private OnFireObjectStatusConfiguration GetFireConfiguration()
    {
        return new OnFireObjectStatusConfiguration
        {
            BurnBeforeExtinguishCheck = 3
        };
    }

    private void CheckCatchFire(int damage)
    {
        var catchFireChance = damage * CatchFireChanceMultiplier;

        var burningRelatesStatuses = Statuses.GetStatuses<IBurningRelatedStatus>();
        foreach (var burningRelatesStatus in burningRelatesStatuses)
        {
            catchFireChance += burningRelatesStatus.CatchFireChanceModifier;
        }

        var catchFireWholeChange = (int) Math.Round(catchFireChance);
        if (RandomHelper.CheckChance(catchFireWholeChange))
        {
            Statuses.Add(new OnFireObjectStatus(GetFireConfiguration()));
        }
    }

    public virtual void OnDeath(Point position)
    {
        CurrentGame.Journal.Write(new DeathMessage(this), this);
    }

    public override bool Equals(IMapObject other)
    {
        if (other is DestroyableObject destroyable)
        {
            return string.Equals(Id, destroyable.Id);
        }

        return false;
    }
}
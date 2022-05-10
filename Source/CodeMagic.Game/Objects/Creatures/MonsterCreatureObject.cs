using System;
using System.Collections.Generic;
using CodeMagic.Core.Common;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Core.Objects;
using CodeMagic.Game.Configuration;
using CodeMagic.Game.Configuration.Monsters;
using CodeMagic.Game.JournalMessages;
using CodeMagic.Game.Objects.Creatures.Loot;
using CodeMagic.Game.Objects.DecorativeObjects;

namespace CodeMagic.Game.Objects.Creatures;

public abstract class MonsterCreatureObject : NonPlayableCreatureObject
{
    private MonsterCreatureObjectConfiguration _configuration;

    public MonsterCreatureObjectConfiguration Configuration
    {
        get => _configuration;
        set
        {
            _configuration = value ?? throw new ArgumentNullException(nameof(value), "Monster creature configuration cannot be null");
            LogicPattern = _configuration.LogicPattern;
            Health = _configuration.MaxHealth;
        }
    }

    public override Dictionary<Element, int> BaseProtection => Configuration.BaseProtection;

    public override List<string> StatusesImmunity => Configuration.StatusesImmunity;

    public override int MaxHealth => Configuration.MaxHealth;

    public override string Name => Configuration.Name;

    public override int DodgeChance => Configuration.DodgeChance;

    public sealed override ObjectSize Size => Configuration.Size;

    public sealed override ZIndex ZIndex => Configuration.ZIndex;

    protected sealed override double CatchFireChanceMultiplier => Configuration.CatchFireChanceMultiplier;

    protected sealed override double SelfExtinguishChance => Configuration.SelfExtinguishChance;

    public sealed override int MaxVisibilityRange => Configuration.VisibilityRange;

    protected sealed override float NormalSpeed => Configuration.Speed / 1;

    protected override int TryBlockMeleeDamage(Direction damageDirection, int damage, Element element)
    {
        if (!RandomHelper.CheckChance(Configuration.ShieldBlockChance))
            return damage;

        var blockedDamage = Math.Min(Configuration.ShieldBlocksDamage, damage);
        if (blockedDamage == 0)
            return damage;

        CurrentGame.Journal.Write(new ShieldBlockedDamageMessage(this, blockedDamage, element));
        return damage - blockedDamage;
    }

    public override void Attack(Point position, Point targetPosition, IDestroyableObject target)
    {
        base.Attack(position, targetPosition, target);

        var currentHitChance = CalculateHitChance(Configuration.Accuracy);
        if (!RandomHelper.CheckChance(currentHitChance))
        {
            CurrentGame.Journal.Write(new AttackMissMessage(this, target), this);
            return;
        }

        if (RandomHelper.CheckChance(target.DodgeChance))
        {
            CurrentGame.Journal.Write(new AttackDodgedMessage(this, target));
            return;
        }

        var attackDirection = Point.GetAdjustedPointRelativeDirection(targetPosition, position);
        if (!attackDirection.HasValue)
            throw new ApplicationException("Can only attack adjusted target");

        foreach (var damageValue in Configuration.Damage)
        {
            var value = RandomHelper.GetRandomValue(damageValue.MinValue, damageValue.MaxValue);
            target.MeleeDamage(targetPosition, attackDirection.Value, value, damageValue.Element);
            CurrentGame.Journal.Write(new DealDamageMessage(this, target, value, damageValue.Element), this);
        }
    }

    protected sealed override IMapObject GenerateRemains()
    {
        return CreatureRemains.Create(Configuration.RemainsType);
    }

    protected override IMapObject GenerateDamageMark()
    {
        return CreatureRemains.Create(Configuration.DamageMarkType);
    }

    protected sealed override IItem[] GenerateLoot()
    {
        return new ChancesLootGenerator(Configuration.LootConfiguration).GenerateLoot();
    }

    public override void OnDeath(Point position)
    {
        base.OnDeath(position);

        var experience = RandomHelper.GetRandomValue(Configuration.Experience.Min, Configuration.Experience.Max);
        CurrentGame.Player.AddExperience(experience);
    }
}

[Serializable]
public class MonsterCreatureObjectConfiguration
{
    public MonsterCreatureObjectConfiguration()
    {
        Damage = new List<MonsterDamageValue>();
        BaseProtection = new Dictionary<Element, int>();
        StatusesImmunity = new List<string>();
    }

    public int Accuracy { get; set; }

    public List<MonsterDamageValue> Damage { get; set; }

    public int DodgeChance { get; set; }

    public ILootConfiguration LootConfiguration { get; set; }

    public string Name { get; set; }

    public string Id { get; set; }

    public string LogicPattern { get; set; }

    public IMonsterExperienceConfiguration Experience { get; set; }

    public ObjectSize Size { get; set; }

    public ZIndex ZIndex { get; set; }

    public int MaxHealth { get; set; }

    public RemainsType RemainsType { get; set; }

    public RemainsType DamageMarkType { get; set; }

    public double CatchFireChanceMultiplier { get; set; }

    public double SelfExtinguishChance { get; set; }

    public int VisibilityRange { get; set; }

    public float Speed { get; set; }

    public Dictionary<Element, int> BaseProtection { get; set; }

    public List<string> StatusesImmunity { get; set; }

    public int ShieldBlockChance { get; set; }

    public int ShieldBlocksDamage { get; set; }
}

[Serializable]
public class MonsterDamageValue
{
    public MonsterDamageValue(Element element, int minValue, int maxValue)
    {
        Element = element;
        MinValue = minValue;
        MaxValue = maxValue;
    }

    public MonsterDamageValue()
    {
    }

    public Element Element { get; set; }

    public int MinValue { get; set; }

    public int MaxValue { get; set; }
}
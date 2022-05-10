using System;
using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Common;
using CodeMagic.Core.Game;
using CodeMagic.Core.Objects;
using CodeMagic.Game.Configuration.Monsters;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Objects.Creatures.NonPlayable;

[Serializable]
public class MonsterCreatureImpl : MonsterCreatureObject, IWorldImageProvider
{
    public static MonsterCreatureImpl Create(MonsterCreatureImplConfiguration config)
    {
        return new MonsterCreatureImpl
        {
            Configuration = config
        };
    }

    private MonsterCreatureImplConfiguration ConfigurationImpl => (MonsterCreatureImplConfiguration)Configuration;

    public ISymbolsImage GetWorldImage(IImagesStorage storage)
    {
        var body = storage.GetImage(ConfigurationImpl.Image);
        var directionImageName = GetWorldImageName();
        var directionImage = storage.GetImage(directionImageName);
        return SymbolsImage.Combine(body, directionImage);
    }

    private string GetWorldImageName()
    {
        switch (Direction)
        {
            case Direction.North:
                return "Creature_Up";
            case Direction.South:
                return "Creature_Down";
            case Direction.West:
                return "Creature_Left";
            case Direction.East:
                return "Creature_Right";
            default:
                throw new ArgumentException($"Unknown creature direction: {Direction}");
        }
    }
}

[Serializable]
public class MonsterCreatureImplConfiguration : MonsterCreatureObjectConfiguration
{
    public string Image { get; set; }

    public static MonsterCreatureImplConfiguration FromConfiguration(IMonsterConfiguration config)
    {
        var health = RandomHelper.GetRandomValue(config.Stats.MinHealth, config.Stats.MaxHealth);

        var result = new MonsterCreatureImplConfiguration
        {
            Id = config.Id,
            Name = config.Name,
            LogicPattern = config.LogicPattern,
            Experience = config.Experience,
            Size = config.Size,
            ZIndex = ZIndex.Creature,
            Accuracy = config.Stats.Accuracy,
            DodgeChance = config.Stats.DodgeChance,
            MaxHealth = health,
            RemainsType = config.RemainsType,
            DamageMarkType = config.DamageMarkType,
            CatchFireChanceMultiplier = config.Stats.CatchFireChanceMultiplier,
            SelfExtinguishChance = config.Stats.SelfExtinguishChanceMultiplier,
            Image = config.Image,
            LootConfiguration = config.Loot,
            VisibilityRange = config.Stats.VisibilityRange,
            Speed = config.Stats.Speed,
            ShieldBlockChance = config.Stats.ShieldBlockChance,
            ShieldBlocksDamage = config.Stats.ShieldBlocksDamage,
            Damage = new List<MonsterDamageValue>(config.Stats.Damage.Select(conf =>
                new MonsterDamageValue(conf.Element, conf.MinValue, conf.MaxValue)))
        };

        foreach (var protectionConfiguration in config.Stats.Protection)
        {
            result.BaseProtection.Add(protectionConfiguration.Element, protectionConfiguration.Value);
        }

        if (config.Stats.StatusesImmunity != null)
        {
            result.StatusesImmunity.AddRange(config.Stats.StatusesImmunity);
        }

        return result;
    }
}
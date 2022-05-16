using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using CodeMagic.Core.Common;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Objects.Creatures;
using CodeMagic.Game.Area.EnvironmentData;
using CodeMagic.Game.Configuration;
using CodeMagic.Game.Images;
using CodeMagic.Game.Items;
using CodeMagic.Game.JournalMessages;
using CodeMagic.Game.Objects.DecorativeObjects;
using CodeMagic.Game.Statuses;
using Microsoft.Extensions.Logging;
using Point = CodeMagic.Core.Game.Point;

namespace CodeMagic.Game.Objects.Creatures;

[Serializable]
public class Player : CreatureObject, IPlayer, ILightObject, IWorldImageProvider
{
    private readonly ILogger<Player> _logger = StaticLoggerFactory.CreateLogger<Player>();

    private const string ImageUp = "Creature_Up";
    private const string ImageDown = "Creature_Down";
    private const string ImageLeft = "Creature_Left";
    private const string ImageRight = "Creature_Right";

    private const string ImageUpUsable = "Player_Up_Usable";
    private const string ImageDownUsable = "Player_Down_Usable";
    private const string ImageLeftUsable = "Player_Left_Usable";
    private const string ImageRightUsable = "Player_Right_Usable";

    private const string ImageBody = "Player_Body";
    private const string ImageBodyLeftWeapon = "Player_Body_LeftWeapon";
    private const string ImageBodyRightWeapon = "Player_Body_RightWeapon";

    private const int StaminaLooseOnBlock = 5;

    private const int DefaultStatValue = 1;

    private const int MaxProtection = 75;
    private const int MaxDodgeChance = 50;

    private const double HungerIncrement = 0.02;
    private const double RegenerationIncrement = 0.03;

    private const double HungerBlocksRegeneration = 30d;
    private const double HungerBlocksManaRestore = 70d;

    public event EventHandler Died;
    public event EventHandler LeveledUp;

    public Player()
    {
        Equipment = new Equipment();
        Inventory = new Inventory();
        KnownPotions = new List<PotionType>();
        Stats = new Dictionary<PlayerStats, int>();

        foreach (var playerStat in Enum.GetValues<PlayerStats>())
        {
            Stats.Add(playerStat, DefaultStatValue);
        }
    }

    public int ManaValue { get; set; }

    public int StaminaValue { get; set; }

    public double RegenerationValue { get; set; }

    public double HungerPercentValue { get; set; }

    public Dictionary<PlayerStats, int> Stats { get; set; }

    public override string Name => "Player";

    public IEquipment Equipment { get; set; }

    public IInventory Inventory { get; set; }

    public List<PotionType> KnownPotions { get; set; }

    public int Experience { get; set; }

    public int Level { get; set; }

    [IgnoreDataMember]
    public int HungerPercent
    {
        get => (int)Math.Floor(HungerPercentValue);
        set => HungerPercentValue = Math.Max(0d, Math.Min(100d, value));
    }

    public int MaxCarryWeight => 23000 + 2000 * GetStat(PlayerStats.Strength);

    public override int MaxVisibilityRange => 4;

    public override int DodgeChance =>
        Math.Min(MaxDodgeChance, 1 * (GetStat(PlayerStats.Agility) - DefaultStatValue));

    public int DamageBonus => 2 * (GetStat(PlayerStats.Strength) - DefaultStatValue);

    public int AccuracyBonus => 1 * (GetStat(PlayerStats.Agility) - DefaultStatValue) + Equipment.GetHitChanceBonus(Inventory);

    public int ScrollReadingBonus => 2 * (GetStat(PlayerStats.Wisdom) - DefaultStatValue);

    public override bool BlocksMovement => true;

    public int ManaRegeneration => 2 + GetStat(PlayerStats.Wisdom) + Equipment.GetBonus(EquipableBonusType.ManaRegeneration, Inventory);

    [IgnoreDataMember]
    public override int MaxHealth
    {
        get => GetMaxHealth(GetStat(PlayerStats.Endurance)) +
               Equipment.GetBonus(EquipableBonusType.Health, Inventory);
        set {}
    }

    [IgnoreDataMember]
    public int Stamina
    {
        get => StaminaValue;
        set => StaminaValue = Math.Max(0, Math.Min(MaxStamina, value));
    }

    public int MaxStamina =>
        80 + 20 * GetStat(PlayerStats.Endurance) + Equipment.GetBonus(EquipableBonusType.Stamina, Inventory);

    [IgnoreDataMember]
    public int Mana
    {
        get => ManaValue;
        set => ManaValue = Math.Max(0, Math.Min(MaxMana, value));
    }

    public int MaxMana => 80 + 20 * GetStat(PlayerStats.Intelligence) + Equipment.GetBonus(EquipableBonusType.Mana, Inventory);

    public void Initialize()
    {
        Health = MaxHealth;
        Mana = MaxMana;
        Stamina = MaxStamina;

        HungerPercentValue = 0d;
        RegenerationValue = 0d;
        Experience = 0;
        Level = 1;

        Inventory.ItemRemoved += Inventory_ItemRemoved;

        foreach (var playerStat in Enum.GetValues<PlayerStats>())
        {
            Stats[playerStat] = DefaultStatValue;
        }
    }

    public void AddExperience(int exp)
    {
        Experience += exp;
        CurrentGame.Journal.Write(new ExperienceGainedMessage(exp));

        var xpToLevelUp = GetXpToLevelUp();
        if (Experience >= xpToLevelUp)
        {
            _logger.LogDebug("Leveled Up. EXP: {Experience}, EXP to LVL: {ExperienceToLevelUp}", Experience, GetXpToLevelUp());
            Level++;
            Experience -= xpToLevelUp;
            CurrentGame.Journal.Write(new LevelUpMessage(Level));
            LeveledUp?.Invoke(this, EventArgs.Empty);
        }
    }

    public bool IsKnownPotion(PotionType type)
    {
        return KnownPotions.Contains(type);
    }

    public void MarkPotionKnown(PotionType type)
    {
        if (!KnownPotions.Contains(type))
        {
            KnownPotions.Add(type);
        }
    }

    public void IncreaseStat(PlayerStats stat)
    {
        Stats[stat]++;
    }

    public int GetXpToLevelUp()
    {
        var config = ConfigurationManager.Current.Levels;
        return (int)Math.Pow(Level, config.PlayerLevels.XpLevelPower) * config.PlayerLevels.XpMultiplier;
    }

    private static int GetMaxHealth(int endurance)
    {
        return 10 + endurance * 10;
    }

    private int GetStat(PlayerStats stat)
    {
        return GetPureStat(stat) + Equipment.GetStatsBonus(stat, Inventory);
    }

    public int GetPureStat(PlayerStats stat)
    {
        return Stats[stat];
    }

    private void Inventory_ItemRemoved(object sender, ItemEventArgs e)
    {
        if (e.Item is not IEquipableItem equipable)
            return;

        if (Equipment.IsEquiped(equipable))
        {
            Equipment.UnequipItem(equipable);
        }
    }

    protected override IMapObject GenerateDamageMark()
    {
        return CreatureRemains.Create(RemainsType.BloodRedSmall);
    }

    protected override int TryBlockMeleeDamage(Direction damageDirection, int damage, Element element)
    {
        if (Direction != damageDirection)
            return damage;

        var remainingDamage = damage;

        var shields = Equipment.GetEquippedItems(Inventory).OfType<IShieldItem>().ToArray();
        foreach (var shield in shields)
        {
            if (!RandomHelper.CheckChance(shield.ProtectChance)) 
                continue;

            if (Stamina < StaminaLooseOnBlock) 
                continue;

            Stamina -= StaminaLooseOnBlock;

            var damageBlocked = Math.Min(shield.BlocksDamage, remainingDamage);
            if (damageBlocked > 0)
            {
                CurrentGame.Journal.Write(new ShieldBlockedDamageMessage(this, damageBlocked, element));
            }

            remainingDamage -= damageBlocked;
            shield.Durability--;
        }

        return remainingDamage;
    }

    protected override void ApplyRealDamage(int damage, Element element, Point position)
    {
        base.ApplyRealDamage(damage, element, position);

        var targetArmorType = RandomHelper.GetRandomEnumValue<ArmorType>();
        if (Equipment.GetEquipedArmor(targetArmorType, Inventory) is IDurableItem targetArmor)
        {
            targetArmor.Durability--;
        }
    }

    public override void Update(Point position)
    {
        base.Update(position);

        Inventory.Update();

        IncrementHunger();
        RegenerateHealth();
        RegenerateMana(position);
        CheckOverweight();
    }

    private void RegenerateHealth()
    {
        if (HungerPercentValue >= HungerBlocksRegeneration)
        {
            RegenerationValue = 0;
            return;
        }

        RegenerationValue += RegenerationIncrement;
        if (RegenerationValue >= 1d)
        {
            RegenerationValue -= 1d;
            Health += 1;
        }

        if (Health == MaxHealth)
        {
            RegenerationValue = 0;
        }
    }

    private void RegenerateMana(Point position)
    {
        if (HungerPercentValue >= HungerBlocksManaRestore)
            return;

        var manaRegeneration = ManaRegeneration;
        if (Statuses.Contains(ManaDisturbedObjectStatus.StatusType))
        {
            manaRegeneration = (int)Math.Floor(manaRegeneration / 2d);
        }

        if (Mana < MaxMana && !Statuses.Contains(HungryObjectStatus.StatusType))
        {
            var cell = CurrentGame.Map.GetCell(position);
            var manaToRegenerate = Math.Min(manaRegeneration, cell.MagicEnergyLevel());
            cell.Environment.Cast().MagicEnergyLevel -= manaToRegenerate;
            Mana += manaToRegenerate;
        }
    }

    private void IncrementHunger()
    {
        HungerPercentValue = Math.Min(100d, HungerPercentValue + HungerIncrement);
        if (HungerPercentValue >= 100d)
        {
            Statuses.Add(new HungryObjectStatus());
        }
    }

    private void CheckOverweight()
    {
        var weight = Inventory.GetWeight();
        if (weight > MaxCarryWeight)
        {
            Statuses.Add(new OverweightObjectStatus());
        }
    }

    public override void OnDeath(Point position)
    {
        _logger.LogDebug("Player is dead");

        base.OnDeath(position);

        Died?.Invoke(this, EventArgs.Empty);
    }

    public override ObjectSize Size => ObjectSize.Medium;

    public override int GetProtection(Element element)
    {
        var value = base.GetProtection(element) + Equipment.GetProtection(element, Inventory);
        return Math.Min(MaxProtection, value);
    }

    public ILightSource[] LightSources => Equipment.GetLightSources(Inventory);

    public ISymbolsImage GetWorldImage(IImagesStorageService storage)
    {
        var body = GetBodyImage(storage);

        var equipmentImage = GetEquipmentImage(storage, body.Width, body.Height);
        body = SymbolsImage.Combine(body, equipmentImage);

        var directionImage = GetDirectionImage(storage);

        return SymbolsImage.Combine(body, directionImage);
    }

    private ISymbolsImage GetDirectionImage(IImagesStorageService storage)
    {
        var facingPosition = Point.GetPointInDirection(CurrentGame.PlayerPosition, Direction);
        var facingUsable = (CurrentGame.Map.TryGetCell(facingPosition)?.Objects.OfType<IUsableObject>())?.Any(usable => usable.CanUse) ?? false;

        switch (Direction)
        {
            case Direction.North:
                return storage.GetImage(facingUsable ? ImageUpUsable : ImageUp);
            case Direction.South:
                return storage.GetImage(facingUsable ? ImageDownUsable : ImageDown);
            case Direction.West:
                return storage.GetImage(facingUsable ? ImageLeftUsable : ImageLeft);
            case Direction.East:
                return storage.GetImage(facingUsable ? ImageRightUsable : ImageRight);
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private ISymbolsImage GetEquipmentImage(IImagesStorageService storage, int width, int height)
    {
        ISymbolsImage result = new SymbolsImage(width, height);

        var equippedImages = Equipment.GetEquippedItems(Inventory)
            .OfType<IEquippedImageProvider>()
            .OrderBy(item => item.EquippedImageOrder)
            .Select(item => item.GetEquippedImage(this, storage))
            .Where(image => image != null);

        foreach (var image in equippedImages)
        {
            result = SymbolsImage.Combine(result, image);
        }

        return result;
    }

    private ISymbolsImage GetBodyImage(IImagesStorageService storage)
    {
        var body = storage.GetImage(ImageBody);

        if (Equipment.RightHandItemEquipped)
        {
            var rightHandImage = storage.GetImage(ImageBodyRightWeapon);
            body = SymbolsImage.Combine(body, rightHandImage);
        }

        if (Equipment.LeftHandItemEquipped)
        {
            var leftHandImage = storage.GetImage(ImageBodyLeftWeapon);
            body = SymbolsImage.Combine(body, leftHandImage);
        }

        return body;
    }
}
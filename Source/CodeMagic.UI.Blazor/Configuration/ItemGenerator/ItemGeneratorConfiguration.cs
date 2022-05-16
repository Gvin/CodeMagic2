using CodeMagic.Game.Items.ItemsGeneration.Configuration;
using CodeMagic.Game.Items.ItemsGeneration.Configuration.Armor;
using CodeMagic.Game.Items.ItemsGeneration.Configuration.Bonuses;
using CodeMagic.Game.Items.ItemsGeneration.Configuration.Shield;
using CodeMagic.Game.Items.ItemsGeneration.Configuration.SpellBook;
using CodeMagic.Game.Items.ItemsGeneration.Configuration.Weapon;
using Newtonsoft.Json;

namespace CodeMagic.UI.Blazor.Configuration.ItemGenerator;

[Serializable]
public class ItemGeneratorConfiguration : IItemGeneratorConfiguration
{
    [JsonConverter(typeof(FixedJsonTypeConverter<WeaponsConfiguration>))]
    public IWeaponsConfiguration? WeaponsConfiguration { get; set; }

    [JsonConverter(typeof(FixedJsonTypeConverter<ArmorConfiguration>))]
    public IArmorConfiguration? ArmorConfiguration { get; set; }

    [JsonConverter(typeof(FixedJsonTypeConverter<ShieldsConfiguration>))]
    public IShieldsConfiguration? ShieldsConfiguration { get; set; }

    [JsonConverter(typeof(FixedJsonTypeConverter<SpellBooksConfiguration>))]
    public ISpellBooksConfiguration? SpellBooksConfiguration { get; set; }

    [JsonConverter(typeof(FixedJsonTypeConverter<BonusesConfiguration>))]
    public IBonusesConfiguration? BonusesConfiguration { get; set; }
}

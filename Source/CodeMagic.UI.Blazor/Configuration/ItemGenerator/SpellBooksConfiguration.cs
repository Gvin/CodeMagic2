using CodeMagic.Core.Items;
using CodeMagic.Game.Items.ItemsGeneration.Configuration.SpellBook;
using Newtonsoft.Json;

namespace CodeMagic.UI.Blazor.Configuration.ItemGenerator;

[Serializable]
public class SpellBooksConfiguration : ISpellBooksConfiguration
{
    public string? Template { get; set; }

    public string[]? SymbolImages { get; set; }

    [JsonConverter(typeof(FixedJsonTypeConverter<SpellBookRarenessConfiguration[]>))]
    public ISpellBookRarenessConfiguration[]? Configuration { get; set; }

    public int Weight { get; set; }
}

[Serializable]
public class SpellBookRarenessConfiguration : ISpellBookRarenessConfiguration
{
    public ItemRareness Rareness { get; set; }

    public int MinBonuses { get; set; }

    public int MaxBonuses { get; set; }

    public int MinSpells { get; set; }

    public int MaxSpells { get; set; }

    public string[]? Description { get; set; }
}
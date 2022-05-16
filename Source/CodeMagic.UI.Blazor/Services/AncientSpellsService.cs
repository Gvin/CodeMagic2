using CodeMagic.Game.Items.ItemsGeneration;
using CodeMagic.Game.Spells;

namespace CodeMagic.UI.Blazor.Services;

public class AncientSpellsService : IAncientSpellsService
{
    private readonly IFilesLoadService _filesLoadService;

    private BookSpell[] _uncommonSpells;
    private BookSpell[] _rareSpells;

    public AncientSpellsService(IFilesLoadService filesLoadService)
    {
        _uncommonSpells = Array.Empty<BookSpell>();
        _rareSpells = Array.Empty<BookSpell>();

        _filesLoadService = filesLoadService;
    }

    public async Task Initialize()
    {
        _uncommonSpells = new[]
        {
            new BookSpell
            {
                Name = "Thor's Hands",
                ManaCost = 300,
                Code = await LoadSpellCode("AncientSpell_ThorsHands")
            },
            new BookSpell
            {
                Name = "Lighter",
                ManaCost = 1000,
                Code = await LoadSpellCode("AncientSpell_Lighter")
            }
        };

        _rareSpells = new[]
        {
            new BookSpell
            {
                Name = "Shield",
                ManaCost = 1000,
                Code = await LoadSpellCode("AncientSpell_Shield")
            },
            new BookSpell
            {
                Name = "Fireball",
                ManaCost = 500,
                Code = await LoadSpellCode("AncientSpell_Fireball")
            }
        };
    }

    private Task<string> LoadSpellCode(string spellName)
    {
        var filePath = $"./resources/ancient_spells/{spellName}.js";
        return _filesLoadService.LoadFileAsStringAsync(filePath);
    }

    public BookSpell[] GetUncommonSpells()
    {
        return _uncommonSpells;
    }

    public BookSpell[] GetRareSpells()
    {
        return _rareSpells;
    }
}
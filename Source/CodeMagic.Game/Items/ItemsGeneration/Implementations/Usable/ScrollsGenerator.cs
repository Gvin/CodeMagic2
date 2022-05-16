using System;
using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Game.Items.Usable;
using CodeMagic.Game.Spells;

namespace CodeMagic.Game.Items.ItemsGeneration.Implementations.Usable
{
    public class ScrollsGenerator : IUsableItemTypeGenerator
    {
        private const char DamagedSymbol = '▒';
        private const string NewLineSign = "\r\n";
        private const int MinDamageZoneLength = 3;
        private const int MaxDamageZoneLength = 10;

        private const string AncientImageInventory1 = "Item_Scroll_Old_V1";
        private const string AncientImageInventory2 = "Item_Scroll_Old_V2";
        private const string AncientImageInventory3 = "Item_Scroll_Old_V3";

        private const int UncommonMinDamage = 15;
        private const int UncommonMaxDamage = 30;

        private const int RareMinDamage = 25;
        private const int RareMaxDamage = 50;

        private const int ScrollWeight = 300;

        private readonly IAncientSpellsService _spellsService;
        private readonly Dictionary<BookSpell, string> _spellNamesCache;

        public ScrollsGenerator(IAncientSpellsService spellsService)
        {
            _spellsService = spellsService;
            _spellNamesCache = new Dictionary<BookSpell, string>();
        }

        public IItem Generate(ItemRareness rareness)
        {
            switch (rareness)
            {
                case ItemRareness.Trash:
                case ItemRareness.Common:
                    return null;
                case ItemRareness.Uncommon:
                    return GenerateScroll(rareness, _spellsService.GetUncommonSpells(), UncommonMinDamage, UncommonMaxDamage);
                case ItemRareness.Rare:
                    return GenerateScroll(rareness, _spellsService.GetRareSpells(), RareMinDamage, RareMaxDamage);
                case ItemRareness.Epic:
                    throw new ArgumentException("Scrolls generator cannot generate scroll with Epic rareness.");
                default:
                    throw new ArgumentException($"Unknown rareness: {rareness}");
            }
        }

        public void Reset()
        {
        }

        private IItem GenerateScroll(ItemRareness rareness, BookSpell[] spells, int minDamage, int maxDamage)
        {
            var spell = RandomHelper.GetRandomElement(spells);
            var damage = RandomHelper.GetRandomValue(minDamage, maxDamage);
            var name = GetName(spell);

            return new AncientScroll
            {
                Name = $"{name} Scroll",
                Code = spell.Code,
                Mana = spell.ManaCost,
                DamagePercent = damage,
                Key = Guid.NewGuid().ToString(),
                Rareness = rareness,
                SpellName = name,
                DamagedCode = GenerateDamagedCode(spell.Code, damage),
                InventoryImageName = GetAncientScrollInventoryImageName(spell.Code),
                Weight = ScrollWeight
            };
        }

        private static string GetAncientScrollInventoryImageName(string code)
        {
            var letterA = code.Count(c => char.ToLower(c) == 'a');
            var letterB = code.Count(c => char.ToLower(c) == 'b');
            var letterC = code.Count(c => char.ToLower(c) == 'c');

            if (letterA > letterB && letterA > letterC)
                return AncientImageInventory1;
            if (letterB > letterA && letterB > letterC)
                return AncientImageInventory2;
            return AncientImageInventory3;
        }

        private static string GenerateDamagedCode(string code, int damagePercent)
        {
            var lines = code.Split(new[] { NewLineSign }, StringSplitOptions.None);
            var totalSymbolsCount = lines.Sum(line => line.Length);
            var remainingDamageSymbols = (int)Math.Round(totalSymbolsCount * damagePercent / 100d);

            const int maxIterations = 1000;
            var iteration = 0;

            while (remainingDamageSymbols > 0 && iteration < maxIterations)
            {
                iteration++;

                var damageZoneLength = RandomHelper.GetRandomValue(MinDamageZoneLength, MaxDamageZoneLength);
                var lineIndex = RandomHelper.GetRandomValue(0, lines.Length - 1);
                var line = lines[lineIndex];
                if (line.Length < damageZoneLength)
                    continue;

                var zoneStartPosition = RandomHelper.GetRandomValue(0, line.Length - 1 - damageZoneLength);
                if (line[zoneStartPosition] == DamagedSymbol)
                    continue;

                for (var index = 0; index < damageZoneLength; index++)
                {
                    var onLineIndex = index + zoneStartPosition;
                    line = line.Remove(onLineIndex, 1);
                    line = line.Insert(onLineIndex, DamagedSymbol.ToString());
                }

                lines[lineIndex] = line;
                remainingDamageSymbols -= damageZoneLength;
            }

            return string.Join(NewLineSign, lines);
        }

        private string GetName(BookSpell spell)
        {
            if (_spellNamesCache.ContainsKey(spell))
                return _spellNamesCache[spell];

            var name = GenerateName();
            _spellNamesCache.Add(spell, name);
            return name;
        }

        private static readonly char[] CapitalLetters =
        {
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L',
            'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X',
            'Y', 'Z', 'Ç', 'Ä', 'Å', 'È', 'Æ', 'Ö', 'Ü', 'Γ', 'Σ', 'Θ',
            'Ω', 'Φ', 'Π', 'Ñ'
        };

        private static readonly char[] SmallLetters =
        {
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l',
            'm', 'm', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x',
            'y', 'z', 'ϋ', 'â', 'ä', 'à', 'å', 'ç', 'ê', 'ë', 'è',
            'ï', 'î', 'ì', 'æ', 'ô', 'ö', 'ò', 'û', 'ù', 'ÿ', 'α', 'β',
            'π', 'σ', 'μ', 'γ', 'θ', 'δ', 'φ', 'ε', ' ', '\'', '`', 'ñ'
        };

        private const int MinNameLength = 4;
        private const int MaxNameLength = 10;

        private string GenerateName()
        {
            var nameLength = RandomHelper.GetRandomValue(MinNameLength, MaxNameLength);

            var name = RandomHelper.GetRandomElement(CapitalLetters).ToString();
            for (var counter = 1; counter < nameLength; counter++)
            {
                name += RandomHelper.GetRandomElement(SmallLetters);
            }

            return name;
        }
    }
}
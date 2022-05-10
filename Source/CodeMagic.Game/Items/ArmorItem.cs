using System;
using System.Collections.Generic;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Game.Objects.Creatures;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Items
{
    public class ArmorItem : DurableItem, IArmorItem, IInventoryImageProvider, IDescriptionProvider, IWorldImageProvider, IEquippedImageProvider
    {
        public ISymbolsImage InventoryImage { get; set; }

        public ISymbolsImage WorldImage { get; set; }

        public ISymbolsImage EquippedImage { get; set; }

        public Dictionary<Element, int> Protection { get; set; }

        public int EquippedImageOrder => (int) ArmorType;

        public ArmorType ArmorType { get; set; }

        public int GetProtection(Element element)
        {
            return Protection.ContainsKey(element) ? Protection[element] : 0;
        }

        public ISymbolsImage GetInventoryImage(IImagesStorage storage)
        {
            return InventoryImage;
        }

        public StyledLine[] GetDescription(Player player)
        {
            var equipedArmor = player.Equipment.GetEquipedArmor(ArmorType, player.Inventory);

            var result = new List<StyledLine>();

            if (equipedArmor == null || Equals(equipedArmor))
            {
                result.Add(TextHelper.GetWeightLine(Weight));
            }
            else
            {
                result.Add(TextHelper.GetCompareWeightLine(Weight, equipedArmor.Weight));
            }

            result.Add(StyledLine.Empty);

            result.Add(TextHelper.GetDurabilityLine(Durability, MaxDurability));

            result.Add(StyledLine.Empty);

            AddProtectionDescription(result, equipedArmor);

            result.Add(StyledLine.Empty);

            TextHelper.AddBonusesDescription(this, equipedArmor, result);

            result.Add(StyledLine.Empty);

            result.AddRange(TextHelper.ConvertDescription(Description));

            return result.ToArray();
        }

        private void AddProtectionDescription(List<StyledLine> descriptionResult, IArmorItem equipedArmor)
        {
            var equiped = Equals(equipedArmor);
            foreach (Element element in Enum.GetValues(typeof(Element)))
            {
                var value = GetProtection(element);
                var equipedValue = equipedArmor?.GetProtection(element) ?? 0;

                if (value != 0 || equipedValue != 0)
                {
                    var protectionLine = new StyledLine
                    {
                        new StyledString($"{TextHelper.GetElementName(element)}",
                            TextHelper.GetElementColor(element)),
                        " Protection: "
                    };

                    if (equiped)
                    {
                        protectionLine.Add(TextHelper.GetValueString(value, "%"));
                    }
                    else
                    {
                        protectionLine.Add(TextHelper.GetCompareValueString(value, equipedValue, "%"));
                    }

                    descriptionResult.Add(protectionLine);
                }
            }
        }

        public ISymbolsImage GetWorldImage(IImagesStorage storage)
        {
            return WorldImage;
        }

        public ISymbolsImage GetEquippedImage(Player player, IImagesStorage imagesStorage)
        {
            return EquippedImage;
        }
    }
}
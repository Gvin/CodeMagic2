using System.Collections.Generic;
using System.Drawing;
using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Items.Custom
{
    public class BanHammer : WeaponItem
    {
        public BanHammer()
        {
            MaxDurability = 100000000;
            Description = new[]
            {
                "Powerful weapon designed to give his owner",
                "the power of God. For testing purpose mostly."
            };
            Accuracy = 100;
            LightPower = LightLevel.Medium;

            MinDamage = new Dictionary<Element, int>
            {
                { Element.Acid, 100 },
                { Element.Blunt, 100 },
                { Element.Electricity, 100 },
                { Element.Fire, 100 },
                { Element.Frost, 100 },
                { Element.Piercing, 100 },
                { Element.Slashing, 100 },
                { Element.Magic, 100 }
            };
            MaxDamage = new Dictionary<Element, int>
            {
                { Element.Acid, 200 },
                { Element.Blunt, 200 },
                { Element.Electricity, 200 },
                { Element.Fire, 200 },
                { Element.Frost, 200 },
                { Element.Piercing, 200 },
                { Element.Slashing, 200 },
                { Element.Magic, 200 }
            };
        }

        public override string Name
        {
            get => "Ban Hammer";
            set 
            { 
                // Do nothing
            }
        }

        public override string Key => "weapon_ban_hammer";

        public override int Weight
        {
            get => 0;
            set
            {
                // Do nothing
            }
        }

        public override ItemRareness Rareness
        {
            get => ItemRareness.Epic;
            set
            {
                // Do nothing
            }
        }

        public override ISymbolsImage GetInventoryImage(IImagesStorage storage)
        {
            return storage.GetImage("Weapon_BanHammer");
        }

        protected override ISymbolsImage GetLeftEquippedImage(IImagesStorage storage)
        {
            return RecolorImage(storage.GetImage("ItemOnPlayer_Weapon_Left_Mace"));
        }

        protected override ISymbolsImage GetRightEquippedImage(IImagesStorage storage)
        {
            return RecolorImage(storage.GetImage("ItemOnPlayer_Weapon_Right_Mace"));
        }

        public override ISymbolsImage GetWorldImage(IImagesStorage storage)
        {
            return RecolorImage(storage.GetImage("ItemsOnGround_Weapon_Mace"));
        }

        private ISymbolsImage RecolorImage(ISymbolsImage image)
        {
            var palette = new Dictionary<Color, Color>
            {
                {Color.FromArgb(255, 0, 0), Color.MediumVioletRed}
            };
            return SymbolsImage.Recolor(image, palette);
        }
    }
}
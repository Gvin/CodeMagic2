using System;
using System.Collections.Generic;
using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Game.Drawing;
using CodeMagic.Game.Objects;

namespace CodeMagic.Game.Items.Custom
{
    [Serializable]
    public class TorchItem : WeaponItem
    {
        private const string TorchInventoryImage = "Weapon_Torch";
        private const string TorchWorldImage = "ItemsOnGround_Torch";
        private const string TorchEquippedImageRight = "ItemOnPlayer_Weapon_Right_Torch";
        private const string TorchEquippedImageLeft = "ItemOnPlayer_Weapon_Left_Torch";

        private readonly SymbolsAnimationsManager _animation;

        public TorchItem()
        {
            Accuracy = 50;
            MinDamage = new Dictionary<Element, int>
            {
                { Element.Fire, 1 },
                { Element.Blunt, 1 }
            };
            MaxDamage = new Dictionary<Element, int>
            {
                { Element.Fire, 3 },
                { Element.Blunt, 3 }
            };
            LightPower = LightLevel.Dim1;
            Description = new[] { "Rude torch made from wood and clothes." };
            MaxDurability = 20;

            _animation = new SymbolsAnimationsManager(TimeSpan.FromMilliseconds(500),
                AnimationFrameStrategy.OneByOneStartFromRandom);
        }

        public override string Key
        {
            get => "torch";
            set
            {
                // Do nothing
            }
        }
        public override ItemRareness Rareness
        {
            get => ItemRareness.Common;
            set
            {
                // Do nothing
            }
        }

        public override int Weight
        {
            get => 1500;
            set
            {
                // Do nothing
            }
        }

        public override string Name
        {
            get => "Torch";
            set
            {
                // Do nothing
            }
        }

        public override ISymbolsImage GetInventoryImage(IImagesStorageService storage)
        {
            return storage.GetImage(TorchInventoryImage);
        }

        public override ISymbolsImage GetWorldImage(IImagesStorageService storage)
        {
            return storage.GetImage(TorchWorldImage);
        }

        protected override ISymbolsImage GetRightEquippedImage(IImagesStorageService storage)
        {
            return _animation.GetImage(storage, TorchEquippedImageRight);
        }

        protected override ISymbolsImage GetLeftEquippedImage(IImagesStorageService storage)
        {
            return _animation.GetImage(storage, TorchEquippedImageLeft);
        }
    }
}
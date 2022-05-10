using System;
using CodeMagic.Core.Objects;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Objects.SolidObjects
{
    [Serializable]
    public class DungeonStairs : MapObjectBase, IWorldImageProvider
    {
        private const string ImageName = "Stairs_Up";

        public override string Name => "Stairs";

        public override ZIndex ZIndex => ZIndex.BigDecoration;

        public override ObjectSize Size => ObjectSize.Huge;

        public ISymbolsImage GetWorldImage(IImagesStorage storage)
        {
            return storage.GetImage(ImageName);
        }
    }
}
using System;
using CodeMagic.Core.Game;
using CodeMagic.Core.Objects;
using CodeMagic.Game.JournalMessages;
using CodeMagic.Game.MapGeneration.Dungeon;
using CodeMagic.Game.Objects.Creatures;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Objects.SolidObjects
{
    [Serializable]
    public class DungeonTrapDoor : MapObjectBase, IUsableObject, IWorldImageProvider
    {
        private const string ImageName = "Decoratives_TrapDoor";

        public override string Name => "Trap Door";

        public override ZIndex ZIndex => ZIndex.BigDecoration;

        public ISymbolsImage GetWorldImage(IImagesStorage storage)
        {
            return storage.GetImage(ImageName);
        }

        public override ObjectSize Size => ObjectSize.Huge;

        public bool CanUse => true;

        public void Use(GameCore<Player> game, Point position)
        {
            DialogsManager.Provider.OpenWaitDialog("Descending...", () =>
            {
                var (newMap, newPlayerPosition) = DungeonMapGenerator.Current.GenerateNewMap(game.Map.Level + 1);
                game.ChangeMap(newMap, newPlayerPosition);
                game.Map.Refresh();
                game.Journal.Write(new DungeonLevelMessage(game.Map.Level));
            });
        }
    }
}
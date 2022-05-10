using CodeMagic.Core.Game;
using CodeMagic.Core.Objects;
using CodeMagic.Game.Objects.Floor;
using CodeMagic.Game.Objects.SolidObjects;

namespace CodeMagic.Game.MapGeneration.Dungeon.MapObjectFactories
{
    public class CaveMapObjectsFactory : IMapObjectFactory
    {
        public IMapObject CreateFloor()
        {
            return FloorObject.Create(FloorObject.Type.Stone);
        }

        public IMapObject CreateStairs()
        {
            return new DungeonStairs();
        }

        public IMapObject CreateDoor()
        {
            return new DungeonDoor();
        }

        public IMapObject CreateTrapDoor()
        {
            return new DungeonTrapDoor();
        }

        public IMapObject CreateWall()
        {
            return new CaveWall();
        }

        public IMapObject CreateTorchWall()
        {
            return DungeonTorchWall.Create();
        }

        public IMapObject CreateWall(int torchChance)
        {
            if (RandomHelper.CheckChance(torchChance))
            {
                return CreateTorchWall();
            }

            return CreateWall();
        }
    }
}
using System.Linq;
using CodeMagic.Core.Objects;

namespace CodeMagic.Core.Area
{
    public abstract class AreaMapCellBase : IAreaMapCell
    {
        protected AreaMapCellBase()
        {
            ObjectsCollection = new MapObjectsCollection();
            LightLevel = LightLevel.Darkness;
        }

        public IEnvironment Environment { get; set; }

        IMapObject[] IAreaMapCell.Objects => ObjectsCollection.ToArray();

        public MapObjectsCollection ObjectsCollection { get; set; }

        public LightLevel LightLevel { get; set; }

        public bool BlocksMovement => ObjectsCollection.Any(obj => obj.BlocksMovement);

        public bool BlocksEnvironment => ObjectsCollection.Any(obj => obj.BlocksEnvironment);

        public bool BlocksVisibility => ObjectsCollection.Any(obj => obj.BlocksVisibility);

        public bool BlocksProjectiles => ObjectsCollection.Any(obj => obj.BlocksProjectiles);

        public int GetVolume<T>() where T : IVolumeObject
        {
            return ObjectsCollection.GetVolume<T>();
        }

        public void RemoveVolume<T>(int volume) where T : IVolumeObject
        {
            ObjectsCollection.RemoveVolume<T>(volume);
        }

        public IDestroyableObject GetBiggestDestroyable()
        {
            var destroyable = ObjectsCollection.OfType<IDestroyableObject>().ToArray();
            var bigDestroyable = destroyable.FirstOrDefault(obj => obj.BlocksMovement);
            if (bigDestroyable != null)
                return bigDestroyable;
            return destroyable.LastOrDefault();
        }
    }
}
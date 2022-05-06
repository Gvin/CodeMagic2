using CodeMagic.Core.Area;
using CodeMagic.Core.Game;

namespace CodeMagic.Game.MapGeneration.Dungeon.MapGenerators
{
    internal interface IMapAreaGenerator
    {
        (IAreaMap map, Point playerPosition) Generate(int level, MapSize size);
    }
}
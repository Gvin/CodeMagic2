using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using CodeMagic.Core.Area;
using CodeMagic.Core.Common;
using CodeMagic.Core.Game;
using CodeMagic.Game.Objects.SolidObjects;
using CodeMagic.Game.MapGeneration.Dungeon.MapGenerators;
using CodeMagic.Game.MapGeneration.Dungeon.MapObjectFactories;
using CodeMagic.Game.MapGeneration.Dungeon.MonstersGenerators;
using Microsoft.Extensions.Logging;
using Point = CodeMagic.Core.Game.Point;

namespace CodeMagic.Game.MapGeneration.Dungeon
{
    public interface IDungeonMapGenerator
    {
        (IAreaMap map, Point playerPosition) GenerateNewMap(int level);
    }

    public class DungeonMapGenerator : IDungeonMapGenerator
    {
        private readonly ILogger<DungeonMapGenerator> _logger;
        private readonly Dictionary<MapType, IMapAreaGenerator> _generators;

        public DungeonMapGenerator(IImagesStorage imagesStorage, IPerformanceMeter performanceMeter, ILogger<DungeonMapGenerator> logger)
        {
            _logger = logger;

            _generators = new Dictionary<MapType, IMapAreaGenerator>
            {
                {MapType.Dungeon, new DungeonRoomsMapGenerator(
                    new DungeonMapObjectsFactory(), 
                    new ObjectsGenerators.DungeonObjectsGenerator(imagesStorage), 
                    new DungeonMonstersGenerator(performanceMeter),
                    performanceMeter)},
                {MapType.Labyrinth, new LabyrinthMapGenerator(new DungeonMapObjectsFactory(), performanceMeter)},
                {MapType.Cave, new CaveDungeonMapGenerator(new CaveMapObjectsFactory(), performanceMeter)}
            };
        }

        public (IAreaMap map, Point playerPosition) GenerateNewMap(int level)
        {
            _logger.LogDebug($"Generating map for level {level}.");
            var stopwatch = Stopwatch.StartNew();

            try
            {
                return PerformMapGeneration(level);
            }
            finally
            {
                stopwatch.Stop();
                _logger.LogDebug($"Map generation took {stopwatch.ElapsedMilliseconds} milliseconds total.");
            }
        }

        private (IAreaMap map, Point playerPosition) PerformMapGeneration(int level)
        {
            for (var attempt = 0; attempt < 500; attempt++)
            {
                try
                {
                    var size = GenerateMapSize();
                    var result = GenerateMap(level, size);
                    _logger.LogInformation($"Map was generated for {attempt + 1} attempts.");
                    return result;
                }
                catch (MapGenerationException ex)
                {
                    _logger.LogDebug(ex, "Error when generating map.");
                }
            }

            throw new ApplicationException("Unable to generate map.");
        }

        private MapSize GenerateMapSize()
        {
            var value = RandomHelper.GetRandomValue(0, 100);

            if (value >= 0 && value <= 25)
                return MapSize.Big; // 25%
            if (value > 25 && value <= 62)
                return MapSize.Medium; // 37%

            return MapSize.Small; // 38%
        }

        private (IAreaMap map, Point playerPosition) GenerateMap(
            int level,
            MapSize size)
        {
            var mapType = GenerateMapType(level);
            var generator = _generators[mapType];
            return generator.Generate(level, size);
        }

        private void WriteMapToFile(IAreaMap map, Point playerPosition)
        {
            using (var file = File.CreateText(@".\Map.txt"))
            {
                for (int y = 0; y < map.Height; y++)
                {
                    var line = string.Empty;
                    for (int x = 0; x < map.Width; x++)
                    {
                        
                        line += GetCellSymbol(x, y, map, playerPosition);
                    }
                    file.WriteLine(line);
                }
            }
        }

        private string GetCellSymbol(int x, int y, IAreaMap map, Point playerPosition)
        {
            if (playerPosition.X == x && playerPosition.Y == y)
            {
                return "+";
            }

            var cell = map.GetCell(x, y);

            if (cell.Objects.OfType<DoorBase>().Any())
            {
                return "▒";
            }

            if (cell.Objects.OfType<WallBase>().Any())
            {
                return "█";
            }

            if (cell.Objects.OfType<DungeonTrapDoor>().Any())
            {
                return "v";
            }

            if (cell.Objects.OfType<DungeonStairs>().Any())
            {
                return "^";
            }

            return " ";
        }

        private MapType GenerateMapType(int level)
        {
            // TODO: Delete only-dungeon
            return MapType.Dungeon;

            if (level == 1) // Always dungeons for the 1st level.
                return MapType.Dungeon;

            var chance = RandomHelper.GetRandomValue(0, 100);
            if (chance < 20) // 20%
                return MapType.Labyrinth;
            if (chance >= 20 && chance < 30) // 10%
                return MapType.Cave;
            return MapType.Dungeon; // 70%
        }
    }
    public enum MapType
    {
        Dungeon,
        Labyrinth,
        Cave
    }

    public enum MapSize
    {
        Small,
        Medium,
        Big
    }
}
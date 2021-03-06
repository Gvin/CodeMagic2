using System;
using System.Collections.Generic;
using CodeMagic.Core.Area;
using CodeMagic.Core.Common;
using CodeMagic.Core.Game;

namespace CodeMagic.Game.MapGeneration.Dungeon.ObjectsGenerators
{
    internal partial class DungeonObjectsGenerator : IObjectsGenerator
    {
        private readonly IPerformanceMeter _performanceMeter;
        private readonly ObjectsPattern[] _patterns;

        public DungeonObjectsGenerator(IImagesStorageService storage, IPerformanceMeter performanceMeter)
        {
            _performanceMeter = performanceMeter;
            _patterns = GetPatterns(storage);
        }

        private static ObjectsPattern[] GetPatterns(IImagesStorageService storage)
        {
            var patternsList = new List<ObjectsPattern>
            {
                // Table with 2 chairs
                CreateTableWithChairs(),
                // Single Shelf
                CreateShelfDown(),
                CreateShelfUp(),
                CreateShelfLeft(),
                CreateShelfRight(),
                // Crate
                CreateCrate(),
                // Chest
                CreateChest(),
                CreateGoldenChest(),
                // Water
                CreateWaterPool(),
                // Spiked Floor
                CreateSpikedFloor(),
                // Stone
                CreateStone()
            };

            return patternsList.ToArray();
        }

        public void GenerateObjects(IAreaMap map, Point playerPosition)
        {
            using var scope = _performanceMeter.Start($"{nameof(DungeonObjectsGenerator)}.{nameof(GenerateObjects)}");

            foreach (var pattern in _patterns)
            {
                var count = (int)Math.Floor(map.Width * map.Height * pattern.MaxCountMultiplier);
                for (var counter = 0; counter < count; counter++)
                {
                    AddPattern(map, playerPosition, pattern);
                }
            }
        }

        private void AddPattern(IAreaMap map, Point playerPosition, ObjectsPattern pattern)
        {
            const int maxAttempts = 100;
            var attempt = 0;
            while (attempt < maxAttempts)
            {
                attempt++;

                if (TryAddPattern(map, playerPosition, pattern))
                    return;
            }
        }

        private bool TryAddPattern(IAreaMap map, Point playerPosition, ObjectsPattern pattern)
        {
            var x = RandomHelper.GetRandomValue(0, map.Width);
            var y = RandomHelper.GetRandomValue(0, map.Height);

            if (!map.ContainsCell(x, y))
                return false;

            for (int cursorX = 0; cursorX < pattern.Width; cursorX++)
            {
                for (int cursorY = 0; cursorY < pattern.Height; cursorY++)
                {
                    var posX = cursorX + x;
                    var posY = cursorY + y;

                    if (posX == playerPosition.X && posY == playerPosition.Y)
                        return false;

                    var cell = map.TryGetCell(posX, posY);
                    if (cell == null)
                        return false;

                    if (!pattern.CheckRequirements(cursorX, cursorY, cell))
                        return false;
                }
            }

            for (int cursorX = 0; cursorX < pattern.Width; cursorX++)
            {
                for (int cursorY = 0; cursorY < pattern.Height; cursorY++)
                {
                    var posX = cursorX + x;
                    var posY = cursorY + y;

                    foreach (var factory in pattern.Get(cursorX, cursorY))
                    {
                        map.AddObject(new Point(posX, posY), factory(map.Level));
                    }
                }
            }

            return true;
        }
    }
}
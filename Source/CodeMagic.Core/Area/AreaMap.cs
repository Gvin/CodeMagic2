using System;
using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Common;
using CodeMagic.Core.Game;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Saving;

namespace CodeMagic.Core.Area
{
    public class AreaMap : IAreaMap
    {
        private const string SaveKeyLevel = "Level";
        private const string SaveKeyWidth = "Width";
        private const string SaveKeyHeight = "Height";
        private const string SaveKeyCells = "Cells";

        private readonly Dictionary<Type, Point> _objectPositionCache;
        private readonly IAreaMapCellInternal[][] _cells;
        private readonly Dictionary<string, IDestroyableObject> _destroyableObjects;
        private readonly IMapLightLevelProcessor _mapLightLevelProcessor;
        private readonly IPerformanceMeter _performanceMeter;

        public AreaMap(SaveData dataBuilder, IPerformanceMeter performanceMeter)
        {
            _performanceMeter = performanceMeter;
            _mapLightLevelProcessor = new MapLightLevelProcessor();

            _objectPositionCache = new Dictionary<Type, Point>();
            _destroyableObjects = new Dictionary<string, IDestroyableObject>();

            Level = dataBuilder.GetIntValue(SaveKeyLevel);
            Width = dataBuilder.GetIntValue(SaveKeyWidth);
            Height = dataBuilder.GetIntValue(SaveKeyHeight);

            _cells = dataBuilder.GetObject<GridSaveable>(SaveKeyCells).Rows.Select(row => row.Cast<IAreaMapCellInternal>().ToArray()).ToArray();

            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    var cell = _cells[y][x];
                    foreach (var destroyable in cell.ObjectsCollection.OfType<IDestroyableObject>())
                    {
                        _destroyableObjects.Add(destroyable.Id, destroyable);
                    }
                }
            }
        }

        public AreaMap(
            int level, 
            Func<IAreaMapCellInternal> cellFactory, 
            int width, 
            int height, IPerformanceMeter performanceMeter, IMapLightLevelProcessor mapLightLevelProcessor = null)
        {
            this._mapLightLevelProcessor = mapLightLevelProcessor ?? new MapLightLevelProcessor();

            _objectPositionCache = new Dictionary<Type, Point>();
            _destroyableObjects = new Dictionary<string, IDestroyableObject>();

            Level = level;
            Width = width;
            Height = height;
            _performanceMeter = performanceMeter;

            _cells = new IAreaMapCellInternal[height][];
            for (var y = 0; y < height; y++)
            {
                _cells[y] = new IAreaMapCellInternal[width];
                for (var x = 0; x < width; x++)
                {
                    _cells[y][x] = cellFactory();
                }
            }
        }

        public SaveDataBuilder GetSaveData()
        {
            var grid = new GridSaveable(_cells.Cast<object[]>().ToArray());
            return new SaveDataBuilder(GetType(), new Dictionary<string, object>
            {
                {SaveKeyLevel, Level},
                {SaveKeyWidth, Width},
                {SaveKeyHeight, Height},
                {SaveKeyCells, grid}
            });
        }

        public IDestroyableObject GetDestroyableObject(string id)
        {
            if (_destroyableObjects.ContainsKey(id))
                return _destroyableObjects[id];

            return null;
        }

        public int Level { get; }

        public int Width { get; }

        public int Height { get; }

        public IAreaMapCell GetCell(int x, int y)
        {
            if (x < 0 || x >= Width)
                throw new ArgumentOutOfRangeException(nameof(x), x, $"Coordinate X value is {x} which doesn't match map size {Width}");
            if (y < 0 || y >= Height)
                throw new ArgumentOutOfRangeException(nameof(y), y, $"Coordinate Y value is {y} which doesn't match map size {Height}");

            return _cells[y][x];
        }

        public IAreaMapCell TryGetCell(Point position)
        {
            return TryGetCell(position.X, position.Y);
        }

        public IAreaMapCell TryGetCell(int x, int y)
        {
            if (!ContainsCell(x, y))
                return null;

            return GetCell(x, y);
        }

        public void AddObject(int x, int y, IMapObject @object)
        {
            AddObject(new Point(x, y), @object);
        }

        public void AddObject(Point position, IMapObject @object)
        {
            if (@object is IDestroyableObject destroyableObject)
            {
                _destroyableObjects.Add(destroyableObject.Id, destroyableObject);
            }

            GetOriginalCell(position.X, position.Y).ObjectsCollection.Add(@object);

            if (@object is IPlacedHandler placedHandler)
            {
                placedHandler.OnPlaced(this, position);
            }
        }

        public void RemoveObject(Point position, IMapObject @object)
        {
            if (@object is IDestroyableObject destroyableObject)
            {
                _destroyableObjects.Remove(destroyableObject.Id);
            }

            GetOriginalCell(position.X, position.Y).ObjectsCollection.Remove(@object);
        }

        public Point GetObjectPosition<T>() where T : IMapObject
        {
            var type = typeof(T);
            if (_objectPositionCache.ContainsKey(type))
                return _objectPositionCache[type];

            var position = GetObjectPosition(obj => obj is T);
            _objectPositionCache.Add(type, position);
            return position;
        }

        public Point GetObjectPosition(Func<IMapObject, bool> selector)
        {
            for (var y = 0; y < _cells.Length; y++)
            {
                var row = _cells[y];
                for (var x = 0; x < row.Length; x++)
                {
                    var cell = row[x];
                    if (cell.ObjectsCollection.Any(selector))
                        return new Point(x, y);
                }
            }

            return null;
        }

        private IAreaMapCellInternal GetOriginalCell(int x, int y)
        {
            return (IAreaMapCellInternal) GetCell(x, y);
        }

        public IAreaMapCell GetCell(Point point)
        {
            return GetCell(point.X, point.Y);
        }

        public bool ContainsCell(int x, int y)
        {
            return x >= 0 && x < Width && y >= 0 && y < Height;
        }

        public bool ContainsCell(Point point)
        {
            return ContainsCell(point.X, point.Y);
        }

        public IAreaMapCell[][] GetMapPart(Point position, int radius)
        {
            var startIndexX = position.X - radius;
            var startIndexY = position.Y - radius;
            var visionDiameter = radius * 2 + 1;

            var result = new IAreaMapCell[visionDiameter][];
            for (var y = 0; y < visionDiameter; y++)
            {
                result[y] = new IAreaMapCell[visionDiameter];
                for (var x = 0; x < visionDiameter; x++)
                {
                    result[y][x] = TryGetCell(startIndexX + x, startIndexY + y);
                }
            }

            return result;
        }

        public void Refresh()
        {
            _mapLightLevelProcessor.ResetLightLevel(this);
            _mapLightLevelProcessor.UpdateLightLevel(this);
        }

        public void PreUpdate()
        {
            PreUpdateCells();
        }

        public void Update(ITurnProvider turnProvider)
        {
            _objectPositionCache.Clear();

            using (_performanceMeter.Start($"Map_UpdateCells_Early[{turnProvider.CurrentTurn}]"))
            {
                UpdateCells(UpdateOrder.Early);
            }

            _mapLightLevelProcessor.ResetLightLevel(this);
            _mapLightLevelProcessor.UpdateLightLevel(this);

            using (_performanceMeter.Start($"Map_UpdateCells_Medium[{turnProvider.CurrentTurn}]"))
            {
                UpdateCells(UpdateOrder.Medium);
            }

            using (_performanceMeter.Start($"Map_UpdateCells_Late[{turnProvider.CurrentTurn}]"))
            {
                UpdateCells(UpdateOrder.Late);
            }

            using (_performanceMeter.Start($"Map_PostUpdateCells[{turnProvider.CurrentTurn}]"))
            {
                PostUpdateCells();
            }
        }

        private void UpdateCells(UpdateOrder order)
        {
            var cellCoordinates = new List<Point>();
            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    cellCoordinates.Add(new Point(x, y));
                }
            }

            while (cellCoordinates.Count > 0)
            {
                var position = RandomHelper.GetRandomElement<Point>(cellCoordinates);
                cellCoordinates.Remove(position);
                var cell = GetOriginalCell(position.X, position.Y);
                cell.Update(position, order);
            }
        }

        private void PreUpdateCells()
        {
            for (var y = 0; y < Height; y++)
            {
                var row = _cells[y];
                for (var x = 0; x < Width; x++)
                {
                    var cell = row[x];
                    var cellDestroyableObjects = cell.ObjectsCollection.OfType<IDestroyableObject>();
                    foreach (var destroyableObject in cellDestroyableObjects)
                    {
                        destroyableObject.ClearDamageRecords();
                    }
                }
            }
        }

        private void PostUpdateCells()
        {
            var mergedCells = new CellPairsStorage(Width, Height);
            for (var y = 0; y < _cells.Length; y++)
            {
                for (var x = 0; x < _cells[y].Length; x++)
                {
                    var position = new Point(x, y);
                    var cell = GetOriginalCell(x, y);
                    cell.Environment.Update(position, cell);
                    MergeCellEnvironment(position, cell, mergedCells);
                    cell.PostUpdate(this, position);
                    cell.ResetDynamicObjectsState();
                }
            }
        }

        private void MergeCellEnvironment(Point position, IAreaMapCellInternal cell, CellPairsStorage mergedCells)
        {
            if (cell.BlocksEnvironment)
                return;
            SpreadEnvironment(position, cell, mergedCells);
        }

        private void SpreadEnvironment(Point position, IAreaMapCellInternal cell, CellPairsStorage mergedCells)
        {
            TrySpreadEnvironment(position, Direction.North, cell, mergedCells);
            TrySpreadEnvironment(position, Direction.South, cell, mergedCells);
            TrySpreadEnvironment(position, Direction.West, cell, mergedCells);
            TrySpreadEnvironment(position, Direction.East, cell, mergedCells);
        }

        private void TrySpreadEnvironment(Point position, Direction direction, IAreaMapCellInternal cell, CellPairsStorage mergedCells)
        {
            var nextPosition = Point.GetPointInDirection(position, direction);
            if (!ContainsCell(nextPosition))
                return;

            var nextCell = GetOriginalCell(nextPosition.X, nextPosition.Y);

            if (mergedCells.ContainsPair(position, direction))
                return;
            mergedCells.RegisterPair(position, direction);

            if (nextCell.BlocksEnvironment)
                return;

            cell.CheckSpreading(nextCell);
            cell.Environment.Balance(cell, nextCell);
        }

        private class CellPairsStorage
        {
            private readonly List<Direction>[,] pairs;

            public CellPairsStorage(int width, int height)
            {
                pairs = new List<Direction>[width, height];
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        pairs[x, y] = new List<Direction>();
                    }
                }
            }

            public void RegisterPair(Point initialCell, Direction direction)
            {
                pairs[initialCell.X, initialCell.Y].Add(direction);
                var targetCell = Point.GetPointInDirection(initialCell, direction);
                var invertedDirection = DirectionHelper.InvertDirection(direction);
                pairs[targetCell.X, targetCell.Y].Add(invertedDirection);
            }

            public bool ContainsPair(Point initialCell, Direction direction)
            {
                return pairs[initialCell.X, initialCell.Y].Contains(direction);
            }
        }
    }
}
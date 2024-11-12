using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine.Serialization;

namespace Joymg.Patterns.Command
{
    [System.Serializable]
    public class Map
    {
        private Cell[][] cells;
        public Cell[][] Cells => cells;
        private readonly string _startingMap;
        public Map(string startingMap) 
        {
            _startingMap = startingMap;
        
        
            string[] parts = startingMap.Split('\n');
            cells = new Cell[parts.Length][];
            for (int i = 0; i < parts.Length; i++)
            {
                cells[i] = new Cell[parts[i].Length];
                for (int j = 0; j < parts[i].Length; j++)
                {
                    CreateCell(i, j, parts[i][j]);
                }
            }
        }

        private void CreateCell(int x, int y, char character)
        {
            Cell cell = cells[x][y] = new Cell(new Coordinates(y, x), character, this);
        }

        public Cell GetCell(Coordinates coordinates)
        {
            return !AreValidCoordinates(coordinates) ? null : cells[coordinates.X][coordinates.Y];
        }

        private bool AreValidCoordinates(Coordinates coordinates)
        {
            return coordinates.X >= 0 && coordinates.X < cells.Length && coordinates.Y >= 0 &&
                   coordinates.Y >= cells[coordinates.X].Length;
        }

        public bool TryGetCell(Coordinates coordinates, out Cell cell)
        {
            if (!AreValidCoordinates(coordinates))
            {
                cell = null;
                return false;
            }

            cell = cells[coordinates.X][coordinates.Y];
            return true;
        }

        [System.Serializable]
        public class Cell
        {
            public enum CellType
            {
                Floor,
                Wall,
                Box,
                Goal,
                Player,
                BoxOnGoal,
                PlayerOnGoal,
            }

            public Coordinates coordinates;
            public char character;
            [FormerlySerializedAs("grid")] public Map map;

            public Cell(Coordinates coordinates, char character, Map map)
            {
                this.character = character;
                this.map = map;
                this.coordinates = coordinates;
            }

            public Cell GetNeighbour(Direction direction) => map.GetCell(coordinates.Step(direction));

            public bool TryGetNeighbour(Direction direction, out Cell cell) =>
                map.TryGetCell(coordinates.Step(direction), out cell);

            public static implicit operator bool(Cell cell) => cell != null;
        }




    }

    public static class MapExtensions
    {
        private static Dictionary<char, Map.Cell.CellType> _lookup = new Dictionary<char, Map.Cell.CellType>()
        {
            { ' ', Map.Cell.CellType.Floor },
            { '#', Map.Cell.CellType.Wall },
            { '$', Map.Cell.CellType.Box },
            { '.', Map.Cell.CellType.Goal },
            { '@', Map.Cell.CellType.Player },
            { '*', Map.Cell.CellType.BoxOnGoal },
            { '+', Map.Cell.CellType.PlayerOnGoal },
        };

        public static Map.Cell.CellType CellType(this Map.Cell cell)
        {
            return _lookup[cell.character];
        }
    }
}
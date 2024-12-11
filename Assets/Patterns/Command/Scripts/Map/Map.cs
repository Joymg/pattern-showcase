using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using UnityEngine.Serialization;
using UnityEngine.TextCore.Text;

namespace Joymg.Patterns.Command
{
    [System.Serializable]
    public class Map
    {
        private Cell[][] cells;
        private readonly string _startingMap;
        public Cell[][] Cells => cells;
        public List<Cell> goals;
        public List<Entity> entities;
        public int Height => cells.Length;

        public Map(string startingMap)
        {
            _startingMap = startingMap;
            entities = new List<Entity>();
            goals = new List<Cell>();

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
            Cell cell = cells[x][y] = new Cell(new Coordinates(x, y), character, this);
            if (character is '.' or '+' or '*')
            {
                goals.Add(cell);
            }
        }

        public Cell GetCell(Coordinates coordinates)
        {
            return !AreValidCoordinates(coordinates) ? null : cells[coordinates.X][coordinates.Y];
        }

        private bool AreValidCoordinates(Coordinates coordinates)
        {
            return coordinates.X >= 0 && coordinates.X < cells.Length && coordinates.Y >= 0 &&
                   coordinates.Y < cells[coordinates.X].Length;
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

        public void Swap(Cell a, Cell b)
        {
            Cell.CellType aCelltype = a.CellType();
            Cell.CellType bCelltype = b.CellType();

            if (aCelltype == Cell.CellType.Player && bCelltype == Cell.CellType.Goal)
            {
                a.character = MapExtensions.Lookup.First(entry => entry.Value == Cell.CellType.Floor).Key;
                b.character = MapExtensions.Lookup.First(entry => entry.Value == Cell.CellType.PlayerOnGoal).Key;
            }
            else if (aCelltype == Cell.CellType.PlayerOnGoal && bCelltype == Cell.CellType.Floor)
            {
                a.character = MapExtensions.Lookup.First(entry => entry.Value == Cell.CellType.Goal).Key;
                b.character = MapExtensions.Lookup.First(entry => entry.Value == Cell.CellType.Player).Key;
            }
            else if (aCelltype == Cell.CellType.Box && bCelltype == Cell.CellType.Goal)
            {
                a.character = MapExtensions.Lookup.First(entry => entry.Value == Cell.CellType.Floor).Key;
                b.character = MapExtensions.Lookup.First(entry => entry.Value == Cell.CellType.BoxOnGoal).Key;
            }
            else if (aCelltype == Cell.CellType.BoxOnGoal && bCelltype == Cell.CellType.Floor)
            {
                a.character = MapExtensions.Lookup.First(entry => entry.Value == Cell.CellType.Goal).Key;
                b.character = MapExtensions.Lookup.First(entry => entry.Value == Cell.CellType.Box).Key;
            }
            else
            {
                (a.character, b.character) = (b.character, a.character);
            }

        }

        public bool TryMovingEntitiesInDirection(List<Entity> _controlledEntities, Direction direction, out List<Entity> movingEntities)
        {
            movingEntities = new List<Entity>();
            foreach (Entity entity in _controlledEntities)
            {
                Cell currentCell = GetCell(entity.Coordinates.Reverse());

                if (!TryMoveCell(movingEntities, currentCell, direction, 0))
                    return false;
            }
            return true;

        }

        private bool TryMoveCell(List<Entity> movements, Cell currentCell, Direction direction,
            int i)
        {
            if (!currentCell.TryGetNeighbour(direction, out Cell neighborCell))
                return false;
            if (i > 1) return false;

            switch (neighborCell.CellType())
            {
                case Cell.CellType.Floor:
                case Cell.CellType.Goal:
                    movements.Add(GetEntityAtCoordinates(currentCell.coordinates.Reverse()));
                    //Swap(currentCell, neighborCell);
                    return true;
                case Cell.CellType.Box:
                case Cell.CellType.BoxOnGoal:
                    if (!TryMoveCell(movements, neighborCell, direction, i + 1)) return false;

                    movements.Add(GetEntityAtCoordinates(currentCell.coordinates.Reverse()));
                    //Swap(currentCell, neighborCell);
                    return true;
                case Cell.CellType.Wall:
                case Cell.CellType.Player:
                case Cell.CellType.PlayerOnGoal:
                    return false;
                default:
                    return true;
            }
        }

        private Entity GetEntityAtCoordinates(Coordinates entityCoordinates)
        {
            return entities.First(entity => entity.Coordinates.Equals(entityCoordinates));
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < cells.Length; i++)
            {
                for (int j = 0; j < cells[i].Length; j++)
                {
                    sb.Append(cells[i][j].character);
                }
                sb.Append('\n');
            }

            return sb.ToString();
        }

        internal bool CheckWin()
        {
            foreach (Cell goal in goals)
            {
                if (GetCell(goal.coordinates).CellType() != Cell.CellType.BoxOnGoal)
                return false;

            }
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
            private Map _map;

            public Cell(Coordinates coordinates, char character, Map map)
            {
                this.character = character;
                this._map = map;
                this.coordinates = coordinates;
            }

            public Cell GetNeighbour(Direction direction) => _map.GetCell(coordinates.Step(direction));

            public bool TryGetNeighbour(Direction direction, out Cell cell) =>
                _map.TryGetCell(coordinates.Step(direction), out cell);

            public static implicit operator bool(Cell cell) => cell != null;
        }
    }

    public static class MapExtensions
    {
        public static Dictionary<char, Map.Cell.CellType> Lookup = new Dictionary<char, Map.Cell.CellType>()
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
            return Lookup[cell.character];
        }
    }
}
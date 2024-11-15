using System.Collections.Generic;
using UnityEngine;

namespace Joymg.Patterns.Command
{
    [System.Serializable]
    public struct Coordinates
    {
        [SerializeField] private int _x, _y;

        public Coordinates(int x, int y)
        {
            _x = x;
            _y = y;
        }

        public Coordinates(Vector2 position)
        {
            _x = (int)position.x;
            _y = (int)position.y;
        }

        public readonly int X => _x;
        public readonly int Y => _y;

        public Coordinates Step(Direction direction) => direction switch
        {
            Direction.Right => new Coordinates(_x + 1, _y),
            Direction.Down => new Coordinates(_x, _y - 1),
            Direction.Left => new Coordinates(_x - 1, _y),
            _ => new Coordinates(_x, _y + 1)
        };
    }

    public class CorrdinateDirectionComparer : IComparer<Coordinates>
    {
        private Direction direction;

        public CorrdinateDirectionComparer(Direction direction)
        {
            this.direction = direction;
        }


        public int Compare(Coordinates a, Coordinates b)
        {
            switch (direction)
            {
                case Direction.Right:
                    return a.X > b.X ? 1 : -1;
                case Direction.Down:
                    return a.Y < b.Y ? 1 : -1;
                case Direction.Left:
                    return a.X < b.X ? 1 : -1;
                default:
                    return a.Y > b.Y ? 1 : -1;

            }
        }
    }
}
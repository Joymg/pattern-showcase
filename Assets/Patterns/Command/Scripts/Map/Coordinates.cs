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

        public int X
        {
            readonly get => _x;
            set => _x = value;
        }

        public int Y
        {
            readonly get => _y;
            set => _y = value;
        }

        public Coordinates Step(Direction direction) => direction switch
        {
            Direction.Right => new Coordinates(_x , _y+ 1),
            Direction.Down => new Coordinates(_x- 1, _y ),
            Direction.Left => new Coordinates(_x , _y- 1),
            _ => new Coordinates(_x + 1, _y )
        };

        public override bool Equals(object obj)
        {
            if (obj is not Coordinates coordinates)
                return false;

            return coordinates.X == _x && coordinates.Y == _y;
        }
    }

    public static class CoordinatesExtensions
    {
        public static Coordinates Reverse(this Coordinates coordinates)
        {
            return new Coordinates(coordinates.Y, coordinates.X);
        }
    }

    public class CoordinateDirectionComparer : IComparer<Coordinates>
    {
        private readonly Direction _direction;

        public CoordinateDirectionComparer(Direction direction)
        {
            _direction = direction;
        }


        public int Compare(Coordinates a, Coordinates b)
        {
            switch (_direction)
            {
                case Direction.Right:
                    return a.X > b.X ? 1 : -1;
                case Direction.Down:
                    return a.Y > b.Y ? 1 : -1;
                case Direction.Left:
                    return a.X < b.X ? 1 : -1;
                default:
                    return a.Y < b.Y ? 1 : -1;
            }
        }
    }
}
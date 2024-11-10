using UnityEngine;

// Author : 
namespace Joymg.Patterns.Command
{
    public enum Direction
    {
        Up,
        Right,
        Down,
        Left
    }

    public static class DirectionExtensions
    {
        public static Direction Opposite(this Direction direction)
        {
            return (int)direction < 2 ? (direction + 2) : (direction - 2);
        }

        public static Vector3 Collapse(this Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    return Vector3.up;
                case Direction.Right:
                    return Vector3.right;
                case Direction.Down:
                    return Vector3.down;
                case Direction.Left:
                    return Vector3.left;
                default:
                    return Vector3.zero;
            }
        }
    }
}
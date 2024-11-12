using System;
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
}
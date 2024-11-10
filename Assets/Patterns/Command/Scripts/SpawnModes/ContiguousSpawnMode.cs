using System.Collections.Generic;
using System.Linq;
using Joymg.Patterns.Command;
using UnityEngine;

// Author : 
public class ContiguousSpawnMode : SpawnMode
{
    private class Cell
    {
        public readonly Vector2 position;
        public int level;

        public Cell(Vector2 position, int level)
        {
            this.position = position;
            this.level = level;
        }
    }

    #region Enums

    #endregion

    #region Consts

    #endregion

    #region Fields

    private List<Vector2> _checkedCells;
    private List<Cell> _pendingCells;
    private int _height;
    private int _width;
    private readonly float _zOffset = 1.5f;


    public ContiguousSpawnMode(string map, char[] ids) : base(map, ids)
    {
        _checkedCells = new List<Vector2>();
        _pendingCells = new List<Cell>();
    }

    #endregion

    #region Unity Methods

    #endregion

    #region Methods

    public override List<Vector3> GenerateOrder()
    {
        Vector3 position;
        List<Vector3> order = new List<Vector3>();

        _width = _map.IndexOf('\n') + 1;
        _height = (_map.Length / _width) + 1;

        for (int i = 0; i < _map.Length; i++)
        {
            position = new Vector3(i % _width, (i / _width));
            position.z -= 15;
            if (_checkedCells.Contains(position))
                continue;

            _pendingCells.Add(new Cell(position, 0));

            while (_pendingCells.Count > 0)
            {
                Cell cell = _pendingCells.ElementAt(0);
                CheckCell(order, cell);
                _pendingCells.RemoveAt(0);
            }
        }

        return order;
    }

    private void CheckCell(List<Vector3> order, Cell cell)
    {
        if (!IsValidPosition(cell.position))
            return;
        if (_checkedCells.Contains(cell.position))
            return;

        Vector3 pos = cell.position;
        for (var index = 0; index < _ids.Length; index++)
        {
            var id = _ids[index];
            int mapIndex = (int)pos.y * _width + (int)pos.x % _width;
            if (_map[mapIndex] != id) continue;

            _checkedCells.Add(pos);
            _pendingCells.Add(new Cell(pos + Vector3.left, cell.level + 1));
            _pendingCells.Add(new Cell(pos + Vector3.up, cell.level + 1));
            _pendingCells.Add(new Cell(pos + Vector3.right, cell.level + 1));
            _pendingCells.Add(new Cell(pos + Vector3.down, cell.level + 1));
            pos.z -= 15f + _zOffset * cell.level;
            order.Add(pos + (Vector3)Offset);
        }
    }

    private bool IsValidPosition(Vector2 position)
    {
        return position.x >= 0 && position.x < _width - 1  && position.y >= 0 && position.y < _height;
    }

    #endregion
}
using System.Collections.Generic;
using System.Linq;
using Joymg.Patterns.Command;
using UnityEngine;

// Author : 
public class ContiguousSpawnMode : SpawnMode
{
    private class SpawnDepth
    {
        public readonly Map.Cell cell;
        public int level;

        public SpawnDepth(Map.Cell cell, int level)
        {
            this.cell = cell;
            this.level = level;
        }
    }

    #region Enums

    #endregion

    #region Consts

    #endregion

    #region Fields

    private List<Map.Cell> _checkedCells;
    private List<SpawnDepth> _pendingCells;
    private int _height;
    private int _width;
    private readonly float _zOffset = 1.5f;


    public ContiguousSpawnMode(Map map, char[] ids) : base(map, ids)
    {
        _checkedCells = new List<Map.Cell>();
        _pendingCells = new List<SpawnDepth>();
    }

    #endregion

    #region Unity Methods

    #endregion

    #region Methods

    public override List<Vector3> GenerateOrder()
    {
        List<Vector3> order = new List<Vector3>();

        int width = _map.Cells.Length;
        for (int i = 0; i < width; i++)
        {
            int height = _map.Cells[i].Length;
            for (int j = 0; j < height; j++)
            {
                Map.Cell cell = _map.Cells[i][j];
                if (!_ids.ToList().Contains(cell.character))
                    continue;
                if (_checkedCells.Contains(cell))
                    continue;

                _pendingCells.Add(new SpawnDepth(cell, 0));

                while (_pendingCells.Count > 0)
                {
                    SpawnDepth spawnDepth = _pendingCells.ElementAt(0);
                    CheckCell(order, spawnDepth);
                    _pendingCells.RemoveAt(0);
                }
            }
        }

        return order;
    }

    private void CheckCell(List<Vector3> order, SpawnDepth spawnDepth)
    {
        if (_checkedCells.Contains(spawnDepth.cell))
            return;

        for (var index = 0; index < _ids.Length; index++)
        {
            var id = _ids[index];
            if (spawnDepth.cell.character != id) continue;

            _checkedCells.Add(spawnDepth.cell);
            List<char> ids = _ids.ToList();
            for (int i = 0; i < 4; i++)
            {
                Direction direction = (Direction)i;
                if (spawnDepth.cell.TryGetNeighbour(direction, out Map.Cell neighbourCell) && ids.Contains(neighbourCell.character))
                {
                    _pendingCells.Add(new SpawnDepth(neighbourCell, spawnDepth.level + 1));
                }
            }

            Vector3 pos = new Vector3(spawnDepth.cell.coordinates.Y, spawnDepth.cell.coordinates.X, -15);
            pos.z -= _zOffset * spawnDepth.level;
            order.Add(pos);
        }
    }
    #endregion
}
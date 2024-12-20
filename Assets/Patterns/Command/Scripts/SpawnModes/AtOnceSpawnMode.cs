using System.Collections.Generic;
using UnityEngine;

// Author : Joy
namespace Joymg.Patterns.Command
{
    public class AtOnceSpawnMode : SpawnMode
    {
        public AtOnceSpawnMode(Map map, char[] ids) : base(map, ids)
        {
        }

        #region Enums

        #endregion

        #region Consts

        #endregion

        #region Fields

        #endregion

        #region Unity Methods

        #endregion

        #region Methods

        public override List<Vector3> GenerateOrder()
        {
            order = new List<Vector3>();

            int width = _map.Cells.Length;
            for (int i = 0; i < width; i++)
            {
                int height = _map.Cells[i].Length;
                for (int j = 0; j < height; j++)
                {
                    foreach (var id in _ids)
                    {
                        Map.Cell cell = _map.Cells[i][j];
                        if (cell.character == id)
                        {
                            AddToOrder(cell.coordinates, -15);
                        }
                    }
                }
            }

            return order;
        }

        #endregion
    }
}
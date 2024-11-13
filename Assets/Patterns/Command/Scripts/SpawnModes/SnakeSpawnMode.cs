using System;
using Joymg.Patterns.Command;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

// Author : Joy

namespace Joymg.Patterns.Command
{
    public class SnakeSpawnMode : SpawnMode
    {
        private const float Z_OFFSET = 2f;

        public SnakeSpawnMode(Map map, char[] ids) : base(map, ids)
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
            int jumps = 0;
            float zValue = 0f;
            Vector3 position;
            List<Vector3> order = new List<Vector3>();
            
            int width = _map.Cells.Length;
            for (int i = 0; i < width; i++)
            {
                List<Map.Cell> row = _map.Cells[i].ToList();
                if (jumps % 2 == 1)
                    row.Reverse();
                for (int j = 0; j < row.Count; j++)
                {
                    Map.Cell cell = row[j];
                    
                    foreach (var id in _ids)
                    {
                        if (cell.character != id) continue;

                        position = new Vector3(cell.coordinates.Y, cell.coordinates.X, -15 + zValue);
                        order.Add(position + (Vector3)Offset);
                        zValue -= Z_OFFSET;
                    }
                }
                
                zValue -= 2;
                jumps++;
                
            }

            return order;
        }

        #endregion
    }
}
using System.Collections.Generic;
using UnityEngine;

namespace Joymg.Patterns.Command
{
    public class LinesSpawnMode : SpawnMode
    {
        public LinesSpawnMode(Map map, char[] ids) : base(map, ids)
        {
        }

        public override List<Vector3> GenerateOrder()
        {
            order = new List<Vector3>();
            float zOffset = 0f;

            int width = _map.Cells.Length;
            for (int i = 0; i < width; i++)
            {
                int height = _map.Cells[i].Length;
                for (int j = 0; j < height; j++)
                {
                    foreach (var id in _ids)
                    {
                        Map.Cell cell = _map.Cells[i][j];
                        if (cell.character != id) continue;



                        AddToOrder(cell.coordinates, -15 + zOffset);
                        zOffset -= 1;
                    }
                }
            }

            return order;
        }
    }
}
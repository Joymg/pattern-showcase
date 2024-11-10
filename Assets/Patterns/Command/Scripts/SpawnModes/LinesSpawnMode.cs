using System.Collections.Generic;
using UnityEngine;

namespace Joymg.Patterns.Command
{
    public class LinesSpawnMode : SpawnMode
    {
        public LinesSpawnMode(string map, char[] ids) : base(map, ids)
        {
        }

        public override List<Vector3> GenerateOrder()
        {
            List<Vector3> order = new List<Vector3>();
            float zOffset = 0f;
            Vector3 position;
            int width = _map.IndexOf('\n') + 1;
            for (int i = 0; i < _map.Length; i++)
            {
                foreach (var id in _ids)
                {
                    if (_map[i] != id) continue;

                    position = new Vector3((i % width), i / width, zOffset);
                    position.z -= 15;
                    order.Add(position + (Vector3)Offset);
                    zOffset -= 1;
                }
            }

            return order;
        }
    }
}
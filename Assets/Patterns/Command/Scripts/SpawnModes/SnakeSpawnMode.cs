using System;
using Joymg.Patterns.Command;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

// Author : Joy

namespace Joymg.Patterns.Command
{
    public class SnakeSpawnMode : SpawnMode
    {
        private const float Z_OFFSET = 1.5f;

        public SnakeSpawnMode(string map, char[] ids) : base(map, ids)
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
            int width = _map.IndexOf('\n') + 1;
            float zValue = 0f;
            Vector3 position;
            List<Vector3> order = new List<Vector3>();
            
            for (int i = 0; i < _map.Length;)
            {
                var currentCharacter = _map[i];
                if (currentCharacter == '\n')
                {
                    i += width;
                    jumps++;
                }
                
                foreach (var id in _ids)
                {
                    if (currentCharacter != id) continue;
                    
                    position = new Vector3((i % width), i / width, zValue);
                    position.z -= 15;
                    order.Add(position + (Vector3)Offset);
                    zValue -= Z_OFFSET;
                }

                i = (jumps % 2 == 0) ? i + 1 : i - 1;
            }

            return order;
        }

        #endregion
    }
}
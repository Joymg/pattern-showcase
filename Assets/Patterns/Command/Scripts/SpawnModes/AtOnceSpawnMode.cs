using System.Collections.Generic;
using UnityEngine;

// Author : Joy
namespace Joymg.Patterns.Command
{
    public class AtOnceSpawnMode : SpawnMode
    {
        public AtOnceSpawnMode(string map, char[] ids) : base(map, ids)
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
            List<Vector3> order = new List<Vector3>();
            int width = _map.IndexOf('\n') + 1;
            Vector3 position;
            for (int i = 0; i < _map.Length; i++)
            {
                foreach (var id in _ids)
                {
                    if (_map[i] == id)
                    {
                        position = new Vector3(i % width, (i / width));
                        position.z -= 15;
                        order.Add(position + (Vector3)Offset);
                    }
                }
            }
            return order;
        }

        #endregion
    }
}
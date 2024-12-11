using System;
using System.Collections.Generic;
using UnityEngine;
using static Joymg.Patterns.Command.Map;
using UnityEngine.UIElements;

// Author : Joy

namespace Joymg.Patterns.Command
{
    [Serializable]
    public abstract class SpawnMode
    {
        #region Enums
        #endregion

        #region Consts
        public readonly Vector2 Offset = Vector2.one * 0.5f;
        #endregion

        #region Fields
        protected Map _map;
        protected char[] _ids;
        protected List<Vector3> order;

        protected SpawnMode(Map map, char[] ids)
        {
            _map = map;
            _ids = ids;
        }

        #endregion

        #region Unity Methods
        #endregion

        #region Methods

        public virtual void AddToOrder(Coordinates coordinates, float zValue)
        {
            Vector3 position = new Vector3(coordinates.Y,  coordinates.X, zValue);
            order.Add(position);
        }
        public abstract List<Vector3> GenerateOrder();
        #endregion

    }
}

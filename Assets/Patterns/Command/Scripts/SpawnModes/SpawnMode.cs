using System;
using System.Collections.Generic;
using UnityEngine;

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
        protected string _map;
        protected char[] _ids;

        protected SpawnMode(string map, char[] ids)
        {
            _map = map;
            _ids = ids;
        }

        #endregion

        #region Unity Methods
        #endregion

        #region Methods
        public abstract List<Vector3> GenerateOrder();
        #endregion

    }
}

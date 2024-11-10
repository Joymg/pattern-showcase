using UnityEngine;

// Author : Joy
namespace Joymg.Patterns.Builder
{
    public interface Builder<T>
    {
        #region Enums
        #endregion

        #region Consts
        #endregion

        #region Fields
        #endregion

        #region Unity Methods
        #endregion

        #region Methods

        public void Reset();
        public Builder<T> SetCannon(GameObject cannon);
        public Builder<T> SetFundations(GameObject fundations);
        public Builder<T> SetWall(GameObject wall);

        public T GetResult();
        #endregion

    }
}

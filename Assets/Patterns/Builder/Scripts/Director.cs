using UnityEngine;

// Author : 
namespace Joymg.Patterns.Builder
{
    public class Director : MonoBehaviour
    {
        #region Enums
        #endregion

        #region Consts
        #endregion

        #region Fields
        GameObject placeholder;
        public Builder<MonoBehaviour> builder;
        #endregion

        #region Unity Methods
        #endregion

        #region Methods

        public void ConstructBasicTower()
        {
            builder.Reset();
            builder.SetCannon(placeholder).SetFundations(placeholder).SetWall(placeholder);
        }
        #endregion

    }
}

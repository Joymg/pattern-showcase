using Joymg.Patterns.Builder;
using UnityEngine;

// Author : 
namespace Joymg.Patterns.Builder
{
    public class TowerBuilder : Builder<Cannon>
    {
        #region Enums
        #endregion

        #region Consts
        #endregion

        #region Fields
        private Cannon _tower;
        #endregion

        #region Unity Methods
        #endregion

        #region Methods

        public Cannon GetResult()
        {
            throw new System.NotImplementedException();
        }

        public void Reset()
        {
            throw new System.NotImplementedException();
        }

        public Builder<Cannon> SetCannon(GameObject cannon)
        {
            throw new System.NotImplementedException();
        }

        public Builder<Cannon> SetFundations(GameObject fundations)
        {
            throw new System.NotImplementedException();
        }

        public Builder<Cannon> SetWall(GameObject wall)
        {
            throw new System.NotImplementedException();
        }
        #endregion
    }
}

using UnityEngine;

namespace Joymg.Patterns.Command
{

    public class Player : Entity
    {
        #region Enums
        #endregion

        #region Consts
        private const float STEP_DISTANCE = 1.0f;
        private const float STEP_SPEED = 1.0f;
        #endregion

        #region Fields
        #endregion

        #region Unity Methods
        #endregion

        #region Methods
        public void MoveTowards(Vector3 direction)
        {
            transform.Translate(direction * STEP_DISTANCE);
        }

        public void MoveUpwards()
        {
            MoveTowards(Vector3.up);
        }
        public void MoveRight()
        {
            MoveTowards(Vector3.right);
        }
        public void MoveDownwards()
        {
            MoveTowards(Vector3.down);
        }
        public void MoveLeftwards()
        {
            MoveTowards(Vector3.left);
        }
        #endregion
    }
}

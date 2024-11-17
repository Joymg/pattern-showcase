using Joymg.Utils;
using System.Collections;
using UnityEngine;

// Author : Joy

namespace Joymg.Patterns.Command
{

    public class Entity : MonoBehaviour
    {
        #region Enums
        #endregion

        #region Consts
        private const float STEP_DISTANCE = 1.0f;
        private const float STEP_SPEED = 1.0f;
        #endregion

        #region Fields
        private SpriteRenderer spriteRenderer;
        [SerializeField] protected Coordinates _coordinates;
        public Coordinates Coordinates => _coordinates;

        public SpriteRenderer SpriteRenderer  => spriteRenderer; 
        #endregion

        #region Unity Methods
        #endregion

        #region Methods

        public void Init(Coordinates coordinates)
        {
            _coordinates = coordinates;
        }
        public IEnumerator SetIntoPosition(float eta, EasingType easing = EasingType.Linear)
        {
            float t = 0;
            Vector3 start = transform.position;
            Vector3 end = transform.position;
            end.z = 0;
            while (t <= 1)
            {
                t += Time.deltaTime / eta;
                float easet = EasingHelper.ApplyEasing(easing, t);
                transform.position = Vector3.LerpUnclamped(start, end, easet);
                yield return null;
            }
            transform.position = end;
        }
        
        public void MoveTowards(Vector3 direction)
        {
            transform.Translate(direction * STEP_DISTANCE);
            _coordinates.X += (int)direction.x;
            _coordinates.Y += (int)direction.y;
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

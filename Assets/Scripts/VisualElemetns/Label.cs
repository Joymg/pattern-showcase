using Shapes;
using System;
using TMPro;
using UnityEngine;

// Author : Joy
namespace Joymg.VisualElements
{
    [Serializable]
    public class Label : VisualElement
    {
        #region Enums
        #endregion

        #region Consts
        #endregion

        #region Fields
        public Vector2 Position;
        public float FontSize;
        public string Text;
        private Vector2 offset =new Vector2 (0.2f, 0.4f);
        #endregion

        #region Unity Methods
        #endregion

        #region Methods

        public override void Init() { }

        public override void DrawElement()
        {
            Draw.ResetAllDrawStates();
            Draw.LineGeometry = LineGeometry.Flat2D;
            Draw.ThicknessSpace = ThicknessSpace.Pixels;
            Draw.Text(transform.position, Quaternion.identity, Text, TextAlign.Left, FontSize, CurrentColor);
        }
        public void DrawElement(Rect parentRect, Vector2 parentPosition)
        {
            Vector2 currentPosition = Position + parentPosition + Vector2.right * offset.x + Vector2.down * offset.y;

            Draw.ResetAllDrawStates();
            Draw.LineGeometry = LineGeometry.Flat2D;
            Draw.ThicknessSpace = ThicknessSpace.Pixels;

            Draw.Text(currentPosition, Quaternion.identity, Text,TextAlign.Left, FontSize, CurrentColor);
        }
        #endregion
    }
}
using Shapes;
using System.Collections.Generic;
using UnityEngine;

// Author : Joy

namespace Joymg.VisualElements
{
    public class Block : VisualElement
    {
        #region Enums
        #endregion

        #region Consts
        #endregion

        #region Fields
        public Label Title;
        public Vector2 Size;
        public Rectangle Parent;
        public Vector2 Position => Parent.transform.TransformPoint((Vector2)transform.localPosition);
        private Vector2 ScaledSize => Size * transform.localScale;
        public Vector2 RectTopLeftCorner => Position.AddToX(-(ScaledSize / 2).x).AddToY((ScaledSize / 2).y);
        public Vector2 RectBottomLeftCorner => Position.AddToX(-(ScaledSize / 2).x).AddToY(-(ScaledSize / 2).y);


        #endregion

        #region Unity Methods
        #endregion

        #region Methods

        public override void Init()
        {
            Children = new List<VisualElement>
            {
                Title
            };
        }
        public override void DrawElement()
        {
            Draw.ResetAllDrawStates(); // this makes sure no static draw states "leak" over to this scene

            Draw.LineGeometry = LineGeometry.Flat2D;
            Draw.ThicknessSpace = ThicknessSpace.Pixels;

            Rect rect = new Rect(RectBottomLeftCorner, Size);


            Draw.RectangleBorder(Position,
                                 transform.rotation,
                                 Size * transform.localScale,
                                 RectPivot.Center,
                                 6f,
                                 cornerRadius: 0.2f,
                                 CurrentColor);

            Draw.Push();
            Title.DrawElement(rect, RectTopLeftCorner);
            Draw.Pop();
        }

        #endregion
    }
}

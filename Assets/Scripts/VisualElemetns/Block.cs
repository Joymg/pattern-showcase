using Joymg.Utils;
using Shapes;
using System.Collections;
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
        public Rectangle Parent;
        public Color BorderColor;
        public Vector2 Position => Parent != null ? Parent.transform.TransformPoint((Vector2)transform.localPosition) : transform.position;
        private Vector2 ScaledSize => transform.localScale;
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

            Rect rect = new Rect(RectBottomLeftCorner, transform.position);

            Draw.Rectangle(Position, transform.rotation, transform.localScale, RectPivot.Center, 0.2f, CurrentColor);
            Draw.RectangleBorder(Position,
                                 transform.rotation,
                                 transform.localScale,
                                 RectPivot.Center,
                                 6f,
                                 cornerRadius: 0.2f,
                                 BorderColor);

            Draw.Push();
            Title.DrawElement(rect, RectTopLeftCorner);
            Draw.Pop();
        }

        public override IEnumerator FadeIn(float duration, EasingType easing = EasingType.Linear, bool showChildren = true)
        {
            float t = 0;
            Color bgStart = CurrentColor;
            Color bgEnd = CurrentColor;
            Color borderStart = BorderColor;
            Color borderEnd = BorderColor;
            borderEnd.a = 1;
            bgEnd.a = 1;
            while (t <= 1)
            {
                t += Time.deltaTime / duration;
                float easet = EasingHelper.ApplyEasing(easing, t);
                CurrentColor = Color.Lerp(bgStart, bgEnd, easet);
                BorderColor = Color.Lerp(borderStart, borderEnd, easet);
                if (!showChildren)
                {
                    yield return null;
                    continue;
                }

                foreach (var child in Children)
                {
                    child.CurrentColor = CurrentColor;
                }
                yield return null;
            }
            CurrentColor = bgEnd;
            BorderColor = borderEnd;
        }

        public override IEnumerator FadeOut(float duration, EasingType easing = EasingType.Linear, bool showChildren = true)
        {
            float t = 0;
            Color bgStart = CurrentColor;
            Color bgEnd = CurrentColor;
            Color borderStart = BorderColor;
            Color borderEnd = BorderColor;
            borderEnd.a = 0;
            bgEnd.a = 0;
            while (t <= 1)
            {
                t += Time.deltaTime / duration;
                float easet = EasingHelper.ApplyEasing(easing, t);
                CurrentColor = Color.Lerp(bgStart, bgEnd, easet);
                BorderColor = Color.Lerp(borderStart, borderEnd, easet);

                if (!showChildren)
                {
                    yield return null;
                    continue;
                }

                foreach (var child in Children)
                {
                    child.CurrentColor = CurrentColor;
                }
                yield return null;
            }
            CurrentColor = bgEnd;
            BorderColor = borderEnd;
        }


        #endregion
    }
}

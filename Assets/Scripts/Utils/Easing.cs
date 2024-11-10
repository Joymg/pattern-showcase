using System;
using UnityEngine;

// Author : Joy
namespace Joymg.Utils
{
    public enum EasingType
    {
        Linear,
        EaseInQuad,
        EaseOutQuad,
        EaseInOutQuad,
        EaseInCubic,
        EaseOutCubic,
        EaseInOutCubic,
        EaseInCirc,
        EaseOutCirc,
        EaseInOutCirc,
        EaseInExp,
        EaseOutExp,
        EaseInOutExp,
        EaseInBack,
        EaseOutBack,
        EaseInOutBack,
        Bounce,
        Elastic
    }

    public static class Easing {
        #region Enums
        #endregion

        #region Consts
        #endregion

        #region Fields
        #endregion

        #region Unity Methods
        #endregion

        #region Methods
        public static float Linear(float t)
        {
            return t;
        }

        public static float EaseInQuad(float t)
        {
            return t * t;
        }

        public static float EaseOutQuad(float t)
        {
            return t * (2 - t);
        }

        public static float EaseInOutQuad(float t)
        {
            return t < 0.5f ? 2 * t * t : -1 + (4 - 2 * t) * t;
        }

        public static float EaseInCubic(float t)
        {
            return t * t * t;
        }

        public static float EaseOutCubic(float t)
        {
            return 1 - MathF.Pow(1 - t, 3);
        }

        public static float EaseInOutCubic(float t)
        {
            return t < 0.5f ? 4 * t * t * t : 1 - MathF.Pow(-2 * t + 2, 3) / 2;
        }

        // Circular In (starts slow)
        public static float EaseInCirc(float t)
        {
            return 1 - (float)Math.Sqrt(Mathf.Clamp01(1 - t * t));
        }

        // Circular Out (starts fast)
        public static float EaseOutCirc(float t)
        {
            return (float)Math.Sqrt(1 - (t -= 1) * t);
        }

        // Circular InOut (starts slow, accelerates in the middle, slows down at the end)
        public static float EaseInOutCirc(float t)
        {
            if (t < 0.5f)
                return 0.5f * (1 - (float)Math.Sqrt(1 - 4 * t * t));
            return 0.5f * ((float)Math.Sqrt(1 - (t = t * 2 - 2) * t) + 1);
        }

        // Exponential easing (starts fast and decelerates)
        public static float EaseInExp(float t)
        {
            return (t == 0) ? 0 : (float)Math.Pow(2, 10 * (t - 1));
        }

        // Exponential Out Ease(starts fast)
        public static float EaseOutExp(float t)
        {
            return (t == 1) ? 1 : 1 - (float)Math.Pow(2, -10 * t);
        }

        // Exponential InOut (starts slow, accelerates in the middle, slows down at the end)
        public static float EaseInOutExp(float t)
        {
            if (t == 0 || t == 1)
                return t;
            if (t < 0.5f)
                return 0.5f * (float)Math.Pow(2, 20 * t - 10);
            return 0.5f * (2 - (float)Math.Pow(2, -20 * t + 10));
        }

        // Back ease-in function
        public static float EaseInBack(float t, float s = 1.70158f)
        {
            float c1 = 1.70158f;
            float c3 = c1 + 1;

            return (float)(c3 * t * t * t - c1 * t * t);
        }

        // Back ease-out function
        public static float EaseOutBack(float t, float s = 1.70158f)
        {
            float c1 = 1.70158f;
            float c3 = c1 + 1;

            return (float)(1 + c3 * Math.Pow(t - 1, 3) + c1 * Math.Pow(t - 1, 2));
        }

        // Back ease-in-out function
        public static float EaseInOutBack(float t, float s = 1.70158f)
        {
            float c1 = 1.70158f;
            float c2 = c1 * 1.525f;

            return t < 0.5
              ? (float)((Math.Pow(2 * t, 2) * ((c2 + 1) * 2 * t - c2)) / 2)
              : (float)((Math.Pow(2 * t - 2, 2) * ((c2 + 1) * (t * 2 - 2) + c2) + 2) / 2);
        }

        public static float Bounce(float t)
        {
            if (t < 1 / 2.75f)
                return 7.5625f * t * t;
            else if (t < 2 / 2.75f)
                return 7.5625f * (t -= 1.5f / 2.75f) * t + 0.75f;
            else if (t < 2.5 / 2.75f)
                return 7.5625f * (t -= 2.25f / 2.75f) * t + 0.9375f;
            else
                return 7.5625f * (t -= 2.625f / 2.75f) * t + 0.984375f;
        }

        // Elastic easing (simulates a spring-like effect)
        public static float Elastic(float t)
        {
            if (t == 0 || t == 1)
                return t;
            float p = 0.3f;
            float s = p / 4;
            return (float)Math.Pow(2, -10 * t) * (float)Math.Sin((t - s) * (2 * Math.PI) / p) + 1;
        }
        #endregion
    }

}

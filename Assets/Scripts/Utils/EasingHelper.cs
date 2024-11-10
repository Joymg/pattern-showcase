using System;
using UnityEngine;
using UnityEngine.UIElements;

// Author : Joy

namespace Joymg.Utils
{
    public class EasingHelper
    {
        public static float ApplyEasing(EasingType easingType, float t)
        {
            switch (easingType)
            {
                case EasingType.Linear:
                    return Easing.Linear(t);
                case EasingType.EaseInQuad:
                    return Easing.EaseInQuad(t);
                case EasingType.EaseOutQuad:
                    return Easing.EaseOutQuad(t);
                case EasingType.EaseInOutQuad:
                    return Easing.EaseInOutQuad(t);
                case EasingType.EaseInCubic:
                    return Easing.EaseInCubic(t);
                case EasingType.EaseOutCubic:
                    return Easing.EaseOutCubic(t);
                case EasingType.EaseInOutCubic:
                    return Easing.EaseInOutCubic(t);
                case EasingType.EaseInCirc:
                    return Easing.EaseInCirc(t);
                case EasingType.EaseOutCirc:
                    return Easing.EaseOutCirc(t);
                case EasingType.EaseInOutCirc:
                    return Easing.EaseInOutCirc(t);
                case EasingType.EaseInExp:
                    return Easing.EaseInExp(t);
                case EasingType.EaseOutExp:
                    return Easing.EaseOutExp(t);
                case EasingType.EaseInOutExp:
                    return Easing.EaseInOutExp(t);
                case EasingType.EaseInBack:
                    return Easing.EaseInBack(t);
                case EasingType.EaseOutBack:
                    return Easing.EaseOutBack(t);
                case EasingType.EaseInOutBack:
                    return Easing.EaseInOutBack(t);
                case EasingType.Bounce:
                    return Easing.Bounce(t);
                case EasingType.Elastic:
                    return Easing.Elastic(t);
                default:
                    return Easing.Linear(t);
            }
        }
    }
}

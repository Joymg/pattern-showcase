using Joymg.Utils;
using log4net.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

// Author : 
public abstract class VisualElement : MonoBehaviour
{
    #region Enums
    #endregion

    #region Consts
    #endregion

    #region Fields
    public Color CurrentColor;
    public List<VisualElement> Children;
    #endregion

    #region Unity Methods
    #endregion

    #region Methods
    public abstract void Init();
    public abstract void DrawElement();

    public void Show(bool showChildren = true)
    {
        CurrentColor.a = 1;
        if (!showChildren)
            return;
        foreach (var child in Children)
        {
            child.CurrentColor.a = 1;
        }
    }

    public void Hide(bool showChildren = true)
    {
        CurrentColor.a = 0;
        if (!showChildren)
            return;
        foreach (var child in Children)
        {
            child.CurrentColor.a = 0;
        }
    }

    public virtual IEnumerator FadeIn(float duration, EasingType easing = EasingType.Linear, bool showChildren = true)
    {
        float t = 0;
        Color start = CurrentColor;
        Color end = CurrentColor;
        end.a = 1;
        while (t <= 1)
        {
            t += Time.deltaTime / duration;
            float easet = EasingHelper.ApplyEasing(easing, t);
            CurrentColor = Color.Lerp(start, end, easet);
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
        CurrentColor = end;
    }

    public virtual IEnumerator FadeOut(float duration, EasingType easing = EasingType.Linear, bool showChildren = true)
    {
        float t = 0;
        Color start = CurrentColor;
        Color end = CurrentColor;
        end.a = 0;
        while (t <= 1)
        {
            t += Time.deltaTime / duration;
            float easet = EasingHelper.ApplyEasing(easing, t);
            CurrentColor = Color.Lerp(start, end, easet);
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
        CurrentColor = end;
    }


    public IEnumerator MoveTo(Vector2 position, float duration, EasingType easing = EasingType.Linear)
    {
        float t = 0;
        Vector2 start = transform.position;
        Vector2 end = position;
        while (t <= 1)
        {
            t += Time.deltaTime / duration;
            float easet = EasingHelper.ApplyEasing(easing, t);
            transform.position = Vector2.LerpUnclamped(start, end, easet);
            yield return null;
        }
        transform.position = end;
    }
    public IEnumerator MoveCircularTo(Vector2 position, float duration, EasingType easing = EasingType.Linear)
    {
        float t = 0;
        Vector2 start = transform.position;
        Vector2 end = position;
        Vector2 center = (start - end) * 0.5f;
        center += -Vector2.one * 1;
        start -= center;
        end -= center;
        while (t <= 1)
        {
            t += Time.deltaTime / duration;
            float easet = EasingHelper.ApplyEasing(easing, t);
            transform.position = Vector3.LerpUnclamped(start, end, easet);
            transform.position += (Vector3)center;
            yield return null;
        }
        transform.position = end;
    }

    public IEnumerator Rotate(Quaternion rotation, float duration, EasingType easing = EasingType.Linear)
    {
        float t = 0;
        Quaternion start = transform.rotation;
        Quaternion end = rotation;
        while (t <= 1)
        {
            t += Time.deltaTime / duration;
            float easet = EasingHelper.ApplyEasing(easing, t);
            transform.rotation = Quaternion.LerpUnclamped(start, end, easet);
            yield return null;
        }
        transform.rotation = end;
    }

    public IEnumerator Scale(Vector3 initialScale, Vector3 endScale, float duration, EasingType easing = EasingType.Linear)
    {
        float t = 0;
        while (t <= 1)
        {
            t += Time.deltaTime / duration;
            float easet = EasingHelper.ApplyEasing(easing, t);
            transform.localScale = Vector3.LerpUnclamped(initialScale, endScale, easet);
            yield return null;
        }
        transform.localScale = endScale;
    }

    public IEnumerator Bounce(Vector3 originalSize, float scaleFactor, float duration, EasingType easing = EasingType.Linear)
    {
        float t = 0;
        Vector3 start = originalSize;
        Vector3 end = start == Vector3.zero ? start + Vector3.one * scaleFactor : start * scaleFactor;
        while (t <= 1)
        {
            t += Time.deltaTime / duration;
            float easet = EasingHelper.ApplyEasing(easing, t);

            transform.localScale = t < 0.5f ?
                Vector3.Lerp(start, end, easet * 2) :
                Vector3.Lerp(end, start, easet * 2);
            yield return null;
        }
        transform.localScale = start;
    }

    public VisualElement Duplicate()
    {
        VisualElement visualElement = Instantiate(this);
        visualElement.transform.SetParent(transform.parent);
        return visualElement;
    }

    #endregion

}

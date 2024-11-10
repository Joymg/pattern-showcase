using UnityEngine;

// Author : Joy
public static class Vector2Extensions
{
    #region Methods

    public static Vector2 AddToX(this Vector2 vector, float value)
    {
        return vector += Vector2.right * value;
    }

    public static Vector2 AddToY(this Vector2 vector, float value)
    {
        return vector += Vector2.up * value;
    }
    #endregion

}

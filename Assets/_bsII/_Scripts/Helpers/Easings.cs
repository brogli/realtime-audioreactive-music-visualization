using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Helper class providing common easings. Source: http://gizma.com/easing , https://spicyyoghurt.com/tools/easing-functions .
/// Assume t = value, b = 0, c = 1, d = 1
/// </summary>
public static class Easings
{
    #region sinusoidal
    public static float EaseInSine(float x)
    {
        return 1 - Mathf.Cos(x * (Mathf.PI / 2)) ;
    }
    #endregion

    #region quadratic
    public static float EaseInQuad(float x)
    {
        return x * x;
    }
    #endregion

    #region cubic
    public static float EaseInCubic(float x)
    {
        return x * x * x;
    }
    public static float EaseInOutCubic(float x)
    {
        x /= 0.5f;
        if (x < 1) 
            return 0.5f * x * x * x;
        x -= 2;
        return 0.5f * (x * x * x + 2);
    }

    #endregion

    #region exponential
    public static float EaseInExpo(float x)
    {
        return x == 0 ? 0 : Mathf.Pow(2, 10 * (x - 1));
    }

    public static float EaseOutExpo(float x)
    {
        return x == 1 ? 1 : (-Mathf.Pow(2, -10 * x) + 1);
    }

    #endregion

}

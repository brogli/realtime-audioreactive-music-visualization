using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Helper class providing common easings.
/// </summary>
public static class Easings
{
    public static float EaseInCubic(float x)
    {
        return x * x * x;
    }

    public static float EaseInOutCubic(float x)
    {
        return x < 0.5f ? 4 * x * x * x : 1 - Mathf.Pow(-2 * x + 2, 3) / 2;
    }

    public static float EaseInExpo(float x)
    {
        return x == 0 ? 0 : Mathf.Pow(2, 10 * x - 10);
    }

    public static float EaseOutExpo(float x)
    {
        return x == 1 ? 1 : 1 - Mathf.Pow(2, -10 * x);
    }

}

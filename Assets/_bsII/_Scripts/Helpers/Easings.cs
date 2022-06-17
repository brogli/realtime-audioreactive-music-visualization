using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Helper class providing common easings. Source: https://easings.net .
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

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BsIImath
{
    /// <summary>
    /// According to some SO thread https://stackoverflow.com/a/51018529 C#'s modulo is not modulo but remainder, thus gives negative values.
    /// </summary>
    /// <returns></returns>
    public static float AcutalModulo(float x, float m)
    {
        float r = x % m;
        return r < 0 ? r + m : r;
    }
}

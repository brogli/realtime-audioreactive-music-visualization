using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageSequence
{
    Texture2D[] _sequence;

    public ImageSequence(Texture2D[] sequence)
    {
        _sequence = sequence;
    }

    public Texture2D GetImageAtIndex(int index)
    {
        return _sequence[index];
    }

    /// <summary>
    /// Returns the image at the passed index (which is normalized between 0 and 1.
    /// </summary>
    /// <param name="normalizedIndex"></param>
    /// <returns></returns>
    public Texture2D GetImageAtNormalizedIndex(float normalizedIndex)
    {
        var actualIndex = Mathf.RoundToInt(normalizedIndex * (_sequence.Length -1 ));
        return _sequence[actualIndex];
    }
}

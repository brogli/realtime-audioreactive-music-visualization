using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggeledUserInputs<T> where T : new()
{
    public T[] keys;

    public ToggeledUserInputs()
    {
        keys = new T[8];
        for (int i = 0; i<keys.Length; i++)
        {
            keys[i] = new T();
        }
    }
}

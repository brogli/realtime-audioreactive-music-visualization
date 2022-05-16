﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggeledUserInputs<T> where T : IUserInput, new()
{
    public T[] Keys { get; private set; }

    public ToggeledUserInputs()
    {
        Keys = new T[8];
        for (int i = 0; i < Keys.Length; i++)
        {
            Keys[i] = new T();
        }
    }
}

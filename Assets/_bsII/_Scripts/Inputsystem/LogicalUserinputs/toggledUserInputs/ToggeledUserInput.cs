﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ToggeledUserInput : IUserInput
{
    public delegate void KeyUpEvent();
    public event KeyUpEvent KeyUpEventEmitted;

    public delegate void KeyDownEvent();
    public event KeyDownEvent KeyDownEventEmitted;

    public bool IsPressed { get; private set; }

    public void SetNewStateIfNecessary(bool newIsPressed)
    {
        if (newIsPressed == IsPressed)
        {
            return;
        }

        if (newIsPressed)
        {
            IsPressed = true;
            KeyDownEventEmitted?.Invoke();
        } else
        {
            IsPressed = false;
            KeyUpEventEmitted?.Invoke();
        }
    }
}



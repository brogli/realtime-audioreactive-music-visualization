using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggeledUserInput : IUserInput
{
    public delegate void TurnedOnEvent();
    public event TurnedOnEvent EmitTurnedOnEvent;

    public delegate void TurnedOffEvent();
    public event TurnedOffEvent EmitTurnedOffEvent;

    public bool IsPressed { get; private set; }

    public float Value { get; private set; }

    public void SetNewStateIfNecessary(bool newIsPressed, float _)
    {
        if (newIsPressed == IsPressed)
        {
            return;
        }

        if (newIsPressed)
        {
            IsPressed = true;
            EmitTurnedOnEvent?.Invoke();
        } else
        {
            IsPressed = false;
            EmitTurnedOffEvent?.Invoke();
        }
    }
}



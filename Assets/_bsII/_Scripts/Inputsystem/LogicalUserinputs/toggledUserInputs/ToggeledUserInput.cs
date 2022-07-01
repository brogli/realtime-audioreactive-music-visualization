using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggeledUserInput : IUserInput
{
    public delegate void TurnedOnOrOffEvent(bool hasTurnedOn, int index);
    public event TurnedOnOrOffEvent EmitTurnedOnOrOffEvent;

    public bool IsPressed { get; private set; } = false;

    public float Value { get; private set; }

    private int _index;
    public int Index
    {
        get => _index;
        private set
        {
            _index = value;
            HasIndexSet = true;
        }
    }
    public bool HasIndexSet { get; private set; }

    public ToggeledUserInput()
    {

    }

    public ToggeledUserInput(int index)
    {
        _index = index;
    }

    public void SetNewStateIfNecessary(bool newIsPressed, float _)
    {
        if (newIsPressed == IsPressed)
        {
            return;
        }

        if (newIsPressed)
        {
            IsPressed = true;
            EmitTurnedOnOrOffEvent?.Invoke(true, Index);
        }
        else
        {
            IsPressed = false;
            EmitTurnedOnOrOffEvent?.Invoke(false, Index);
        }
    }
}



using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggeledUserInput : ISceneUserInput
{
    public delegate void TurnedOnOrOffEvent(bool hasTurnedOn, int index);
    public event TurnedOnOrOffEvent EmitTurnedOnOrOffEvent;
    private bool _hasAccessors;
    private int _index;
    private bool isPressed = false;
    private float _value;

    public bool IsPressed
    {
        get
        {
            _hasAccessors = true;
            return isPressed;
        }
        private set => isPressed = value;
    }
    public float Value
    {
        get
        {
            _hasAccessors = true;
            return _value;
        }
        private set => _value = value;
    }


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

    public bool IsUsed()
    {
        return EmitTurnedOnOrOffEvent != null || _hasAccessors;
    }

    public void ResetValidationFlags()
    {
        _hasAccessors = false;
    }
}



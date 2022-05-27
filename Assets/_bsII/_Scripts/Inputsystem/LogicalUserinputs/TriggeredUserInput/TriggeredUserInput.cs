using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggeredUserInput : IUserInput
{
    public int index = 0;
    public delegate void KeyTriggerEvent(int index);
    public event KeyTriggerEvent EmitTriggerEvent;

    public TriggeredUserInput()
    {

    }

    public TriggeredUserInput(int index)
    {
        this.index = index;
    }

    public void SetNewStateIfNecessary(bool newIsPressed, float value)
    {
        EmitTriggerEvent?.Invoke(index);
    }
}

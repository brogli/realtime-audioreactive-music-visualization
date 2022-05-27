using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggeredUserInput : IUserInput
{
    public delegate void KeyTriggerEvent();
    public event KeyTriggerEvent EmitTriggerEvent;

    public void SetNewStateIfNecessary(bool newIsPressed, float value)
    {
        EmitTriggerEvent?.Invoke();
    }
}

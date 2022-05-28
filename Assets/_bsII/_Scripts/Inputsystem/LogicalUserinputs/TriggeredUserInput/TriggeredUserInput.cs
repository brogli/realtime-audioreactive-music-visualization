using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggeredUserInput : IUserInput
{
    public int index = 0;
    public delegate void KeyTriggerEvent();
    public delegate void CollectionKeyTriggerEvent(int index);
    public event KeyTriggerEvent EmitKeyTriggeredEvent;
    public event CollectionKeyTriggerEvent EmitCollectionKeyTriggeredEvent;

    public TriggeredUserInput()
    {

    }

    public TriggeredUserInput(int index)
    {
        this.index = index;
    }

    public void SetNewStateIfNecessary(bool newIsPressed, float value)
    {
        EmitKeyTriggeredEvent?.Invoke();
        EmitCollectionKeyTriggeredEvent?.Invoke(index);
    }
}

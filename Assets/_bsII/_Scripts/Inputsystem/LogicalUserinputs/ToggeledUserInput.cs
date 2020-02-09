using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ToggeledUserInput
{
    public delegate void KeyUpEvent();
    public event KeyUpEvent KeyUpEventEmitted;

    public delegate void KeyDownEvent();
    public event KeyDownEvent EmitKeyDownEventEmitted;

    public bool IsPressed { get; private set; }

}

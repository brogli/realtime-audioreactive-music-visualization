using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UserInput : IUserInput
{
    public abstract void SetNewStateIfNecessary(bool newIsPressed, float value);
}

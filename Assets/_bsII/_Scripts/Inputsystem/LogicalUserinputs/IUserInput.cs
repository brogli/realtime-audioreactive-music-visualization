using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUserInput
{
    void SetNewStateIfNecessary(bool newIsPressed, float value);
}

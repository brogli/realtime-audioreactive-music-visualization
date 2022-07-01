using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUserInputsConsumer
{
    void SubscribeUserInputs();
    void UnsubscribeUserInputs();
}

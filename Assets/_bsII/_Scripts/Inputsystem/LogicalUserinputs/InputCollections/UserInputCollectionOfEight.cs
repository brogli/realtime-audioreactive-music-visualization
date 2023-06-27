using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInputCollectionOfEight<T> where T : ISceneUserInput
{
    public T[] Keys { get; private set; }

    public UserInputCollectionOfEight(T[] keys)
    {
        Keys = keys;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInputCollectionOfEight<T> where T : IUserInput
{
    public T[] Keys { get; private set; }

    public UserInputCollectionOfEight()
    {
        Keys = new T[8];
        for (int i = 0; i < Keys.Length; i++)
        {
            //Keys[i] = new T();
        }
    }
}

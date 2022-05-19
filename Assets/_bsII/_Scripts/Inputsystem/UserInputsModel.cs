using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInputsModel : MonoBehaviour
{
    public UserInputCollectionOfEight<MelodyKey> MelodyKeys { get; private set; }

    public void Start()
    {
        MelodyKeys = new UserInputCollectionOfEight<MelodyKey>();
    }
}

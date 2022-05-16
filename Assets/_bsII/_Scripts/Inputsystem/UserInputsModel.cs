using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInputsModel : MonoBehaviour
{
    public ToggeledUserInputs<MelodyKey> MelodyKeys { get; private set; }

    public void Start()
    {
        MelodyKeys = new ToggeledUserInputs<MelodyKey>();
    }
}

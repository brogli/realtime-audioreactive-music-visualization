using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInputsProvider 
{
    public ToggeledUserInputs<MelodyKey> melodyKeys;

    public UserInputsProvider()
    {
        melodyKeys = new ToggeledUserInputs<MelodyKey>();
    }

}

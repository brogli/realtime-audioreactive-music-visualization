using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInputsModel : MonoBehaviour
{
    #region management related
    public TriggeredUserInput ReloadGameSettings { get; private set; }
    public TriggeredUserInput ResetGameSettingsToDefaults { get; private set; }
    #endregion

    #region audio related
    public UserInputCollectionOfEight<ToggeledUserInput> MelodyKeys { get; private set; }
    #endregion
    public void Awake()
    {
        var keys = new ToggeledUserInput[8];
        for (int i = 0; i < keys.Length; i++)
        {
            keys[i] = new ToggeledUserInput();
        }
        MelodyKeys = new UserInputCollectionOfEight<ToggeledUserInput>(keys);
        ReloadGameSettings = new TriggeredUserInput();
        ResetGameSettingsToDefaults = new TriggeredUserInput();
    }
}

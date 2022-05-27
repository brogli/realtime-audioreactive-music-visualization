using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class KeyboardInputProcessor : MonoBehaviour
{
    private UserInputsModel _userInputsModel;

    // Start is called before the first frame update
    void Start()
    {
        _userInputsModel = GameObject.FindGameObjectWithTag("UserInputsModel").GetComponent<UserInputsModel>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ProcessReloadGameSettings(CallbackContext context)
    {
        if (context.performed)
        {
            _userInputsModel.ReloadGameSettings.SetNewStateIfNecessary(true, 0);
        }
    }

    public void ProcessResetGameSettingsToDefault(CallbackContext context)
    {
        if (context.performed)
        {
            _userInputsModel.ResetGameSettingsToDefaults.SetNewStateIfNecessary(true, 0);
        }
    }
}

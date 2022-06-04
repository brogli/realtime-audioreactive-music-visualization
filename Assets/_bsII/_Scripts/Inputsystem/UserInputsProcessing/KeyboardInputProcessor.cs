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

    #region beat related
    public void ProcessFourInFour(CallbackContext context)
    {
        if (context.performed)
        {
            _userInputsModel.FourInFourUserInput.ToggeledUserInput.SetNewStateIfNecessary(!_userInputsModel.FourInFourUserInput.ToggeledUserInput.IsPressed, 0);
        }
    }

    public void ProcessOneInFour(CallbackContext context)
    {
        if (context.performed)
        {
            _userInputsModel.OneInFourUserInput.ToggeledUserInput.SetNewStateIfNecessary(!_userInputsModel.OneInFourUserInput.ToggeledUserInput.IsPressed, 0);
        }
    }

    public void ProcessTwoInFour(CallbackContext context)
    {
        if (context.performed)
        {
            _userInputsModel.TwoInFourUserInput.ToggeledUserInput.SetNewStateIfNecessary(!_userInputsModel.TwoInFourUserInput.ToggeledUserInput.IsPressed, 0);
        }
    }

    public void ProcessEightInFour(CallbackContext context)
    {
        if (context.performed)
        {
            _userInputsModel.EightInFourUserInput.ToggeledUserInput.SetNewStateIfNecessary(!_userInputsModel.EightInFourUserInput.ToggeledUserInput.IsPressed, 0);
        }
    }

    public void ProcessSixteenInFour(CallbackContext context)
    {
        if (context.performed)
        {
            _userInputsModel.SixteenInFourUserInput.ToggeledUserInput.SetNewStateIfNecessary(!_userInputsModel.SixteenInFourUserInput.ToggeledUserInput.IsPressed, 0);
        }
    }

    public void ProcessOneInEight(CallbackContext context)
    {
        if (context.performed)
        {
            _userInputsModel.OneInEightUserInput.ToggeledUserInput.SetNewStateIfNecessary(!_userInputsModel.OneInEightUserInput.ToggeledUserInput.IsPressed, 0);
        }
    }
#endregion
}

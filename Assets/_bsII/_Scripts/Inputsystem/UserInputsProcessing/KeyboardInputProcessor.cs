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

    #region management
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
    #endregion

    #region volume related
    public void ProcessAverageVolumeInput(CallbackContext context)
    {
        if (context.performed)
        {
            _userInputsModel.AverageVolume.ToggeledUserInput.SetNewStateIfNecessary(!_userInputsModel.AverageVolume.ToggeledUserInput.IsPressed, 0);
        }
    }

    public void ProcessLowFrequencyVolumeInput(CallbackContext context)
    {
        if (context.performed)
        {
            _userInputsModel.LowFrequencyVolume.ToggeledUserInput.SetNewStateIfNecessary(!_userInputsModel.LowFrequencyVolume.ToggeledUserInput.IsPressed, 0);
        }
    }
    #endregion

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

    #region melodykeys
    public void ProcessMelodyKeys(CallbackContext context, int index)
    {
        if (context.started)
        {
            _userInputsModel.MelodyKeys.Keys[index].SetNewStateIfNecessary(true, 0);
        }
        if (context.canceled)
        {
            _userInputsModel.MelodyKeys.Keys[index].SetNewStateIfNecessary(false, 0);
        }
    }
    public void ProcessMelodyKey0(CallbackContext context)
    {
        ProcessMelodyKeys(context, 0);
    }

    public void ProcessMelodyKey1(CallbackContext context)
    {
        ProcessMelodyKeys(context, 1);
    }

    public void ProcessMelodyKey2(CallbackContext context)
    {
        ProcessMelodyKeys(context, 2);
    }

    public void ProcessMelodyKey3(CallbackContext context)
    {
        ProcessMelodyKeys(context, 3);
    }

    public void ProcessMelodyKey4(CallbackContext context)
    {
        ProcessMelodyKeys(context, 4);
    }

    public void ProcessMelodyKey5(CallbackContext context)
    {
        ProcessMelodyKeys(context, 5);
    }

    public void ProcessMelodyKey6(CallbackContext context)
    {
        ProcessMelodyKeys(context, 6);
    }

    public void ProcessMelodyKey7(CallbackContext context)
    {
        ProcessMelodyKeys(context, 7);
    }
    #endregion


    #region dronekeys
    public void ProcessDroneKeys(CallbackContext context, int index)
    {
        if (context.started)
        {
            _userInputsModel.DroneKeys.Keys[index].SetNewStateIfNecessary(true, 0);
        }
        if (context.canceled)
        {
            _userInputsModel.DroneKeys.Keys[index].SetNewStateIfNecessary(false, 0);
        }
    }
    public void ProcessDroneKey0(CallbackContext context)
    {
        ProcessDroneKeys(context, 0);
    }

    public void ProcessDroneKey1(CallbackContext context)
    {
        ProcessDroneKeys(context, 1);
    }

    public void ProcessDroneKey2(CallbackContext context)
    {
        ProcessDroneKeys(context, 2);
    }

    public void ProcessDroneKey3(CallbackContext context)
    {
        ProcessDroneKeys(context, 3);
    }

    public void ProcessDroneKey4(CallbackContext context)
    {
        ProcessDroneKeys(context, 4);
    }

    public void ProcessDroneKey5(CallbackContext context)
    {
        ProcessDroneKeys(context, 5);
    }

    public void ProcessDroneKey6(CallbackContext context)
    {
        ProcessDroneKeys(context, 6);
    }

    public void ProcessDroneKey7(CallbackContext context)
    {
        ProcessDroneKeys(context, 7);
    }
    #endregion


    #region MoodKeys
    public void ProcessMoodKeys(CallbackContext context, int index)
    {
        if (context.started)
        {
            _userInputsModel.MoodKeys.Keys[index].SetNewStateIfNecessary(true, 0);
        }
        if (context.canceled)
        {
            _userInputsModel.MoodKeys.Keys[index].SetNewStateIfNecessary(false, 0);
        }
    }
    public void ProcessMoodKey0(CallbackContext context)
    {
        ProcessMoodKeys(context, 0);
    }

    public void ProcessMoodKey1(CallbackContext context)
    {
        ProcessMoodKeys(context, 1);
    }

    public void ProcessMoodKey2(CallbackContext context)
    {
        ProcessMoodKeys(context, 2);
    }

    public void ProcessMoodKey3(CallbackContext context)
    {
        ProcessMoodKeys(context, 3);
    }

    public void ProcessMoodKey4(CallbackContext context)
    {
        ProcessMoodKeys(context, 4);
    }

    public void ProcessMoodKey5(CallbackContext context)
    {
        ProcessMoodKeys(context, 5);
    }

    public void ProcessMoodKey6(CallbackContext context)
    {
        ProcessMoodKeys(context, 6);
    }

    public void ProcessMoodKey7(CallbackContext context)
    {
        ProcessMoodKeys(context, 7);
    }
    #endregion


    #region ExplosionKeys
    public void ProcessExplosionKeys(CallbackContext context, int index)
    {
        if (context.started)
        {
            _userInputsModel.ExplosionKeys.Keys[index].SetNewStateIfNecessary(true, 0);
        }
        if (context.canceled)
        {
            _userInputsModel.ExplosionKeys.Keys[index].SetNewStateIfNecessary(false, 0);
        }
    }
    public void ProcessExplosionKey0(CallbackContext context)
    {
        ProcessExplosionKeys(context, 0);
    }

    public void ProcessExplosionKey1(CallbackContext context)
    {
        ProcessExplosionKeys(context, 1);
    }

    public void ProcessExplosionKey2(CallbackContext context)
    {
        ProcessExplosionKeys(context, 2);
    }

    public void ProcessExplosionKey3(CallbackContext context)
    {
        ProcessExplosionKeys(context, 3);
    }

    public void ProcessExplosionKey4(CallbackContext context)
    {
        ProcessExplosionKeys(context, 4);
    }

    public void ProcessExplosionKey5(CallbackContext context)
    {
        ProcessExplosionKeys(context, 5);
    }

    public void ProcessExplosionKey6(CallbackContext context)
    {
        ProcessExplosionKeys(context, 6);
    }

    public void ProcessExplosionKey7(CallbackContext context)
    {
        ProcessExplosionKeys(context, 7);
    }
    #endregion
}

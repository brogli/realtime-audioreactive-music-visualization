using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class DeviceToLogicalInputMapper : MonoBehaviour
{
    private UserInputsModel _userInputsModel;
    private GameSettingsHandler _gameSettingsHandler;

    // Start is called before the first frame update
    void Start()
    {
        _userInputsModel = GameObject.FindGameObjectWithTag("UserInputsModel").GetComponent<UserInputsModel>();
        _gameSettingsHandler = GameObject.FindGameObjectWithTag("GameSettingsHandler").GetComponent<GameSettingsHandler>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IUserInput GetLogicalInputByChannelAndNote(int channel, int note)
    {
        SettingsNoteInput noteInput;
        if (_gameSettingsHandler.GameSettings.midiDeviceInputs.noteInputs.TryGetValue(channel + "_" + note, out noteInput))
        {
            System.Object property = _userInputsModel.GetType().GetProperty(noteInput.targetProperty).GetValue(_userInputsModel);

            if (noteInput.collection)
            {
                return HandleCollectionNoteInput(noteInput, property);
            }
            else
            {
                IUserInput userInput = (IUserInput)property;
                return userInput;
            }
        }
        else
        {
            Debug.Log("Note not found");
            return null;
        }
    }

    private IUserInput HandleCollectionNoteInput(SettingsNoteInput noteInput, object property)
    {
        if (noteInput.type == InputType.ToggeledUserInput)
        {
            UserInputCollectionOfEight<ToggeledUserInput> userInputCollection = (UserInputCollectionOfEight<ToggeledUserInput>)property;
            return userInputCollection.Keys[noteInput.targetIndex];

        }
        else if (noteInput.type == InputType.FadedUserInput)
        {
            UserInputCollectionOfEight<FadedUserinput> userInputCollection = (UserInputCollectionOfEight<FadedUserinput>)property;
            return userInputCollection.Keys[noteInput.targetIndex];
        }
        else
        {
            // TriggeredUserInput
            UserInputCollectionOfEight<TriggeredUserInput> userInputCollection = (UserInputCollectionOfEight<TriggeredUserInput>)property;
            return userInputCollection.Keys[noteInput.targetIndex];
        }
    }


    public IUserInput GetLogicalInputByChannelAndCcNumber(int channel, int ccNumber)
    {
        SettingsCcInput ccInput;
        if (_gameSettingsHandler.GameSettings.midiDeviceInputs.ccInputs.TryGetValue(channel + "_" + ccNumber, out ccInput))
        {
            System.Object property = _userInputsModel.GetType().GetProperty(ccInput.targetProperty).GetValue(_userInputsModel);

            IUserInput userInput = (IUserInput)property;
            return userInput;
            
        }
        else
        {
            Debug.Log("Note not found");
            return null;
        }
    }
}

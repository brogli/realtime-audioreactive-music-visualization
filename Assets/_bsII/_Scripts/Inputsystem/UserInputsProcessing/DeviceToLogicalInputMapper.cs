using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class DeviceToLogicalInputMapper : MonoBehaviour
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

    public IUserInput GetLogicalInputByChannelAndNote(int channel, int note)
    {
        // example:
        // channel 0, note 72 -> melodyKey index 0
        if (channel == 0 && note == 72)
        {
            System.Object property =_userInputsModel.GetType().GetProperty("MelodyKeys").GetValue(_userInputsModel);
            UserInputCollectionOfEight<IUserInput> userInputCollection = (UserInputCollectionOfEight<IUserInput>)property;
            return userInputCollection.Keys[0];
        }
        else
        {
            return null;
        }
    }

    public IUserInput GetLogicalInputByChannelAndCcNumber(int channel, int ccNumber)
    {
        throw new NotImplementedException();
    }
}

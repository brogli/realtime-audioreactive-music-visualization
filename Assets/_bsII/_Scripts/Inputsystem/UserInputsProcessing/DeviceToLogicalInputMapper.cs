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
            //    return _userInputsModel.MelodyKeys.Keys[0];
            System.Object hello =_userInputsModel.GetType().GetProperty("MelodyKeys").GetValue(_userInputsModel);
            ToggeledUserInputs<MelodyKey> vello = (ToggeledUserInputs<MelodyKey>)hello;
            MelodyKey bello = vello.Keys[0];

            //Debug.Log("hody");
            return bello;
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

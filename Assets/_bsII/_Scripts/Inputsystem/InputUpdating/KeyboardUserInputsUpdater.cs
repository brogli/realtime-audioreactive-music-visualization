﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardUserInputsUpdater : IUserInputsUpdater
{
    private ToggeledUserInputs<MelodyKey> melodyKeys;
    private KeyCode[] numberKeyCodes = { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8 };
    private KeyCode melodyKeysModifierKey = KeyCode.LeftShift;

    public KeyboardUserInputsUpdater(UserInputsProvider userInputsProvider)
    {
        initializeUpdateAccess(userInputsProvider);
    }

    private void initializeUpdateAccess(UserInputsProvider userInputsProvider)
    {
        melodyKeys = userInputsProvider.melodyKeys;
    }

    public void UpdateDroneKeys()
    {
        throw new System.NotImplementedException();
    }

    public void UpdateEightInFour()
    {
        throw new System.NotImplementedException();
    }

    public void UpdateExplosionTriggers()
    {
        throw new System.NotImplementedException();
    }

    public void UpdateFourInFour()
    {
        throw new System.NotImplementedException();
    }

    public void UpdateGlobalIntensity()
    {
        throw new System.NotImplementedException();
    }

    public void UpdateMelodyKeys()
    {
        if (Input.GetKey(melodyKeysModifierKey))
        {
            for (int i = 0; i < melodyKeys.keys.Length; i++)
            {
                melodyKeys.keys[i].SetNewStateIfNecessary(Input.GetKey(numberKeyCodes[i]));
            }
        }
    }

    public void UpdateMoodKeys()
    {
        throw new System.NotImplementedException();
    }

    public void UpdateOneInFour()
    {
        throw new System.NotImplementedException();
    }

    public void UpdateSixteenInFour()
    {
        throw new System.NotImplementedException();
    }

    public void UpdateTwoInFour()
    {
        throw new System.NotImplementedException();
    }

    public void UpdateVisualEffects()
    {
        throw new System.NotImplementedException();
    }

    public void UpdateVolume()
    {
        throw new System.NotImplementedException();
    }

    public void UpdateUserInputs()
    {
        UpdateMelodyKeys();
    }
}

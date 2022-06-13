using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInputsModel : MonoBehaviour
{ 
    #region management related
    public TriggeredUserInput ReloadGameSettings { get; private set; }
    public TriggeredUserInput ResetGameSettingsToDefaults { get; private set; }
    public TriggeredUserInput LoadScene { get; private set; }
    public TriggeredUserInput ActivateScene { get; private set; }
    #endregion

    #region audio related
    public UserInputCollectionOfEight<ToggeledUserInput> MelodyKeys { get; private set; }
    public UserInputCollectionOfEight<ToggeledUserInput> DroneKeys { get; private set; }
    public UserInputCollectionOfEight<TriggeredUserInput> MoodKeys { get; private set; }
    public UserInputCollectionOfEight<TriggeredUserInput> ExplosionKeys { get; private set; }
    #endregion

    #region beat user inputs
    public ToggeledAndFadedUserInput FourInFourUserInput { get; private set; }
    public ToggeledAndFadedUserInput OneInFourUserInput { get; private set; }
    public ToggeledAndFadedUserInput TwoInFourUserInput { get; private set; }
    public ToggeledAndFadedUserInput EightInFourUserInput { get; private set; }
    public ToggeledAndFadedUserInput SixteenInFourUserInput { get; private set; }
    public ToggeledAndFadedUserInput OneInEightUserInput { get; private set; }
    #endregion

    #region volume things
    public ToggeledAndFadedUserInput AverageVolume { get; private set; }
    public ToggeledAndFadedUserInput LowFrequencyVolume { get; private set; }
    #endregion

    public void Awake()
    {
        InitializeMelodyMoodDroneExplosionKeys();
        InitializeManagementInputs();
        InitializeBeatUserInputs();
        InitializeVolumeElements();
    }

    private void InitializeVolumeElements()
    {
        AverageVolume = new ToggeledAndFadedUserInput();
        LowFrequencyVolume = new ToggeledAndFadedUserInput();
    }

    private void InitializeBeatUserInputs()
    {
        FourInFourUserInput = new ToggeledAndFadedUserInput();
        OneInFourUserInput = new ToggeledAndFadedUserInput();
        TwoInFourUserInput = new ToggeledAndFadedUserInput();
        EightInFourUserInput = new ToggeledAndFadedUserInput();
        SixteenInFourUserInput = new ToggeledAndFadedUserInput();
        OneInEightUserInput = new ToggeledAndFadedUserInput();
    }

    private void InitializeManagementInputs()
    {
        ReloadGameSettings = new TriggeredUserInput();
        ResetGameSettingsToDefaults = new TriggeredUserInput();
        LoadScene = new TriggeredUserInput();
        ActivateScene = new TriggeredUserInput();
    }

    private void InitializeMelodyMoodDroneExplosionKeys()
    {
        var melodyKeys = new ToggeledUserInput[8];
        for (int i = 0; i < melodyKeys.Length; i++)
        {
            melodyKeys[i] = new ToggeledUserInput();
        }
        MelodyKeys = new UserInputCollectionOfEight<ToggeledUserInput>(melodyKeys);

        var droneKeys = new ToggeledUserInput[8];
        for (int i = 0; i < melodyKeys.Length; i++)
        {
            droneKeys[i] = new ToggeledUserInput();
        }
        DroneKeys = new UserInputCollectionOfEight<ToggeledUserInput>(droneKeys);

        var moodKeys = new TriggeredUserInput[8];
        for (int i = 0; i < melodyKeys.Length; i++)
        {
            moodKeys[i] = new TriggeredUserInput(i);
        }
        MoodKeys = new UserInputCollectionOfEight<TriggeredUserInput>(moodKeys);

        var explosionKeys = new TriggeredUserInput[8];
        for (int i = 0; i < melodyKeys.Length; i++)
        {
            explosionKeys[i] = new TriggeredUserInput(i);
        }
        ExplosionKeys = new UserInputCollectionOfEight<TriggeredUserInput>(explosionKeys);
    }

    public void OnApplicationQuit()
    {
        FourInFourUserInput?.Unsubscribe();
        OneInFourUserInput?.Unsubscribe();
        TwoInFourUserInput?.Unsubscribe();
        EightInFourUserInput?.Unsubscribe();
        SixteenInFourUserInput?.Unsubscribe();
        OneInEightUserInput?.Unsubscribe();
        AverageVolume?.Unsubscribe();
        LowFrequencyVolume?.Unsubscribe();
    }
}

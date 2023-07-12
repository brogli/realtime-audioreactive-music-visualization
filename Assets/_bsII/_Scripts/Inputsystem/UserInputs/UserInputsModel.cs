using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UserInputsModel : MonoBehaviour
{ 
    #region management related
    public TriggeredUserInput ReloadGameSettings { get; private set; }
    public TriggeredUserInput ResetGameSettingsToDefaults { get; private set; }
    public TriggeredUserInput LoadScene { get; private set; }
    public TriggeredUserInput ActivateScene { get; private set; }
    public TriggeredUserInput SelectNextScene { get; private set; }
    public TriggeredUserInput SelectPreviousScene { get; private set; }
    public TwoSidedFadedUserInput SceneScroller { get; private set; }
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
    public TriggeredUserInput SetOneInFourToNow { get; private set; }
    public TriggeredUserInput SetOneInEightToNow { get; private set; }
    public TwoSidedFadedUserInput BeatClockOffset { get; private set; }
    #endregion

    #region volume things
    public ToggeledAndFadedUserInput AverageVolume { get; private set; }
    public ToggeledAndFadedUserInput LowFrequencyVolume { get; private set; }
    #endregion

    #region global post procesing effects
    public FadedUserInput FadeToBlur { get; private set; }
    public FadedUserInput FadeToWhite { get; private set; }
    public FadedUserInput FadeToBlack { get; private set; }
    
    #endregion

    public void Awake()
    {
        InitializeMelodyMoodDroneExplosionKeys();
        InitializeManagementInputs();
        InitializeBeatUserInputs();
        InitializeVolumeElements();
        InitializePostProcessingElements();
        SceneManager.sceneLoaded += ResetUserInputValidationFlags;
    }

    private void ResetUserInputValidationFlags(Scene arg0, LoadSceneMode arg1)
    {
        MelodyKeys.Keys.ToList().ForEach(key => { key.ResetValidationFlags(); });
        DroneKeys.Keys.ToList().ForEach(key => { key.ResetValidationFlags(); });
        MoodKeys.Keys.ToList().ForEach(key => { key.ResetValidationFlags(); });
        ExplosionKeys.Keys.ToList().ForEach(key => { key.ResetValidationFlags(); });
        FourInFourUserInput.ResetValidationFlags();
        OneInFourUserInput.ResetValidationFlags();
        TwoInFourUserInput.ResetValidationFlags();
        EightInFourUserInput.ResetValidationFlags();
        SixteenInFourUserInput.ResetValidationFlags();
        OneInEightUserInput.ResetValidationFlags();
        SetOneInFourToNow.ResetValidationFlags();
        SetOneInEightToNow.ResetValidationFlags();
        AverageVolume.ResetValidationFlags();
        LowFrequencyVolume.ResetValidationFlags();
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

        SetOneInFourToNow = new TriggeredUserInput();
        SetOneInEightToNow = new TriggeredUserInput();

        BeatClockOffset = new TwoSidedFadedUserInput(0.15f);
    }

    private void InitializeManagementInputs()
    {
        ReloadGameSettings = new TriggeredUserInput();
        ResetGameSettingsToDefaults = new TriggeredUserInput();
        LoadScene = new TriggeredUserInput();
        ActivateScene = new TriggeredUserInput();
        SelectNextScene = new TriggeredUserInput();
        SelectPreviousScene = new TriggeredUserInput();
        SceneScroller = new TwoSidedFadedUserInput(0.15f);
    }

    private void InitializeMelodyMoodDroneExplosionKeys()
    {
        var melodyKeys = new ToggeledUserInput[8];
        for (int i = 0; i < melodyKeys.Length; i++)
        {
            melodyKeys[i] = new ToggeledUserInput(i);
        }
        MelodyKeys = new UserInputCollectionOfEight<ToggeledUserInput>(melodyKeys);

        var droneKeys = new ToggeledUserInput[8];
        for (int i = 0; i < droneKeys.Length; i++)
        {
            droneKeys[i] = new ToggeledUserInput(i);
        }
        DroneKeys = new UserInputCollectionOfEight<ToggeledUserInput>(droneKeys);

        var moodKeys = new TriggeredUserInput[8];
        for (int i = 0; i < moodKeys.Length; i++)
        {
            moodKeys[i] = new TriggeredUserInput(i);
        }
        MoodKeys = new UserInputCollectionOfEight<TriggeredUserInput>(moodKeys);

        var explosionKeys = new TriggeredUserInput[8];
        for (int i = 0; i < explosionKeys.Length; i++)
        {
            explosionKeys[i] = new TriggeredUserInput(i);
        }
        ExplosionKeys = new UserInputCollectionOfEight<TriggeredUserInput>(explosionKeys);
    }

    private void InitializePostProcessingElements()
    {
        FadeToBlur = new();
        FadeToWhite = new();
        FadeToBlack = new();
    }

    public void OnApplicationQuit()
    {
        SceneManager.sceneLoaded -= ResetUserInputValidationFlags;
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

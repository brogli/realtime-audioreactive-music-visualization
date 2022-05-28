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
    public UserInputCollectionOfEight<ToggeledUserInput> DroneKeys { get; private set; }
    public UserInputCollectionOfEight<TriggeredUserInput> MoodKeys { get; private set; }
    public UserInputCollectionOfEight<TriggeredUserInput> ExplosionKeys { get; private set; }
    #endregion

    #region beat user inputs
    public ToggeledUserInput FourInFourUserInput { get; private set; }
    public ToggeledUserInput OneInFourUserInput { get; private set; }
    public ToggeledUserInput TwoInFourUserInput { get; private set; }
    public ToggeledUserInput EightInFourUserInput { get; private set; }
    public ToggeledUserInput SixteenInFourUserInput { get; private set; }
    public ToggeledUserInput OneInEightUserInput { get; private set; }
    #endregion

    public void Awake()
    {
        InitializeMelodyMoodDroneExplosionKeys();
        InitializeManagementInputs();
        InitializeBeatUserInputs();
    }

    private void InitializeBeatUserInputs()
    {
        FourInFourUserInput = new ToggeledUserInput();
        OneInFourUserInput = new ToggeledUserInput();
        TwoInFourUserInput = new ToggeledUserInput();
        EightInFourUserInput = new ToggeledUserInput();
        SixteenInFourUserInput = new ToggeledUserInput();
        OneInEightUserInput = new ToggeledUserInput();
    }

    private void InitializeManagementInputs()
    {
        ReloadGameSettings = new TriggeredUserInput();
        ResetGameSettingsToDefaults = new TriggeredUserInput();
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
}

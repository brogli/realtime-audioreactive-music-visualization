using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// After scene a is loaded, this waits 2 seconds, so a few frames have passed, then validates that all necessary user inputs are used in the scene and prints warnings otherwise.
/// </summary>
public class SceneImplementationValidator : MonoBehaviour
{
    private UserInputsModel _userInputsModel;
    private bool _isCoroutineRunning = false;
    private SceneHandler _sceneHandler;

    void Start()
    {
        _userInputsModel = GameObject.FindGameObjectWithTag("UserInputsModel").GetComponent<UserInputsModel>();
        _sceneHandler = GameObject.FindGameObjectWithTag("SceneHandler").GetComponent<SceneHandler>();
        _sceneHandler.EmitSceneIsReadyToActivate += HandleNewSceneReadyToActivate;

        SceneManager.sceneLoaded += HandleSceneLoaded;

        // since in first scene the sceneLoaded event is not triggered
        StartValidation();
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= HandleSceneLoaded;
        _sceneHandler.EmitSceneIsReadyToActivate -= HandleNewSceneReadyToActivate;
    }

    private void HandleNewSceneReadyToActivate()
    {
        StopValidating();
    }

    private void HandleSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        StartValidation();
    }

    public void StopValidating()
    {
        if (_isCoroutineRunning)
        {
            StopCoroutine("ValidateUserInputs");
            _isCoroutineRunning = false;
        }
    }

    private void StartValidation()
    {
        StopValidating();
        StartCoroutine("ValidateUserInputs");
    }

    private IEnumerator ValidateUserInputs()
    {
        _isCoroutineRunning = true;

        int secondsToWait = 2;
        Debug.Log($"Starting user inputs validation in {secondsToWait} seconds");
        yield return new WaitForSeconds(secondsToWait);

        int problemCount = 0;
        problemCount += ValidateMelodyKeys();
        problemCount += ValidateDroneKeys();
        problemCount += ValidateMoodKeys();
        problemCount += ValidateExplosionKeys();

        problemCount += ValidateFourInFour();
        problemCount += ValidateOneInFour();
        problemCount += ValidateTwoInFour();
        problemCount += ValidateEightInFour();
        problemCount += ValidateSixteenInFour();
        problemCount += ValidateOneInEight();
        problemCount += ValidateLowFrequencyVolume();
        problemCount += ValidateAverageVolume();

        if (problemCount == 0)
        {
            Debug.Log($"User input validation successful.");
        }
        else
        {
            Debug.LogWarning($"Validation finished with {problemCount} problems.");
        }
        _isCoroutineRunning = false;
    }

    #region volume related

    private int ValidateLowFrequencyVolume()
    {
        int problemCount = 0;

        if (!_userInputsModel.LowFrequencyVolume.IsUsed())
        {
            problemCount++;
            Debug.LogWarning($"LowFrequencyVolume user input is not fully used.");
        }
        return problemCount;
    }

    private int ValidateAverageVolume()
    {
        int problemCount = 0;

        if (!_userInputsModel.AverageVolume.IsUsed())
        {
            problemCount++;
            Debug.LogWarning($"AverageVolume user input is not fully used.");
        }
        return problemCount;
    }

    #endregion

    #region beat related

    private int ValidateOneInEight()
    {
        int problemCount = 0;

        if (!_userInputsModel.OneInEightUserInput.IsUsed())
        {
            problemCount++;
            Debug.LogWarning($"OneInEight user input is not fully used.");
        }
        return problemCount;
    }

    private int ValidateSixteenInFour()
    {
        int problemCount = 0;

        if (!_userInputsModel.SixteenInFourUserInput.IsUsed())
        {
            problemCount++;
            Debug.LogWarning($"SixteenInFour user input is not fully used.");
        }
        return problemCount;
    }

    private int ValidateEightInFour()
    {
        int problemCount = 0;

        if (!_userInputsModel.EightInFourUserInput.IsUsed())
        {
            problemCount++;
            Debug.LogWarning($"EightInFour user input is not fully used.");
        }
        return problemCount;
    }


    private int ValidateTwoInFour()
    {
        int problemCount = 0;

        if (!_userInputsModel.TwoInFourUserInput.IsUsed())
        {
            problemCount++;
            Debug.LogWarning($"TwoInFour user input is not fully used.");
        }
        return problemCount;
    }


    private int ValidateOneInFour()
    {
        int problemCount = 0;

        if (!_userInputsModel.OneInFourUserInput.IsUsed())
        {
            problemCount++;
            Debug.LogWarning($"OneInFour user input is not fully used.");
        }
        return problemCount;
    }

    private int ValidateFourInFour()
    {
        int problemCount = 0;

        if (!_userInputsModel.FourInFourUserInput.IsUsed())
        {
            problemCount++;
            Debug.LogWarning($"FourInFour user input is not fully used.");
        }
        return problemCount;
    }

    #endregion

    #region audio related
    private int ValidateExplosionKeys()
    {
        int problemCount = 0;
        for (int i = 0; i < _userInputsModel.ExplosionKeys.Keys.Length; i++)
        {
            var key = _userInputsModel.ExplosionKeys.Keys[i];
            if (!key.IsUsed())
            {
                problemCount++;
                Debug.LogWarning($"Explosion key with index {i} is not used");
            }
        }
        return problemCount;
    }

    private int ValidateMoodKeys()
    {
        int problemCount = 0;
        for (int i = 0; i < _userInputsModel.MoodKeys.Keys.Length; i++)
        {
            var key = _userInputsModel.MoodKeys.Keys[i];
            if (!key.IsUsed())
            {
                problemCount++;
                Debug.LogWarning($"Mood key with index {i} is not used");
            }
        }
        return problemCount;
    }

    private int ValidateDroneKeys()
    {
        int problemCount = 0;
        for (int i = 0; i < _userInputsModel.DroneKeys.Keys.Length; i++)
        {
            var key = _userInputsModel.DroneKeys.Keys[i];
            if (!key.IsUsed())
            {
                problemCount++;
                Debug.LogWarning($"Drone key with index {i} is not used");
            }
        }
        return problemCount;
    }

    private int ValidateMelodyKeys()
    {
        int problemCount = 0;
        for (int i = 0; i < _userInputsModel.MelodyKeys.Keys.Length; i++)
        {
            var key = _userInputsModel.MelodyKeys.Keys[i];
            if (!key.IsUsed())
            {
                problemCount++;
                Debug.LogWarning($"Melody key with index {i} is not used");
            }
        }
        return problemCount;
    }
    #endregion
}

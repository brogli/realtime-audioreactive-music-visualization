using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GlobalPostProcesses : MonoBehaviour, IUserInputsConsumer
{

    private FadeToBlackOrWhitePostProcess _fadeToBlackOrWhitePostProcess;
    private UserInputsModel _userInputsModel;

    void Start()
    {
        if (!GetComponent<Volume>().sharedProfile.TryGet(out _fadeToBlackOrWhitePostProcess))
        {
            throw new NullReferenceException(nameof(_fadeToBlackOrWhitePostProcess));
        }

        _userInputsModel = GameObject.FindGameObjectWithTag("UserInputsModel").GetComponent<UserInputsModel>();
        SubscribeUserInputs();
    }

    void OnDisable()
    {
        UnsubscribeUserInputs();
    }

    void Update()
    {
        if (_userInputsModel.FadeToWhite.IsActive)
        {
            _fadeToBlackOrWhitePostProcess.lightenIntensity.value = _userInputsModel.FadeToWhite.FaderValue;
        }
        if (_userInputsModel.FadeToBlack.IsActive)
        {
            _fadeToBlackOrWhitePostProcess.darkenIntensity.value = _userInputsModel.FadeToBlack.FaderValue;
        }
    }

    public void SubscribeUserInputs()
    {
        _userInputsModel.FadeToWhite.EmitTurnedOnEvent += HandleFadeToWhiteOn;
        _userInputsModel.FadeToWhite.EmitTurnedOffEvent += HandleFadeToWhiteOff;

        _userInputsModel.FadeToBlack.EmitTurnedOnEvent += HandleFadeToBlackOn;
        _userInputsModel.FadeToBlack.EmitTurnedOffEvent += HandleFadeToBlackOff;
    }

    public void UnsubscribeUserInputs()
    {
        _userInputsModel.FadeToWhite.EmitTurnedOnEvent -= HandleFadeToWhiteOn;
        _userInputsModel.FadeToWhite.EmitTurnedOffEvent -= HandleFadeToWhiteOff;

        _userInputsModel.FadeToBlack.EmitTurnedOnEvent -= HandleFadeToBlackOn;
        _userInputsModel.FadeToBlack.EmitTurnedOffEvent -= HandleFadeToBlackOff;
    }

    #region handle fade to black and to white
    private void HandleFadeToWhiteOn()
    {
        _fadeToBlackOrWhitePostProcess.intensity.value = 1;
        _fadeToBlackOrWhitePostProcess.lightenIntensity.overrideState = true;

    }

    private void HandleFadeToWhiteOff()
    {
        _fadeToBlackOrWhitePostProcess.lightenIntensity.overrideState = false;

        if (!_userInputsModel.FadeToBlack.IsActive)
        {
            _fadeToBlackOrWhitePostProcess.intensity.value = 0;
        }
    }

    private void HandleFadeToBlackOn()
    {
        _fadeToBlackOrWhitePostProcess.intensity.value = 1;
        _fadeToBlackOrWhitePostProcess.darkenIntensity.overrideState = true;
    }

    private void HandleFadeToBlackOff()
    {
        _fadeToBlackOrWhitePostProcess.darkenIntensity.overrideState = false;

        if (!_userInputsModel.FadeToWhite.IsActive)
        {
            _fadeToBlackOrWhitePostProcess.intensity.value = 0;
        }
    }
    #endregion


}

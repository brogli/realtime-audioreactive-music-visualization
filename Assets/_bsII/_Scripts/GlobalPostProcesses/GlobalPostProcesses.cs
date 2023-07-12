using IE.RichFX;
using SnapshotShaders.HDRP;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GlobalPostProcesses : MonoBehaviour, IUserInputsConsumer
{

    private FadeToBlackOrWhitePostProcess _fadeToBlackOrWhitePostProcess;
    private Blur _blurPostProcess;
    private GaussianBlur _gaussianBlur;
    private UserInputsModel _userInputsModel;


    void Start()
    {
        if (!GetComponent<Volume>().sharedProfile.TryGet(out _fadeToBlackOrWhitePostProcess))
        {
            throw new NullReferenceException(nameof(_fadeToBlackOrWhitePostProcess));
        }

        if (!GetComponent<Volume>().sharedProfile.TryGet(out _blurPostProcess))
        {
            throw new NullReferenceException(nameof(_blurPostProcess));
        }

        if (!GetComponent<Volume>().sharedProfile.TryGet(out _gaussianBlur))
        {
            throw new NullReferenceException(nameof(_gaussianBlur));
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

        _userInputsModel.FadeToBlur.EmitTurnedOffEvent += HandleFadeToBlurOn;
        _userInputsModel.FadeToBlur.EmitTurnedOffEvent += HandleFadeToBlurOff;
    }

    public void UnsubscribeUserInputs()
    {
        _userInputsModel.FadeToWhite.EmitTurnedOnEvent -= HandleFadeToWhiteOn;
        _userInputsModel.FadeToWhite.EmitTurnedOffEvent -= HandleFadeToWhiteOff;

        _userInputsModel.FadeToBlack.EmitTurnedOnEvent -= HandleFadeToBlackOn;
        _userInputsModel.FadeToBlack.EmitTurnedOffEvent -= HandleFadeToBlackOff;

        _userInputsModel.FadeToBlur.EmitTurnedOffEvent -= HandleFadeToBlurOn;
        _userInputsModel.FadeToBlur.EmitTurnedOffEvent -= HandleFadeToBlurOff;
    }

    #region handle fade to black, to white and to blur
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

    private void HandleFadeToBlurOn()
    {
        //_blurPostProcess.active = true;
        //_blurPostProcess.strength.overrideState = true;
        //_blurPostProcess.strength.value = 300;
        _gaussianBlur.active = true;
        _gaussianBlur.intensity.overrideState = true;
        //_gaussianBlur.intensity.value = 100;
    }

    private void HandleFadeToBlurOff()
    {
        //.darkenIntensity.overrideState = false;


    }
    #endregion


}

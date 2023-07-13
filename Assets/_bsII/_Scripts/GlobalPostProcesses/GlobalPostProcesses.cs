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
    private SimpleGaussianBlur _simpleGaussianBlur;
    private SimpleGaussianBlur1 _simpleGaussianBlur1;
    private SimpleGaussianBlur2 _simpleGaussianBlur2;
    private Kaleidoscope _kaleidoscope;
    private SobelNeon _sobelNeon;
    private Invert _colorInvert;
    private RainbowFlow _rainbowFlow;

    private UserInputsModel _userInputsModel;

    void Start()
    {
        SetupVolumeOverrides();

        _userInputsModel = GameObject.FindGameObjectWithTag("UserInputsModel").GetComponent<UserInputsModel>();
        SubscribeUserInputs();

    }


    void OnDisable()
    {
        UnsubscribeUserInputs();
    }
    private void SetupVolumeOverrides()
    {

        if (!GetComponent<Volume>().sharedProfile.TryGet(out _fadeToBlackOrWhitePostProcess))
        {
            throw new NullReferenceException(nameof(_fadeToBlackOrWhitePostProcess));
        }

        if (!GetComponent<Volume>().sharedProfile.TryGet(out _blurPostProcess))
        {
            throw new NullReferenceException(nameof(_blurPostProcess));
        }

        if (!GetComponent<Volume>().sharedProfile.TryGet(out _simpleGaussianBlur))
        {
            throw new NullReferenceException(nameof(_simpleGaussianBlur));
        }

        if (!GetComponent<Volume>().sharedProfile.TryGet(out _simpleGaussianBlur1))
        {
            throw new NullReferenceException(nameof(_simpleGaussianBlur1));
        }

        if (!GetComponent<Volume>().sharedProfile.TryGet(out _simpleGaussianBlur2))
        {
            throw new NullReferenceException(nameof(_simpleGaussianBlur2));
        }

        if (!GetComponent<Volume>().sharedProfile.TryGet(out _kaleidoscope))
        {
            throw new NullReferenceException(nameof(_kaleidoscope));
        }

        if (!GetComponent<Volume>().sharedProfile.TryGet(out _sobelNeon))
        {
            throw new NullReferenceException(nameof(_sobelNeon));
        }

        if (!GetComponent<Volume>().sharedProfile.TryGet(out _colorInvert))
        {
            throw new NullReferenceException(nameof(_colorInvert));
        }

        if (!GetComponent<Volume>().sharedProfile.TryGet(out _rainbowFlow))
        {
            throw new NullReferenceException(nameof(_rainbowFlow));
        }
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
        if (_userInputsModel.FadeToBlur.IsActive)
        {
            _blurPostProcess.strength.value = Mathf.RoundToInt(_userInputsModel.FadeToBlur.FaderValue * _blurPostProcess.strength.max);
            _simpleGaussianBlur.intensity.value = _userInputsModel.FadeToBlur.FaderValue;
            _simpleGaussianBlur1.intensity.value = _userInputsModel.FadeToBlur.FaderValue;
            _simpleGaussianBlur2.intensity.value = _userInputsModel.FadeToBlur.FaderValue;
        }
    }

    public void SubscribeUserInputs()
    {
        _userInputsModel.FadeToWhite.EmitTurnedOnEvent += HandleFadeToWhiteOn;
        _userInputsModel.FadeToWhite.EmitTurnedOffEvent += HandleFadeToWhiteOff;

        _userInputsModel.FadeToBlack.EmitTurnedOnEvent += HandleFadeToBlackOn;
        _userInputsModel.FadeToBlack.EmitTurnedOffEvent += HandleFadeToBlackOff;

        _userInputsModel.FadeToBlur.EmitTurnedOnEvent += HandleFadeToBlurOn;
        _userInputsModel.FadeToBlur.EmitTurnedOffEvent += HandleFadeToBlurOff;

        _userInputsModel.Kaleidoscope.EmitTurnedOnOrOffEvent += HandleKaleidoscope;
        _userInputsModel.ColorInvert.EmitTurnedOnOrOffEvent += HandleColorInvert;
        _userInputsModel.ColorInvertFourInFour.EmitTurnedOnOrOffEvent += HandleColorInvertFourInFour;
        _userInputsModel.Strobo.EmitTurnedOnOrOffEvent += HandleStrobo;
        _userInputsModel.RandomTextOverlay.EmitKeyTriggeredEvent += HandleRandomTextOverlay;
        _userInputsModel.ColorOverlayFourInFour.EmitTurnedOnOrOffEvent += HandleColorOverlayFourInFour;
        _userInputsModel.SobelNeon.EmitTurnedOnOrOffEvent += HandleSobelNeon;
        _userInputsModel.RainbowFlow.EmitTurnedOnOrOffEvent += HandleRainbowFlow;
    }

    public void UnsubscribeUserInputs()
    {
        _userInputsModel.FadeToWhite.EmitTurnedOnEvent -= HandleFadeToWhiteOn;
        _userInputsModel.FadeToWhite.EmitTurnedOffEvent -= HandleFadeToWhiteOff;

        _userInputsModel.FadeToBlack.EmitTurnedOnEvent -= HandleFadeToBlackOn;
        _userInputsModel.FadeToBlack.EmitTurnedOffEvent -= HandleFadeToBlackOff;

        _userInputsModel.FadeToBlur.EmitTurnedOnEvent -= HandleFadeToBlurOn;
        _userInputsModel.FadeToBlur.EmitTurnedOffEvent -= HandleFadeToBlurOff;

        _userInputsModel.Kaleidoscope.EmitTurnedOnOrOffEvent -= HandleKaleidoscope;
        _userInputsModel.ColorInvert.EmitTurnedOnOrOffEvent -= HandleColorInvert;
        _userInputsModel.ColorInvertFourInFour.EmitTurnedOnOrOffEvent -= HandleColorInvertFourInFour;
        _userInputsModel.Strobo.EmitTurnedOnOrOffEvent -= HandleStrobo;
        _userInputsModel.RandomTextOverlay.EmitKeyTriggeredEvent -= HandleRandomTextOverlay;
        _userInputsModel.ColorOverlayFourInFour.EmitTurnedOnOrOffEvent -= HandleColorOverlayFourInFour;
        _userInputsModel.SobelNeon.EmitTurnedOnOrOffEvent -= HandleSobelNeon;
        _userInputsModel.RainbowFlow.EmitTurnedOnOrOffEvent -= HandleRainbowFlow;
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
        _blurPostProcess.active = true;
        _blurPostProcess.strength.overrideState = true;

        _simpleGaussianBlur.active = true;
        _simpleGaussianBlur.intensity.overrideState = true;

        _simpleGaussianBlur1.active = true;
        _simpleGaussianBlur1.intensity.overrideState = true;

        _simpleGaussianBlur2.active = true;
        _simpleGaussianBlur2.intensity.overrideState = true;
    }

    private void HandleFadeToBlurOff()
    {
        _blurPostProcess.strength.overrideState = false;
        _blurPostProcess.active = false;

        _simpleGaussianBlur.active = false;
        _simpleGaussianBlur.intensity.overrideState = false;

        _simpleGaussianBlur1.active = false;
        _simpleGaussianBlur1.intensity.overrideState = false;

        _simpleGaussianBlur2.active = false;
        _simpleGaussianBlur2.intensity.overrideState = false;
    }
    #endregion

    #region misc


    private void HandleRainbowFlow(bool hasTurnedOn, int index)
    {
        if (hasTurnedOn) {
            _rainbowFlow.active = true;
        } else
        {
            _rainbowFlow.active = false;

        }
    }

    private void HandleSobelNeon(bool hasTurnedOn, int index)
    {
        if (hasTurnedOn)
        {
            _sobelNeon.active = true;
            _sobelNeon.enabled.overrideState = true;
            _sobelNeon.enabled.value = true;
        }
        else
        {
            _sobelNeon.active = false;
            _sobelNeon.enabled.overrideState = true;
            _sobelNeon.enabled.value = false;
        }
    }

    private void HandleColorOverlayFourInFour(bool hasTurnedOn, int index)
    {
        if (hasTurnedOn)
        {
            Debug.Log("color overlay 4-4 on");
        }
        else
        {

        }
    }

    private void HandleRandomTextOverlay()
    {
        Debug.Log("random text overlay");
    }

    private void HandleStrobo(bool hasTurnedOn, int index)
    {
        if (hasTurnedOn)
        {
            Debug.Log("strobo on");
        }
        else
        {

        }
    }

    private void HandleColorInvertFourInFour(bool hasTurnedOn, int index)
    {
        if (hasTurnedOn)
        {
            Debug.Log("colorinvert 4-4 on");
        }
        else
        {

        }
    }

    private void HandleColorInvert(bool hasTurnedOn, int index)
    {
        if (hasTurnedOn)
        {
            _colorInvert.active = true;
            _colorInvert.intensity.overrideState = true;
        }
        else
        {
            _colorInvert.active = false;
            _colorInvert.intensity.overrideState = false;
        }
    }

    private void HandleKaleidoscope(bool hasTurnedOn, int index)
    {
        if (hasTurnedOn)
        {
            _kaleidoscope.active = true;
            _kaleidoscope.segmentCount.overrideState = true;
        }
        else
        {
            _kaleidoscope.active = false;
            _kaleidoscope.segmentCount.overrideState = false;
        }
    }
    #endregion
}

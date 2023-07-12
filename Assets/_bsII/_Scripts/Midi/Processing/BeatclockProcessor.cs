using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatclockProcessor : MonoBehaviour, IUserInputsConsumer
{
    private MusicInputsModel _musicInputsModel;
    private UserInputsModel _userInputsModel;
    private int _fourInFourValue = 0;
    private int _oneInFourValue = 0;
    private int _twoInFourValue = 0;
    private int _eightInFourValue = 0;
    private int _sixteenInFourValue = 0;
    private int _oneInEightValue = 0;

    private const float QuarterAmountClockPulses = 6;
    private const float HalfAmountClockPulses = 12;
    private const float RegularAmountClockPulses = 24;
    private const float DoubleAmountClockPulses = 48;
    private const float QuadrupleAmountClockPulses = 96;
    private const float EightTimesAmountClockPulses = 192;

    private float _beatClockOffset = 0;

    public void Start()
    {
        _musicInputsModel = GameObject.FindGameObjectWithTag("MusicInputsModel").GetComponent<MusicInputsModel>();
        _userInputsModel = GameObject.FindGameObjectWithTag("UserInputsModel").GetComponent<UserInputsModel>();
        SubscribeUserInputs();
    }

    private void OnDisable()
    {
        UnsubscribeUserInputs();
    }

    public void ProcessBeatclock()
    {
        ProcessClockValues();
        NormalizeAndPushValues();
    }

    private void ProcessClockValues()
    {
        // four in four
        if ((_fourInFourValue + 1) > RegularAmountClockPulses - 1)
        {
            _fourInFourValue = 0;
        }
        else
        {
            _fourInFourValue++;
        }

        // two in four
        if ((_twoInFourValue + 1) > DoubleAmountClockPulses - 1)
        {
            _twoInFourValue = 0;
        }
        else
        {
            _twoInFourValue++;
        }

        // one in four
        if ((_oneInFourValue + 1) > QuadrupleAmountClockPulses - 1)
        {
            _oneInFourValue = 0;
        }
        else
        {
            _oneInFourValue++;
        }

        // eight in four
        if ((_eightInFourValue + 1) > HalfAmountClockPulses - 1)
        {
            _eightInFourValue = 0;
        }
        else
        {
            _eightInFourValue++;
        }

        // sixteen in four
        if ((_sixteenInFourValue + 1) > QuarterAmountClockPulses - 1)
        {
            _sixteenInFourValue = 0;
        }
        else
        {
            _sixteenInFourValue++;
        }

        // one in eight
        if ((_oneInEightValue + 1) > EightTimesAmountClockPulses - 1)
        {
            _oneInEightValue = 0;
        }
        else
        {
            _oneInEightValue++;
        }
    }

    private void NormalizeAndPushValues()
    {
        float beatClockOffset = _userInputsModel.BeatClockOffset.FaderValueNormalizedBetweenMinusAndPlusPointFive;
        //Debug.Log(beatClockOffset); 
        _musicInputsModel.FourInFourValue = BsIImath.AcutalModulo(((_fourInFourValue / RegularAmountClockPulses) + beatClockOffset), 1.0f);
        //float 
        _musicInputsModel.OneInFourValue = BsIImath.AcutalModulo(((_oneInFourValue / QuadrupleAmountClockPulses) + beatClockOffset / 4f), 1.0f);
        _musicInputsModel.TwoInFourValue = BsIImath.AcutalModulo(((_twoInFourValue / DoubleAmountClockPulses) + beatClockOffset / 2), 1.0f);
        _musicInputsModel.EightInFourValue = BsIImath.AcutalModulo(((_eightInFourValue / HalfAmountClockPulses) + beatClockOffset * 2), 1.0f);
        _musicInputsModel.SixteenInFourValue = BsIImath.AcutalModulo(((_sixteenInFourValue / QuarterAmountClockPulses) + beatClockOffset * 4), 1.0f);
        _musicInputsModel.OneInEightValue = BsIImath.AcutalModulo(((_oneInEightValue / EightTimesAmountClockPulses) + beatClockOffset / 8), 1.0f);
    }

    public void SubscribeUserInputs()
    {
        _userInputsModel.SetOneInFourToNow.EmitKeyTriggeredEvent += HandleSetOneInFourToNow;
        _userInputsModel.SetOneInEightToNow.EmitKeyTriggeredEvent += HandleSetOneInEightToNow;
    }

    private void HandleSetOneInEightToNow()
    {
        var currentOneInFourTickValue = _oneInFourValue;
        if (_fourInFourValue < RegularAmountClockPulses)
        {
            _oneInEightValue = _fourInFourValue;
        }
        else
        {
            _oneInEightValue = ((int)EightTimesAmountClockPulses - 1) - _oneInFourValue;
        }


        // oneinfour too:
        if (_fourInFourValue < RegularAmountClockPulses)
        {
            _oneInFourValue = _fourInFourValue;
        }
        else
        {
            _oneInFourValue = ((int)QuadrupleAmountClockPulses - 1) - _oneInFourValue;
        }
    }

    private void HandleSetOneInFourToNow()
    {
        var currentOneInFourTickValue = _oneInFourValue;
        if (_fourInFourValue < RegularAmountClockPulses)
        {
            _oneInFourValue = _fourInFourValue;
        }
        else
        {
            _oneInFourValue = ((int)QuadrupleAmountClockPulses - 1) - _oneInFourValue;
        }
        var newOneInFourTickValue = _oneInFourValue;
        var difference = currentOneInFourTickValue - newOneInFourTickValue;

        // also deal with half of 1-8:
        _oneInEightValue = _oneInEightValue - difference;
    }

    public void UnsubscribeUserInputs()
    {
        _userInputsModel.SetOneInFourToNow.EmitKeyTriggeredEvent -= HandleSetOneInFourToNow;
        _userInputsModel.SetOneInEightToNow.EmitKeyTriggeredEvent -= HandleSetOneInEightToNow;

    }
}

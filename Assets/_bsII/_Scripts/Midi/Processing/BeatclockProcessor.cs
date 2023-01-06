using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatclockProcessor : MonoBehaviour
{
    private MusicInputsModel _musicInputsModel;

    private int _fourInFourValue = 1;
    private int _oneInFourValue = 1;
    private int _twoInFourValue = 1;
    private int _eightInFourValue = 1;
    private int _sixteenInFourValue = 1;
    private int _oneInEightValue = 1;

    private const float QuarterAmountClockPulses = 6;
    private const float HalfAmountClockPulses = 12;
    private const float RegularAmountClockPulses = 24;
    private const float DoubleAmountClockPulses = 48;
    private const float QuadrupleAmountClockPulses = 96;
    private const float EightTimesAmountClockPulses = 192;

    public void Start()
    {
        _musicInputsModel = GameObject.FindGameObjectWithTag("MusicInputsModel").GetComponent<MusicInputsModel>();
    }

    public void ProcessBeatclock()
    {
        ProcessClockValues();
        NormalizeAndPushValues();
    }

    private void ProcessClockValues()
    {
        // four in four
        if ((_fourInFourValue + 1) > RegularAmountClockPulses)
        {
            _fourInFourValue = 1;
        }
        else
        {
            _fourInFourValue++;
        }

        // two in four
        if ((_twoInFourValue + 1) > DoubleAmountClockPulses)
        {
            _twoInFourValue = 1;
        }
        else
        {
            _twoInFourValue++;
        }

        // one in four
        if ((_oneInFourValue + 1) > QuadrupleAmountClockPulses)
        {
            _oneInFourValue = 1;
        }
        else
        {
            _oneInFourValue++;
        }

        // eight in four
        if ((_eightInFourValue + 1) > HalfAmountClockPulses)
        {
            _eightInFourValue = 1;
        }
        else
        {
            _eightInFourValue++;
        }

        // sixteen in four
        if ((_sixteenInFourValue + 1) > QuarterAmountClockPulses)
        {
            _sixteenInFourValue = 1;
        }
        else
        {
            _sixteenInFourValue++;
        }

        // one in eight
        if ((_oneInEightValue + 1) > EightTimesAmountClockPulses)
        {
            _oneInEightValue = 1;
        } else
        {
            _oneInEightValue++;
        }
    }

    private void NormalizeAndPushValues()
    {
        _musicInputsModel.FourInFourValue = _fourInFourValue / RegularAmountClockPulses;
        _musicInputsModel.OneInFourValue = _oneInFourValue / QuadrupleAmountClockPulses;
        _musicInputsModel.TwoInFourValue = _twoInFourValue / DoubleAmountClockPulses;
        _musicInputsModel.EightInFourValue = _eightInFourValue / HalfAmountClockPulses;
        _musicInputsModel.SixteenInFourValue = _sixteenInFourValue / QuarterAmountClockPulses;
        _musicInputsModel.OneInEightValue = _oneInEightValue / EightTimesAmountClockPulses;
    }
}

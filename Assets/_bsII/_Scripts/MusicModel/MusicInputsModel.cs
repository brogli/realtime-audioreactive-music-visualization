using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class MusicInputsModel : MonoBehaviour
{
    #region beat values
    public delegate void BeatEvent();
    public event BeatEvent EmitFourInFourEvent;
    private float _fourInFourValue = 1.0f;
    private Stopwatch _fourInFourStopWatch;
    private float _lastConsumedFourInFourValue;
    private long _timeOfLastConsumedFourInFourValue;

    public float FourInFourValue
    {
        get
        {
            return _fourInFourValue;
        }
        set
        {
            if (_fourInFourValue > 0.5f && value < 0.5f)
            {
                EmitFourInFourEvent?.Invoke();
                _fourInFourStopWatch.Restart();
            }
            _fourInFourValue = value;
        }
    }

    public float FourInFourValueWithIntermediates
    {
        get
        {
            float fourInFourValue = _fourInFourValue;
            
            if (_lastConsumedFourInFourValue == fourInFourValue)
            {
                // would return the same value again, we don't want this, thus we extrapolate
                // y_now = m * x_now + b
                // b is 0
                // x_now we call nowTime
                // m we calculate using y_previous (_lastConsumedFourInFourValue) and x_previous (_timeOfLastConsumedFourInFourValue)
                float m = _lastConsumedFourInFourValue / _timeOfLastConsumedFourInFourValue;
                long nowTime = _fourInFourStopWatch.ElapsedTicks;
                float extrapolatedValue = m * nowTime;
                
                if (extrapolatedValue <= 1)
                {
                    // plausible result
                    fourInFourValue = extrapolatedValue;
                }
            } else
            {
                // there's a new beatclock value available, so we return it
                _timeOfLastConsumedFourInFourValue = _fourInFourStopWatch.ElapsedTicks;
                _lastConsumedFourInFourValue = fourInFourValue;
            }            

            return fourInFourValue;
        }
    }

    public event BeatEvent EmitOneInFourEvent;
    private float oneInFourValue = 1.0f;

    public float OneInFourValue
    {
        get => oneInFourValue;
        set
        {
            if (oneInFourValue > 0.5 && value < 0.5)
            {
                EmitOneInFourEvent?.Invoke();
            }
            oneInFourValue = value;
        }
    }

    public event BeatEvent EmitTwoInFourEvent;
    private float twoInFourValue = 1.0f;

    public float TwoInFourValue
    {
        get => twoInFourValue;
        set
        {
            if (twoInFourValue > 0.5 && value < 0.5)
            {
                EmitTwoInFourEvent?.Invoke();
            }
            twoInFourValue = value;
        }
    }

    public event BeatEvent EmitEightInFourEvent;
    private float eightInFourValue = 1.0f;

    public float EightInFourValue
    {
        get => eightInFourValue;
        set
        {
            if (eightInFourValue > 0.5 && value < 0.5)
            {
                EmitEightInFourEvent?.Invoke();
            }
            eightInFourValue = value;
        }
    }


    public event BeatEvent EmitSixteenInFourEvent;
    private float sixteenInFourValue = 1.0f;

    public float SixteenInFourValue
    {
        get => sixteenInFourValue;
        set
        {
            if (sixteenInFourValue > 0.5 && value < 0.5)
            {
                EmitSixteenInFourEvent?.Invoke();
            }
            sixteenInFourValue = value;
        }
    }

    public event BeatEvent EmitOneInEightEvent;
    private float oneInEightValue = 1.0f;

    public float OneInEightValue
    {
        get => oneInEightValue;
        set
        {
            if (oneInEightValue > 0.5 && value < 0.5)
            {
                EmitOneInEightEvent?.Invoke();
            }
            oneInEightValue = value;
        }
    }
    #endregion

    #region average volume

    private float _averageVolume;
    private float _averageVolumeRawMax = 0;
    public float AverageVolumeRaw
    {
        get => _averageVolume;
        set
        {
            _averageVolume = value;
            if (value > _averageVolumeRawMax)
            {
                _averageVolumeRawMax = value;

            }
            AverageVolumeNormalized = Mathf.Min(1f,value / _averageVolumeRawMax);
            AverageVolumeNormalizedEased = Easings.EaseInOutCubic(AverageVolumeNormalized);
            AverageVolumeNormalizedEasedSmoothed = AverageVolumeNormalizedEased;

            if (AverageVolumeNormalizedEased > AverageVolumeNormalizedEasedPeak)
            {
                AverageVolumeNormalizedEasedPeak = AverageVolumeNormalizedEased;
            }

        }
    }

    public float AverageVolumeNormalizedEased { get; private set; }


    private SmoothingRingBuffer _averageVolumeNormalizedEasedSmoothed;
    public float AverageVolumeNormalizedEasedSmoothed
    {
        get
        {
            (var min, var max) = _averageVolumeNormalizedEasedSmoothed.GetMinAndMax();
            return _averageVolumeNormalizedEasedSmoothed.GetSmoothValue() + ((max - min) / 2);
        }
        set => _averageVolumeNormalizedEasedSmoothed.Enqueue(value);
    }


    public float AverageVolumeNormalizedEasedPeak { get; private set; }

    public float AverageVolumeNormalized { get; private set; }

    #endregion


    #region low frequency volume
    private float _lowFrequencyVolume;
    private float _lowFrequencyVolumeRawMax = 0;

    public float LowFrequencyVolume
    {
        get => _lowFrequencyVolume;
        set
        {
            _lowFrequencyVolume = value;
            if (value > _lowFrequencyVolumeRawMax)
            {
                _lowFrequencyVolumeRawMax = value;

            }
            LowFrequencyVolumeNormalized = Mathf.Min(1f, value / _lowFrequencyVolumeRawMax);
            LowFrequencyVolumeNormalizedEased = Easings.EaseInOutCubic(LowFrequencyVolumeNormalized);
            LowFrequencyVolumeNormalizedEasedSmoothed = LowFrequencyVolumeNormalizedEased;

            if (LowFrequencyVolumeNormalizedEased > LowFrequencyVolumeNormalizedEasedPeak)
            {
                LowFrequencyVolumeNormalizedEasedPeak = LowFrequencyVolumeNormalizedEased;
            }

        }
    }

    public float LowFrequencyVolumeNormalized { get; private set; }

    public float LowFrequencyVolumeNormalizedEased { get; private set; }

    public float LowFrequencyVolumePeak { get; private set; }

    public float LowFrequencyVolumeNormalizedEasedPeak { get; private set; }


    private SmoothingRingBuffer _lowFrequencyVolumeNormalizedEasedSmoothed;

    public float LowFrequencyVolumeNormalizedEasedSmoothed
    {
        get
        {
            (var min, var max) = _averageVolumeNormalizedEasedSmoothed.GetMinAndMax();
            return _lowFrequencyVolumeNormalizedEasedSmoothed.GetSmoothValue() + ((max - min) / 2);
        }
        set => _lowFrequencyVolumeNormalizedEasedSmoothed.Enqueue(value);
    }

    
    #endregion



    // Start is called before the first frame update
    void Start()
    {
        _averageVolumeNormalizedEasedSmoothed = new SmoothingRingBuffer(5);
        _lowFrequencyVolumeNormalizedEasedSmoothed = new SmoothingRingBuffer(5);
        _fourInFourStopWatch = new Stopwatch();
        _fourInFourStopWatch.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (AverageVolumeNormalizedEasedPeak - 0.01f < 0)
        {
            AverageVolumeNormalizedEasedPeak = 0;
        }
        else
        {
            AverageVolumeNormalizedEasedPeak -= 0.01f;
        }


        if (_averageVolumeRawMax - 0.01f < 0)
        {
            _averageVolumeRawMax = 0.1f;
        }
        else
        {
            _averageVolumeRawMax -= 0.01f;
        }


        if (LowFrequencyVolumePeak - 0.1f < 0)
        {
            LowFrequencyVolumePeak = 0;
        }
        else
        {
            LowFrequencyVolumePeak -= 0.1f;
        }
    }
}

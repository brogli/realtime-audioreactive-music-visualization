using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicInputsModel : MonoBehaviour
{
    #region beat values
    public delegate void BeatEvent();
    public event BeatEvent EmitFourInFourEvent;
    private float fourInFourValue = 1.0f;

    public float FourInFourValue
    {
        get => fourInFourValue;
        set
        {
            if (fourInFourValue > 0.5 && value < 0.5)
            {
                EmitFourInFourEvent?.Invoke();
            }
            fourInFourValue = value;
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

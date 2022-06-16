using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicValuesModel : MonoBehaviour
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

    #region volume

    private float _averageVolume;
    private float _averageVolumeMax = 0;
    public float AverageVolume
    {
        get => _averageVolume;
        set
        {
            _averageVolume = value;
            _averageVolumeSmoothed.Enqueue(value);
            if (value > _averageVolumeMax)
            {
                _averageVolumeMax = value;

            }
            AverageVolumeNormalized = (value - 0.2f) / (_averageVolumeMax - 0.2f);
            Debug.Log($"averageMax: {_averageVolumeMax}, normalized current: {AverageVolumeNormalized}, normalized peak: {AverageVolumeNormalizedPeak}, current value: {value}");
            if (AverageVolumeNormalized > AverageVolumeNormalizedPeak)
            {
                AverageVolumeNormalizedPeak = AverageVolumeNormalized;
            }

            if (value > AverageVolumePeak)
            {
                AverageVolumePeak = value;
            }
        }
    }

    private SmoothingRingBuffer _averageVolumeSmoothed;
    public float AverageVolumeSmoothed
    {
        get => _averageVolumeSmoothed.GetSmoothValue();
    }

    public float AverageVolumePeak { get; private set; }

    public float AverageVolumeNormalizedPeak { get; private set; }

    public float AverageVolumeNormalized { get; private set; }



    private float _lowFrequencyVolume;
    public float LowFrequencyVolume
    {
        get => _lowFrequencyVolume;
        set
        {
            _lowFrequencyVolume = value;
            _lowFrequencyVolumeSmoothed.Enqueue(value);
            if (value > LowFrequencyVolumePeak)
            {
                LowFrequencyVolumePeak = value;
            }
        }
    }

    public float LowFrequencyVolumePeak { get; private set; }

    private SmoothingRingBuffer _lowFrequencyVolumeSmoothed;
    public float LowFrequencyVolumeSmoothed
    {
        get => _lowFrequencyVolumeSmoothed.GetSmoothValue();
    }
    #endregion



    // Start is called before the first frame update
    void Start()
    {
        _averageVolumeSmoothed = new SmoothingRingBuffer(5);
        _lowFrequencyVolumeSmoothed = new SmoothingRingBuffer(5);
    }

    // Update is called once per frame
    void Update()
    {
        if (AverageVolumeNormalizedPeak - 0.01f < 0)
        {
            AverageVolumeNormalizedPeak = 0;
        }
        else
        {
            AverageVolumeNormalizedPeak -= 0.01f;
        }

        

        if (AverageVolumePeak - 0.1f < 0)
        {
            AverageVolumePeak = 0;
        }
        else
        {
            AverageVolumePeak -= 0.1f;
        }

        if (_averageVolumeMax - 0.01f < 0)
        {
            _averageVolumeMax = 0.1f;
        }
        else
        {
            _averageVolumeMax -= 0.01f;
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

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
    public float AverageVolume
    {
        get => _averageVolume;
        set
        {
            _averageVolume = value;
            _averageVolumeSmoothed.Enqueue(value);
        }
    }

    private SmoothingRingBuffer _averageVolumeSmoothed;
    public float AverageVolumeSmoothed 
    {
        get => _averageVolumeSmoothed.GetSmoothValue();
    }



    private float _lowFrequencyVolume;
    public float LowFrequencyVolume
    {
        get => _lowFrequencyVolume;
        set
        {
            _lowFrequencyVolume = value;
            _lowFrequencyVolumeSmoothed.Enqueue(value);
        }
    }

    private SmoothingRingBuffer _lowFrequencyVolumeSmoothed;
    public float LowFrequencyVolumeSmoothed
    {
        get => _lowFrequencyVolumeSmoothed.GetSmoothValue();
    }
    #endregion



    // Start is called before the first frame update
    void Start()
    {
        _averageVolumeSmoothed = new SmoothingRingBuffer(2);
        _lowFrequencyVolumeSmoothed = new SmoothingRingBuffer(2);
    }

    // Update is called once per frame
    void Update()
    {

    }
}

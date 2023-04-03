using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Behaves similarily than the FadedUserInput, but it has a dead zone in the middle where it is off, and left and right of it
/// it has values and is on. E.g. 0 - 0.4 is on, 0.4 - 0.6 is off, 0.6 - 1 is on. Additionally it returns both sides of the deadzone
/// as values from 0 to -1 or 0 to 1;
/// </summary>
public class TwoSidedFadedUserInput : IUserInput
{
    public delegate void TurnedOnEvent();
    public event TurnedOnEvent EmitTurnedOnEvent;

    public delegate void TurnedOffEvent();
    public event TurnedOffEvent EmitTurnedOffEvent;

    public float FaderValue
    {
        get 
        {
            return _faderValue * 2 - 1;  
        }
        private set { _faderValue = value; }
    }
    public bool IsActive { get; private set; } = true;

    private float _faderValue;

    private float _deadZoneSize = 0.2f;
    private float _deadZoneLowerBound = 0.4f;
    private float _deadZoneUpperBound = 0.6f;

    public TwoSidedFadedUserInput(float deadZoneSize)
    {
        _deadZoneSize = deadZoneSize;
        _deadZoneLowerBound = 0.5f - deadZoneSize / 2f;
        _deadZoneUpperBound = 0.5f + deadZoneSize / 2f;
    }

    public void SetNewStateIfNecessary(bool isPressed, float value)
    {
        if (FaderValue == value)
        {
            return;
        }

        if ((FaderValue >= _deadZoneLowerBound && FaderValue <= _deadZoneUpperBound) && (value > _deadZoneUpperBound || value < _deadZoneLowerBound))
        {
            EmitTurnedOnEvent?.Invoke();
            IsActive = true;

        }
        else if ((FaderValue < _deadZoneLowerBound || FaderValue > _deadZoneUpperBound) && (value < _deadZoneUpperBound && value > _deadZoneLowerBound))
        {
            EmitTurnedOffEvent?.Invoke();
            IsActive = false;
        }

        FaderValue = value;
    }
}

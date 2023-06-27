using System.Diagnostics;

public class FadedUserInput : ISceneUserInput
{
    public delegate void TurnedOnEvent();
    public event TurnedOnEvent EmitTurnedOnEvent;

    public delegate void TurnedOffEvent();
    public event TurnedOffEvent EmitTurnedOffEvent;

    private const float _turnedOffThreshold = 0.05f;
    private float _faderValue = 1;
    private bool _isFaderValueUsed = false;

    public float FaderValue { 
        get 
        {
            _isFaderValueUsed = true;
            return _faderValue; 
        } 
        private set => _faderValue = value; 
    }
    public bool IsActive { get; private set; } = true;

    public void SetNewStateIfNecessary(bool isPressed, float value)
    {
        if (FaderValue == value)
        {
            return;
        }

        if (FaderValue < _turnedOffThreshold && value >= _turnedOffThreshold)
        {
            EmitTurnedOnEvent?.Invoke();
            IsActive = true;

        }
        else if (FaderValue >= _turnedOffThreshold && value < _turnedOffThreshold)
        {
            EmitTurnedOffEvent?.Invoke();
            IsActive = false;
        }

        FaderValue = value;
    }

    public bool IsUsed()
    {
        return _isFaderValueUsed;
    }

    public void ResetValidationFlags()
    {
        _isFaderValueUsed = false;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggeledAndFadedUserInput : ISceneUserInput
{
    public delegate void TurnedOnEvent();
    public event TurnedOnEvent EmitTurnedOnEvent;

    public delegate void TurnedOffEvent();
    public event TurnedOffEvent EmitTurnedOffEvent;

    public delegate void TurnedOnOrOffEvent(bool hasTurnedOn);
    /// <summary>
    /// returns true if turned on and false if turned off
    /// </summary>
    public event TurnedOnOrOffEvent EmitTurnedOnOrOffEvent;

    public bool IsPressed
    {
        get
        {
            _isIsPressedAccessed = true;
            return ToggeledUserInput.IsPressed;
        }
    }

    public float FaderValue
    {
        get => FadedUserInput.FaderValue;
    }
    public ToggeledUserInput ToggeledUserInput { get; }
    public FadedUserInput FadedUserInput { get; }

    private bool _isIsPressedAccessed = false;


    public ToggeledAndFadedUserInput()
    {
        ToggeledUserInput = new ToggeledUserInput();
        FadedUserInput = new FadedUserInput();
        SubscribeToChildEvents();
    }

    private void SubscribeToChildEvents()
    {
        ToggeledUserInput.EmitTurnedOnOrOffEvent += HandleTurnOnOrOffEventFromToggle;

        FadedUserInput.EmitTurnedOffEvent += HandleTurnedOffEventFromFader;
        FadedUserInput.EmitTurnedOnEvent += HandleTurnOnEventFromFader;
    }

    private void HandleTurnOnOrOffEventFromToggle(bool hasTurnedOn, int _)
    {
        if (hasTurnedOn)
        {
            if (FadedUserInput.IsActive)
            {
                EmitTurnedOnEvent?.Invoke();
                EmitTurnedOnOrOffEvent?.Invoke(true);
            }
        }
        else
        {
            EmitTurnedOffEvent?.Invoke();
            EmitTurnedOnOrOffEvent?.Invoke(false);
        }
    }

    private void HandleTurnedOffEventFromFader()
    {
        EmitTurnedOffEvent?.Invoke();
        EmitTurnedOnOrOffEvent?.Invoke(false);
    }

    private void HandleTurnOnEventFromFader()
    {
        if (IsPressed)
        {
            EmitTurnedOnEvent?.Invoke();
            EmitTurnedOnOrOffEvent?.Invoke(true);
        }
    }

    public void SetNewStateIfNecessary(bool newIsActive, float value)
    {
        Debug.LogWarning("ToggeledAndFadedInput objects must be updated through their childs");
    }

    public void Unsubscribe()
    {
        ToggeledUserInput.EmitTurnedOnOrOffEvent -= HandleTurnOnOrOffEventFromToggle;

        FadedUserInput.EmitTurnedOffEvent -= HandleTurnedOffEventFromFader;
        FadedUserInput.EmitTurnedOnEvent -= HandleTurnOnEventFromFader;
    }

    public bool IsUsed()
    {
        bool isToggleUsed = _isIsPressedAccessed || (EmitTurnedOnEvent != null && EmitTurnedOffEvent != null) || EmitTurnedOnOrOffEvent != null;
        bool isFaderUsed = FadedUserInput.IsUsed();
        if (!isFaderUsed) { Debug.LogWarning("Fader isn't used"); }
        if (!isToggleUsed) { Debug.LogWarning("Toggle isn't used"); }
        return isToggleUsed && isFaderUsed;
    }

    public void ResetValidationFlags()
    {
        _isIsPressedAccessed = false;
        ToggeledUserInput.ResetValidationFlags();
        FadedUserInput.ResetValidationFlags();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggeledAndFadedUserInput : IUserInput
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
        get => ToggeledUserInput.IsPressed;
    }

    public float FaderValue
    {
        get => FadedUserInput.FaderValue;
    }
    public ToggeledUserInput ToggeledUserInput { get; }
    public FadedUserInput FadedUserInput { get; }


    public ToggeledAndFadedUserInput()
    {
        ToggeledUserInput = new ToggeledUserInput();
        FadedUserInput = new FadedUserInput();
        SubscribeToChildEvents();
    }

    private void SubscribeToChildEvents()
    {
        ToggeledUserInput.EmitTurnedOffEvent += HandleTurnedOffEventFromToggle; 
        ToggeledUserInput.EmitTurnedOnEvent += HandleTurnOnEventFromToggle;
        
        FadedUserInput.EmitTurnedOffEvent += HandleTurnedOffEventFromFader;
        FadedUserInput.EmitTurnedOnEvent += HandleTurnOnEventFromFader;
    }

    private void HandleTurnedOffEventFromToggle()
    {
        EmitTurnedOffEvent?.Invoke();
        EmitTurnedOnOrOffEvent?.Invoke(false);
    }

    private void HandleTurnOnEventFromToggle()
    {
        if (FadedUserInput.IsActive)
        {
            EmitTurnedOnEvent?.Invoke();
            EmitTurnedOnOrOffEvent?.Invoke(true);
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
        ToggeledUserInput.EmitTurnedOffEvent -= HandleTurnedOffEventFromToggle;
        ToggeledUserInput.EmitTurnedOnEvent -= HandleTurnOnEventFromToggle;

        FadedUserInput.EmitTurnedOffEvent -= HandleTurnedOffEventFromFader;
        FadedUserInput.EmitTurnedOnEvent -= HandleTurnOnEventFromFader;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggeledAndFadedUserInput : IUserInput
{
    public delegate void TurnedOnEvent();
    public event TurnedOnEvent EmitTurnedOnEvent;

    public delegate void TurnedOffEvent();
    public event TurnedOffEvent EmitTurnedOffEvent;

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
        ToggeledUserInput.EmitTurnedOffEvent += () => EmitTurnedOffEvent?.Invoke();
        ToggeledUserInput.EmitTurnedOnEvent += HandleTurnOnEventFromToggle;

        FadedUserInput.EmitTurnedOffEvent += () => EmitTurnedOffEvent?.Invoke();
        FadedUserInput.EmitTurnedOnEvent += HandleTurnOnEventFromFader;
    }

    private void HandleTurnOnEventFromToggle()
    {
        if (FadedUserInput.IsActive)
        {
            EmitTurnedOnEvent?.Invoke();
        }
    }

    private void HandleTurnOnEventFromFader()
    {
        if (IsPressed)
        {
            EmitTurnedOnEvent?.Invoke();
        }
    }

    public void SetNewStateIfNecessary(bool newIsActive, float value)
    {
        Debug.LogWarning("ToggeledAndFadedInput objects must be updated through their childs");
    }

    public void Unsubscribe()
    {
        ToggeledUserInput.EmitTurnedOffEvent -= () => EmitTurnedOffEvent?.Invoke();
        ToggeledUserInput.EmitTurnedOnEvent -= () => EmitTurnedOnEvent?.Invoke();

        FadedUserInput.EmitTurnedOffEvent -= () => EmitTurnedOffEvent?.Invoke();
        FadedUserInput.EmitTurnedOnEvent -= HandleTurnOnEventFromFader;
    }
}

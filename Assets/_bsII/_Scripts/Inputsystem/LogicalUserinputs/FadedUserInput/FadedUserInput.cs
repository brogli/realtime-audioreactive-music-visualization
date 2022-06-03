public class FadedUserInput : IUserInput
{
    public delegate void TurnedOnEvent();
    public event TurnedOnEvent EmitTurnedOnEvent;

    public delegate void TurnedOffEvent();
    public event TurnedOffEvent EmitTurnedOffEvent;

    private const float TurnedOffThreshold = 0.05f;

    public float FaderValue { get; private set; } = 1;
    public bool IsActive { get; private set; } = true;

    public void SetNewStateIfNecessary(bool isPressed, float value)
    {
        if (FaderValue == value)
        {
            return;
        }

        if (FaderValue < TurnedOffThreshold && value >= TurnedOffThreshold )
        {
            EmitTurnedOnEvent?.Invoke();
            IsActive = true;

        }
        else if (FaderValue >= TurnedOffThreshold && value < TurnedOffThreshold)
        {
            EmitTurnedOffEvent?.Invoke();
            IsActive = false;
        }

        FaderValue = value;
    }
}
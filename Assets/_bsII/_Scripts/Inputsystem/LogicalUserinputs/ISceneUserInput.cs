
public interface ISceneUserInput : IUserInput
{
    /// <summary>
    /// Part of scene validation. After validating, some flags need to be reset in a user input until the next validation.
    /// </summary>
    void ResetValidationFlags();

    /// <summary>
    /// Used for validation of a scene, to check if a user input is used in the scene.
    /// </summary>
    /// <returns></returns>
    bool IsUsedInScene();
}

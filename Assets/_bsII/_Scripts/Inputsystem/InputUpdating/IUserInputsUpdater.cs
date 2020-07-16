using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUserInputsUpdater
{
    #region volume related user inputs
    void UpdateVolume();
    #endregion

    #region beat related user inputs
    void UpdateFourInFour();
    void UpdateTwoInFour();
    void UpdateOneInFour();
    void UpdateEightInFour();
    void UpdateSixteenInFour();
    #endregion

    #region piano keys related user inputs
    void UpdateDroneKeys();
    void UpdateMelodyKeys();
    void UpdateMoodKeys();
    #endregion

    #region other user inputs
    void UpdateExplosionTriggers();
    void UpdateGlobalIntensity();
    #endregion

    #region visual effects
    void UpdateVisualEffects();
    #endregion

    void UpdateUserInputs();
}

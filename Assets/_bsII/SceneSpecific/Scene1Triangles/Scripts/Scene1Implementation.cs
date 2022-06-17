using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene1Implementation : MonoBehaviour, IUserInputsConsumer
{
    private UserInputsModel _userInputsModel;
    private MusicValuesModel _musicValuesModel;

    public List<GameObject> TwoInFourCores;
    public List<GameObject> FourInFourCores;
    public List<GameObject> EightInFourCores;
    public List<GameObject> OneInEightCores;

    public List<Light> VolumeLights;
    public List<Light> LowFrequencyVolumeLights;

    public float VolumeLightBrightnessValue;

    // Start is called before the first frame update
    void Start()
    {
        _userInputsModel = GameObject.FindGameObjectWithTag("UserInputsModel").GetComponent<UserInputsModel>();
        _musicValuesModel = GameObject.FindGameObjectWithTag("MusicValuesModel").GetComponent<MusicValuesModel>();
        SubscribeUserInputs();
    }

    public void SubscribeUserInputs()
    {
        // 4-4 
        _userInputsModel.FourInFourUserInput.EmitTurnedOnOrOffEvent += ToggleFourInFour;
        FourInFourCores.ForEach((core) => core.SetActive(_userInputsModel.FourInFourUserInput.IsPressed));

        // 8-4
        _userInputsModel.EightInFourUserInput.EmitTurnedOnOrOffEvent += ToggleEightInFour;
        EightInFourCores.ForEach((core) => core.SetActive(_userInputsModel.EightInFourUserInput.IsPressed));

        // 2-4
        _userInputsModel.TwoInFourUserInput.EmitTurnedOnOrOffEvent += ToggleTwoInFour;
        TwoInFourCores.ForEach((core) => core.SetActive(_userInputsModel.TwoInFourUserInput.IsPressed));

        // volume
        _userInputsModel.AverageVolume.EmitTurnedOnOrOffEvent += ToggleVolume;
        VolumeLights.ForEach((light) => light.gameObject.SetActive(_userInputsModel.AverageVolume.IsPressed));
        

        // low frequency volume
        _userInputsModel.LowFrequencyVolume.EmitTurnedOnOrOffEvent += ToggleLowFrequencyVolume;
        LowFrequencyVolumeLights.ForEach((light) => light.gameObject.SetActive(_userInputsModel.LowFrequencyVolume.IsPressed));


    }

    private void ToggleFourInFour(bool isNowActive)
    {
        FourInFourCores.ForEach((core) => core.SetActive(isNowActive));
    }

    private void ToggleOneInFour(bool isNowActive)
    {
        // todo
    }

    private void ToggleTwoInFour(bool isNowActive)
    {
        TwoInFourCores.ForEach((core) => core.SetActive(true));
    }

    private void ToggleEightInFour(bool isNowActive)
    {
        EightInFourCores.ForEach((core) => core.SetActive(true));
    }

    private void ToggleSixteenInFour(bool isNowActive)
    {
        // todo
    }

    private void ToggleOneInEight(bool isNowActive)
    {
        // todo
    }

    public void OnDisable()
    {
        UnsubscribeUserInputs();
    }

    public void UnsubscribeUserInputs()
    {
        // 4-4 
        _userInputsModel.FourInFourUserInput.EmitTurnedOnOrOffEvent -= ToggleFourInFour;

        // 8-4
        _userInputsModel.EightInFourUserInput.EmitTurnedOnOrOffEvent -= ToggleEightInFour;

        // 2-4
        _userInputsModel.TwoInFourUserInput.EmitTurnedOnOrOffEvent -= ToggleTwoInFour;

        // Volume
        _userInputsModel.AverageVolume.EmitTurnedOnOrOffEvent -= ToggleVolume;


        // low freq volume
        _userInputsModel.LowFrequencyVolume.EmitTurnedOnOrOffEvent -= ToggleLowFrequencyVolume;
    }

    private void ToggleVolume(bool hasTurnedActive)
    {
        VolumeLights.ForEach((light) => light.gameObject.SetActive(hasTurnedActive));
    }


    private void ToggleLowFrequencyVolume(bool hasTurnedActive)
    {
        LowFrequencyVolumeLights.ForEach((light) => light.gameObject.SetActive(hasTurnedActive));
    }


    /// <summary>
    /// Ease-in and ease-out
    /// Sauce: https://stackoverflow.com/a/25730573
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    private float ParametricBlend(float t)
    {
        float sqt = t * t;
        return sqt / (2.0f * (sqt - t) + 1.0f);
    }




    // Update is called once per frame
    void Update()
    {
        float fourInFourValueInverted = ParametricBlend(1.0f - _musicValuesModel.FourInFourValue);
        FourInFourCores.ForEach(
            (core) =>
            {
                
                core.GetComponent<Renderer>().material.SetColor("_EmissiveColor", Color.white * ( fourInFourValueInverted) * 4);

            });

        float EightInFourValueInverted = ParametricBlend(1.0f - _musicValuesModel.EightInFourValue);
        EightInFourCores.ForEach(
            (core) =>
            {

                core.GetComponent<Renderer>().material.SetColor("_EmissiveColor", Color.white * (EightInFourValueInverted) * 4);

            });

        float TwoInFourValueInverted = ParametricBlend(1.0f - _musicValuesModel.TwoInFourValue);
        TwoInFourCores.ForEach(
            (core) =>
            {

                core.GetComponent<Renderer>().material.SetColor("_EmissiveColor", Color.white * (TwoInFourValueInverted) * 4);

            });

        float OneInEightValueInverted = ParametricBlend(1.0f - _musicValuesModel.OneInEightValue);
        OneInEightCores.ForEach(
            (core) =>
            {

                core.GetComponent<Renderer>().material.SetColor("_EmissiveColor", Color.white * (OneInEightValueInverted) * 4);

            });

        VolumeLights.ForEach(light => light.intensity = _musicValuesModel.AverageVolumeNormalizedPeak * VolumeLightBrightnessValue);
        LowFrequencyVolumeLights.ForEach(light => light.intensity = _musicValuesModel.LowFrequencyVolumePeak * VolumeLightBrightnessValue);
    }
}

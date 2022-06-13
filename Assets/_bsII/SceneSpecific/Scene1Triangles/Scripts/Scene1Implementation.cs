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
        _userInputsModel.FourInFourUserInput.EmitTurnedOnEvent += () =>
        {
            FourInFourCores.ForEach((core) => core.SetActive(true));
        };
        _userInputsModel.FourInFourUserInput.EmitTurnedOffEvent += () =>
        {
            FourInFourCores.ForEach((core) => core.SetActive(false));
        };
        FourInFourCores.ForEach((core) => core.SetActive(_userInputsModel.FourInFourUserInput.IsPressed));

        // 8-4
        _userInputsModel.EightInFourUserInput.EmitTurnedOnEvent += () =>
        {
            EightInFourCores.ForEach((core) => core.SetActive(true));
        };
        _userInputsModel.EightInFourUserInput.EmitTurnedOffEvent += () =>
        {
            EightInFourCores.ForEach((core) => core.SetActive(false));
        };
        EightInFourCores.ForEach((core) => core.SetActive(_userInputsModel.EightInFourUserInput.IsPressed));

        // 2-4
        _userInputsModel.TwoInFourUserInput.EmitTurnedOnEvent += () =>
        {
            TwoInFourCores.ForEach((core) => core.SetActive(true));
        };
        _userInputsModel.TwoInFourUserInput.EmitTurnedOffEvent += () =>
        {
            TwoInFourCores.ForEach((core) => core.SetActive(false));
        };
        TwoInFourCores.ForEach((core) => core.SetActive(_userInputsModel.TwoInFourUserInput.IsPressed));

        // volume
        _userInputsModel.AverageVolume.EmitTurnedOnOrOffEvent += SetVolumeActive;
        VolumeLights.ForEach((light) => light.gameObject.SetActive(_userInputsModel.AverageVolume.IsPressed));
        

        // low frequency volume
        _userInputsModel.LowFrequencyVolume.EmitTurnedOnOrOffEvent += SetLowFrequencyVolumeActive;
        LowFrequencyVolumeLights.ForEach((light) => light.gameObject.SetActive(_userInputsModel.LowFrequencyVolume.IsPressed));


    }

    public void OnDisable()
    {
        UnsubscribeUserInputs();
    }

    public void UnsubscribeUserInputs()
    {
        // 4-4
        _userInputsModel.FourInFourUserInput.EmitTurnedOnEvent -= () =>
        {
            FourInFourCores.ForEach((core) => core.SetActive(true));
        };
        _userInputsModel.FourInFourUserInput.EmitTurnedOffEvent -= () =>
        {
            FourInFourCores.ForEach((core) => core.SetActive(false));
        };

        // 8-4
        _userInputsModel.EightInFourUserInput.EmitTurnedOnEvent -= () =>
        {
            EightInFourCores.ForEach((core) => core.SetActive(true));
        };
        _userInputsModel.EightInFourUserInput.EmitTurnedOffEvent -= () =>
        {
            EightInFourCores.ForEach((core) => core.SetActive(false));
        };

        // 2-4
        _userInputsModel.TwoInFourUserInput.EmitTurnedOnEvent -= () =>
        {
            TwoInFourCores.ForEach((core) => core.SetActive(true));
        };
        _userInputsModel.TwoInFourUserInput.EmitTurnedOffEvent += () =>
        {
            TwoInFourCores.ForEach((core) => core.SetActive(false));
        };

        // Volume
        _userInputsModel.AverageVolume.EmitTurnedOnOrOffEvent -= SetVolumeActive;


        // low freq volume
        _userInputsModel.LowFrequencyVolume.EmitTurnedOnOrOffEvent -= SetLowFrequencyVolumeActive;
    }

    private void SetVolumeActive(bool hasTurnedActive)
    {
        VolumeLights.ForEach((light) => light.gameObject.SetActive(hasTurnedActive));
    }


    private void SetLowFrequencyVolumeActive(bool hasTurnedActive)
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

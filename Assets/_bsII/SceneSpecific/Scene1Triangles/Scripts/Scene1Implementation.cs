using System;
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

    public List<GameObject> MelodyKey0Elements;
    public List<GameObject> MelodyKey1Elements;
    public List<GameObject> MelodyKey2Elements;
    public List<GameObject> MelodyKey3Elements;
    public List<GameObject> MelodyKey4Elements;
    public List<GameObject> MelodyKey5Elements;
    public List<GameObject> MelodyKey6Elements;
    public List<GameObject> MelodyKey7Elements;

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

        // melodykeys
        for (int i = 0; i < _userInputsModel.MelodyKeys.Keys.Length; i++)
        {
            _userInputsModel.MelodyKeys.Keys[i].EmitTurnedOnOrOffEvent += ToggleMelodyKey;
            ToggleMelodyKey(_userInputsModel.MelodyKeys.Keys[i].IsPressed, i);
        }
    }

    private void ToggleMelodyKey(bool hasTurnedOn, int index)
    {
        switch (index)
        {
            case 0:
                MelodyKey0Elements.ForEach(element => element.SetActive(hasTurnedOn));
                break;
            case 1:
                MelodyKey1Elements.ForEach(element => element.SetActive(hasTurnedOn));
                break;
            case 2:
                MelodyKey2Elements.ForEach(element => element.SetActive(hasTurnedOn));
                break;
            case 3:
                MelodyKey3Elements.ForEach(element => element.SetActive(hasTurnedOn));
                break;
            case 4:
                MelodyKey4Elements.ForEach(element => element.SetActive(hasTurnedOn));
                break;
            case 5:
                MelodyKey5Elements.ForEach(element => element.SetActive(hasTurnedOn));
                break;
            case 6:
                MelodyKey6Elements.ForEach(element => element.SetActive(hasTurnedOn));
                break;
            case 7:
                MelodyKey7Elements.ForEach(element => element.SetActive(hasTurnedOn));
                break;
            default:
                break;
        }
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
        TwoInFourCores.ForEach((core) => core.SetActive(isNowActive));
    }

    private void ToggleEightInFour(bool isNowActive)
    {
        EightInFourCores.ForEach((core) => core.SetActive(isNowActive));
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

        // melodykeys
        foreach (var key in _userInputsModel.MelodyKeys.Keys)
        {
            key.EmitTurnedOnOrOffEvent -= ToggleMelodyKey;
        }
    }

    private void ToggleVolume(bool hasTurnedActive)
    {
        VolumeLights.ForEach((light) => light.gameObject.SetActive(hasTurnedActive));
    }


    private void ToggleLowFrequencyVolume(bool hasTurnedActive)
    {
        LowFrequencyVolumeLights.ForEach((light) => light.gameObject.SetActive(hasTurnedActive));
    }



    // Update is called once per frame
    void Update()
    {
        float fourInFourValueInverted = Easings.EaseInOutCubic(1.0f - _musicValuesModel.FourInFourValue);
        FourInFourCores.ForEach(
            (core) =>
            {

                core.GetComponent<Renderer>().material.SetColor("_EmissiveColor", Color.white * (fourInFourValueInverted) * 4);

            });

        float EightInFourValueInverted = Easings.EaseInOutCubic(1.0f - _musicValuesModel.EightInFourValue);
        EightInFourCores.ForEach(
            (core) =>
            {

                core.GetComponent<Renderer>().material.SetColor("_EmissiveColor", Color.white * (EightInFourValueInverted) * 4);

            });

        float TwoInFourValueInverted = Easings.EaseInOutCubic(1.0f - _musicValuesModel.TwoInFourValue);
        TwoInFourCores.ForEach(
            (core) =>
            {

                core.GetComponent<Renderer>().material.SetColor("_EmissiveColor", Color.white * (TwoInFourValueInverted) * 4);

            });

        float OneInEightValueInverted = Easings.EaseInOutCubic(1.0f - _musicValuesModel.OneInEightValue);
        OneInEightCores.ForEach(
            (core) =>
            {

                core.GetComponent<Renderer>().material.SetColor("_EmissiveColor", Color.white * (OneInEightValueInverted) * 4);

            });

        VolumeLights.ForEach(light => light.intensity = _musicValuesModel.AverageVolumeNormalizedEasedSmoothed * VolumeLightBrightnessValue);
        LowFrequencyVolumeLights.ForEach(light => light.intensity = _musicValuesModel.LowFrequencyVolumeNormalizedEasedSmoothed * VolumeLightBrightnessValue);
    }
}

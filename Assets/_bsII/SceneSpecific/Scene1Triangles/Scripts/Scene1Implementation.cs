using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class Scene1Implementation : MonoBehaviour, IUserInputsConsumer
{
    private UserInputsModel _userInputsModel;
    private MusicInputsModel _musicInputsModel;

    public List<GameObject> TwoInFourObjects;
    public List<GameObject> FourInFourCores;
    public List<GameObject> EightInFourCores;

    public Transform OneInFourSpawnPosition;
    public GameObject OneInFourPrefab;

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

    public Volume MainCameraVolume;

    public float VolumeLightBrightnessValue;
    public GameObject explosionType2Prefab;

    private Scene1DroneKeyImplementation _scene1DroneKeyImplementation;
    private Scene1SixteenInFourImplementation _scene1OneInSixteenImplementation;
    private bool _isOneInFourActive;
    private bool _isOneInEightActive;
    private Scene1ColorSwitcher _scene1ColorSwitcher;
    private int _currentMoodIndex = 0;
    private Scene1ExplosionFullScreenShaderGraph scene1ExplosionFullScreenShaderGraph;
    private bool isExplosionFullScreenEffectStopped = false;

    void Start()
    {
        _userInputsModel = GameObject.FindGameObjectWithTag("UserInputsModel").GetComponent<UserInputsModel>();
        _musicInputsModel = GameObject.FindGameObjectWithTag("MusicInputsModel").GetComponent<MusicInputsModel>();
        _scene1DroneKeyImplementation = this.GetComponent<Scene1DroneKeyImplementation>();
        _scene1OneInSixteenImplementation = this.GetComponent<Scene1SixteenInFourImplementation>();
        _scene1ColorSwitcher = this.GetComponent<Scene1ColorSwitcher>();

        if (!MainCameraVolume.sharedProfile.TryGet<Scene1ExplosionFullScreenShaderGraph>(out scene1ExplosionFullScreenShaderGraph))
        {
            throw new NullReferenceException(nameof(scene1ExplosionFullScreenShaderGraph));
        }


        SubscribeUserInputs();
        SubscribeMusicInputs();
    }

    private void SubscribeMusicInputs()
    {
        _musicInputsModel.EmitOneInFourEvent += TriggerOneInFourEffect;
        _musicInputsModel.EmitOneInEightEvent += TriggerOneInEightEffect;
        _musicInputsModel.EmitEightInFourEvent += TriggerEightInFourEffect;
    }

    private void TriggerEightInFourEffect()
    {
        scene1ExplosionFullScreenShaderGraph.isInvertActive.value = !scene1ExplosionFullScreenShaderGraph.isInvertActive.value;
        if (isExplosionFullScreenEffectStopped)
        {
            scene1ExplosionFullScreenShaderGraph.scale.value = 0;
            isExplosionFullScreenEffectStopped = false;
        }
    }

    private void TriggerOneInFourEffect()
    {
        if (_isOneInFourActive)
        {
            Instantiate(OneInFourPrefab, OneInFourSpawnPosition);
        }
    }

    private void TriggerOneInEightEffect()
    {
        if (_isOneInEightActive)
        {
            _currentMoodIndex = (_currentMoodIndex + 1) % 8;
            TriggerMoodKey(_currentMoodIndex);
        }
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
        TwoInFourObjects.ForEach((obj) => obj.SetActive(_userInputsModel.TwoInFourUserInput.IsPressed));

        // 1-4
        _userInputsModel.OneInFourUserInput.EmitTurnedOnOrOffEvent += ToggleOneInFour;
        _isOneInFourActive = _userInputsModel.OneInFourUserInput.IsPressed;

        // 1-16
        _userInputsModel.SixteenInFourUserInput.EmitTurnedOnOrOffEvent += ToggleSixteenInFour;
        ToggleSixteenInFour(_userInputsModel.SixteenInFourUserInput.IsPressed);

        // 1-8
        _userInputsModel.OneInEightUserInput.EmitTurnedOnOrOffEvent += ToggleOneInEight;
        ToggleOneInEight(_userInputsModel.SixteenInFourUserInput.IsPressed);

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

        // droneKeys
        for (int i = 0; i < _userInputsModel.DroneKeys.Keys.Length; i++)
        {
            _userInputsModel.DroneKeys.Keys[i].EmitTurnedOnOrOffEvent += ToggleDroneKey;
        }

        // moodKeys
        for (int i = 0; i < _userInputsModel.MoodKeys.Keys.Length; i++)
        {
            _userInputsModel.MoodKeys.Keys[i].EmitCollectionKeyTriggeredEvent += TriggerMoodKey;
        }

        // explosionKeys
        for (int i = 0; i < _userInputsModel.MoodKeys.Keys.Length / 2; i++)
        {
            _userInputsModel.ExplosionKeys.Keys[i].EmitCollectionKeyTriggeredEvent += TriggerExplosionType0;
        }

        for (int i = _userInputsModel.MoodKeys.Keys.Length / 2; i < _userInputsModel.MoodKeys.Keys.Length; i++)
        {
            _userInputsModel.ExplosionKeys.Keys[i].EmitCollectionKeyTriggeredEvent += TriggerExplosionType1;
        }
    }
    private void TriggerExplosionType0(int index)
    {
        scene1ExplosionFullScreenShaderGraph.scale.value = 1;
        StartCoroutine(AnimateExplosionFullScreenEffect());
    }

    private IEnumerator AnimateExplosionFullScreenEffect()
    {
        var factor = 0.2f;
        while (scene1ExplosionFullScreenShaderGraph.scale.value > 0.1f)
        {
            if (scene1ExplosionFullScreenShaderGraph.scale.value - factor * Time.deltaTime < 0.1f)
            {
                scene1ExplosionFullScreenShaderGraph.scale.value = 0.1f;
                isExplosionFullScreenEffectStopped = true;
            }
            else
            {
                scene1ExplosionFullScreenShaderGraph.scale.value -= factor * Time.deltaTime;
            }

            yield return null;
        }
    }

    private void TriggerExplosionType1(int index)
    {
        float randomX = UnityEngine.Random.Range(-16, 16);
        float randomY = UnityEngine.Random.Range(-8, 8);
        Instantiate(explosionType2Prefab, new Vector3(randomX, randomY, 0.5f), Quaternion.identity);
    }


    private void TriggerMoodKey(int index)
    {
        _scene1ColorSwitcher.SwitchColors(index);
    }

    private void ToggleDroneKey(bool hasTurnedOn, int index)
    {
        _scene1DroneKeyImplementation.ToggleDroneKey(hasTurnedOn, index);
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
        _isOneInFourActive = !_isOneInFourActive;
    }

    private void ToggleTwoInFour(bool isNowActive)
    {
        TwoInFourObjects.ForEach((obj) => obj.SetActive(isNowActive));
    }

    private void ToggleEightInFour(bool isNowActive)
    {
        EightInFourCores.ForEach((core) => core.SetActive(isNowActive));
    }

    private void ToggleSixteenInFour(bool isNowActive)
    {
        _scene1OneInSixteenImplementation.ToggleSixteenInFour(isNowActive);
    }

    private void ToggleOneInEight(bool isNowActive)
    {
        _isOneInEightActive = isNowActive;
    }

    public void OnDisable()
    {
        scene1ExplosionFullScreenShaderGraph.scale.value = 0;
        UnsubscribeUserInputs();
        UnsubscribeMusicInputs();
    }

    private void UnsubscribeMusicInputs()
    {
        _musicInputsModel.EmitOneInFourEvent -= TriggerOneInFourEffect;
        _musicInputsModel.EmitOneInEightEvent -= TriggerOneInEightEffect;
        _musicInputsModel.EmitEightInFourEvent -= TriggerEightInFourEffect;
    }

    public void UnsubscribeUserInputs()
    {
        // 4-4 
        _userInputsModel.FourInFourUserInput.EmitTurnedOnOrOffEvent -= ToggleFourInFour;

        // 8-4
        _userInputsModel.EightInFourUserInput.EmitTurnedOnOrOffEvent -= ToggleEightInFour;

        // 2-4
        _userInputsModel.TwoInFourUserInput.EmitTurnedOnOrOffEvent -= ToggleTwoInFour;

        // 1-4
        _userInputsModel.OneInFourUserInput.EmitTurnedOnOrOffEvent -= ToggleOneInFour;

        // Volume
        _userInputsModel.AverageVolume.EmitTurnedOnOrOffEvent -= ToggleVolume;

        // 1-16
        _userInputsModel.SixteenInFourUserInput.EmitTurnedOnOrOffEvent -= ToggleSixteenInFour;

        // 1-8
        _userInputsModel.OneInEightUserInput.EmitTurnedOnOrOffEvent -= ToggleOneInEight;

        // low freq volume
        _userInputsModel.LowFrequencyVolume.EmitTurnedOnOrOffEvent -= ToggleLowFrequencyVolume;

        // melodykeys
        foreach (var key in _userInputsModel.MelodyKeys.Keys)
        {
            key.EmitTurnedOnOrOffEvent -= ToggleMelodyKey;
        }

        // droneKeys
        for (int i = 0; i < _userInputsModel.DroneKeys.Keys.Length; i++)
        {
            Debug.Log("unsubscribing drone keys");
            _userInputsModel.DroneKeys.Keys[i].EmitTurnedOnOrOffEvent -= ToggleDroneKey;
        }

        // moodKeys
        for (int i = 0; i < _userInputsModel.MoodKeys.Keys.Length; i++)
        {
            _userInputsModel.MoodKeys.Keys[i].EmitCollectionKeyTriggeredEvent -= TriggerMoodKey;
        }

        // explosionKeys
        for (int i = 0; i < _userInputsModel.MoodKeys.Keys.Length / 2; i++)
        {
            _userInputsModel.ExplosionKeys.Keys[i].EmitCollectionKeyTriggeredEvent -= TriggerExplosionType0;
        }

        for (int i = _userInputsModel.MoodKeys.Keys.Length / 2; i < _userInputsModel.MoodKeys.Keys.Length; i++)
        {
            _userInputsModel.ExplosionKeys.Keys[i].EmitCollectionKeyTriggeredEvent -= TriggerExplosionType1;
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
        float fourInFourValueInverted = 1.0f - Easings.EaseInQuad(_musicInputsModel.FourInFourValue);
        float faderValueFourinFour = _userInputsModel.FourInFourUserInput.FaderValue;
        FourInFourCores.ForEach(
            (core) =>
            {
                core.GetComponent<Renderer>().material.SetColor("_EmissiveColor", _scene1ColorSwitcher.FourInFourColor * fourInFourValueInverted * faderValueFourinFour);
                core.GetComponent<Renderer>().material.SetColor("_BaseColor", new Color(0, 0, 0, fourInFourValueInverted * faderValueFourinFour));
            });

        float EightInFourValueInverted = 1.0f - (_musicInputsModel.EightInFourValue);
        float faderValueEightInFour = _userInputsModel.EightInFourUserInput.FaderValue;
        EightInFourCores.ForEach(
            (core) =>
            {
                core.GetComponent<Renderer>().material.SetColor("_EmissiveColor", _scene1ColorSwitcher.EightInFourColor * EightInFourValueInverted * faderValueEightInFour);
                core.GetComponent<Renderer>().material.SetColor("_BaseColor", new Color(0, 0, 0, EightInFourValueInverted * faderValueEightInFour));
            });

        float TwoInFourValueInverted = 1.0f - Easings.EaseInQuad(_musicInputsModel.TwoInFourValue);
        float faderValueTwoInFour = _userInputsModel.TwoInFourUserInput.FaderValue;
        TwoInFourObjects.ForEach(
            (obj) =>
            {
                obj.GetComponent<Renderer>().sharedMaterial.SetColor("_EmissiveColor", _scene1ColorSwitcher.TwoInFourColor * TwoInFourValueInverted * faderValueTwoInFour);
                obj.GetComponent<Renderer>().sharedMaterial.SetColor("_BaseColor", new Color(0, 0, 0, TwoInFourValueInverted * faderValueTwoInFour));
            });

        float volumeFaderValue = _userInputsModel.AverageVolume.FaderValue;
        VolumeLights.ForEach(light => light.intensity = _musicInputsModel.AverageVolumeNormalizedEasedSmoothed * VolumeLightBrightnessValue * volumeFaderValue);

        float lowFreqFaderValue = _userInputsModel.LowFrequencyVolume.FaderValue;
        LowFrequencyVolumeLights.ForEach(light => light.intensity = _musicInputsModel.LowFrequencyVolumeNormalizedEasedSmoothed * VolumeLightBrightnessValue * lowFreqFaderValue);
    }
}

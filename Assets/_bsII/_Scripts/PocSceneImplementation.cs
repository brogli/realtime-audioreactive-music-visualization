using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PocSceneImplementation : MonoBehaviour, IUserInputsConsumer
{
    public List<GameObject> cubes;
    public List<GameObject> MelodyKeys;
    public List<GameObject> DroneKeys;
    public List<GameObject> MoodKeys;
    public List<GameObject> ExplosionKeys;
    public GameObject VolumeCube;
    public GameObject VolumeCubeNormalized;
    public GameObject VolumeCubeNormalizedSmoothed;
    public GameObject LowFrequencyVolumeCube;

    private MusicValuesModel _musicValuesModel;
    private UserInputsModel _userInputsModel;

    // Start is called before the first frame update
    void Start()
    {
        _musicValuesModel = GameObject.FindGameObjectWithTag("MusicValuesModel").GetComponent<MusicValuesModel>();
        _userInputsModel = GameObject.FindGameObjectWithTag("UserInputsModel").GetComponent<UserInputsModel>();
        SubscribeUserInputs();
    }

    // Update is called once per frame
    void Update()
    {
        BeatElements();
        MelodyKeysImplementation();
        DroneKeysImplementation();
        VolumeImplementation();
    }

    private void VolumeImplementation()
    {
        VolumeCube.transform.localScale = new Vector3(VolumeCube.transform.localScale.x, _musicValuesModel.AverageVolumeRaw, VolumeCube.transform.localScale.z);
        VolumeCubeNormalized.transform.localScale = new Vector3(VolumeCubeNormalized.transform.localScale.x, _musicValuesModel.AverageVolumeNormalizedEased, VolumeCubeNormalized.transform.localScale.z);
        VolumeCubeNormalizedSmoothed.transform.localScale = new Vector3(VolumeCubeNormalizedSmoothed.transform.localScale.x, _musicValuesModel.AverageVolumeNormalizedEasedSmoothed, VolumeCubeNormalizedSmoothed.transform.localScale.z);

        LowFrequencyVolumeCube.transform.localScale = new Vector3(LowFrequencyVolumeCube.transform.localScale.x, _musicValuesModel.LowFrequencyVolumePeak, LowFrequencyVolumeCube.transform.localScale.z);

        Color currentColor = VolumeCube.GetComponent<Renderer>().material.color;
        VolumeCube.GetComponent<Renderer>().material.color = new Color(currentColor.r, currentColor.g, currentColor.b, _userInputsModel.AverageVolume.FaderValue);

        currentColor = LowFrequencyVolumeCube.GetComponent<Renderer>().material.color;
        LowFrequencyVolumeCube.GetComponent<Renderer>().material.color = new Color(currentColor.r, currentColor.g, currentColor.b, _userInputsModel.LowFrequencyVolume.FaderValue);

    }

    private void RegisterBeatRelatedUserInputs()
    {
        _userInputsModel.FourInFourUserInput.EmitTurnedOnOrOffEvent += ToggleFourInFour;
        ToggleFourInFour(_userInputsModel.FourInFourUserInput.IsPressed);

        _userInputsModel.OneInFourUserInput.EmitTurnedOnOrOffEvent += ToggleOneInFour;
        ToggleFourInFour(_userInputsModel.OneInFourUserInput.IsPressed);

        _userInputsModel.TwoInFourUserInput.EmitTurnedOnOrOffEvent += ToggleTwoInFour;
        ToggleFourInFour(_userInputsModel.TwoInFourUserInput.IsPressed);

        _userInputsModel.EightInFourUserInput.EmitTurnedOnOrOffEvent += ToggleEightInFour;
        ToggleFourInFour(_userInputsModel.EightInFourUserInput.IsPressed);

        _userInputsModel.SixteenInFourUserInput.EmitTurnedOnOrOffEvent += ToggleSixteenInFour;
        ToggleFourInFour(_userInputsModel.SixteenInFourUserInput.IsPressed);

        _userInputsModel.OneInEightUserInput.EmitTurnedOnOrOffEvent += ToggleOneInEight;
        ToggleFourInFour(_userInputsModel.OneInEightUserInput.IsPressed);

    }

    private void ToggleFourInFour(bool isNowActive)
    {
        cubes[0].SetActive(isNowActive);
    }

    private void ToggleOneInFour(bool isNowActive)
    {
        cubes[1].SetActive(isNowActive);
    }

    private void ToggleTwoInFour(bool isNowActive)
    {
        cubes[2].SetActive(isNowActive);
    }

    private void ToggleEightInFour(bool isNowActive)
    {
        cubes[3].SetActive(isNowActive);
    }

    private void ToggleSixteenInFour(bool isNowActive)
    {
        cubes[4].SetActive(isNowActive);
    }

    private void ToggleOneInEight(bool isNowActive)
    {
        cubes[5].SetActive(isNowActive);
    }

    public void OnDisable()
    {
        Debug.Log("unsubscribing to test scene");
        UnsubscribeUserInputs();
    }


    public void SubscribeUserInputs()
    {
        RegisterMoodKeysSubscriptions();
        RegisterExplosionKeysSubscriptions();
        RegisterBeatRelatedUserInputs();
        RegisterVolumeRelatedUserInputs();
    }

    private void RegisterVolumeRelatedUserInputs()
    {
        _userInputsModel.AverageVolume.EmitTurnedOnOrOffEvent += ToggleVolume;
        _userInputsModel.LowFrequencyVolume.EmitTurnedOnOrOffEvent += ToggleLowFrequencyVolume;
    }


    private void ToggleVolume(bool isActive)
    {
        VolumeCube.SetActive(isActive);
    }


    private void ToggleLowFrequencyVolume(bool isActive)
    {
        LowFrequencyVolumeCube.SetActive(isActive);
    }


    public void UnsubscribeUserInputs()
    {
        for (int i = 0; i < _userInputsModel.MoodKeys.Keys.Length; i++)
        {
            _userInputsModel.MoodKeys.Keys[i].EmitCollectionKeyTriggeredEvent -= MoodKeysImplementation;
        }
        _userInputsModel.FourInFourUserInput.EmitTurnedOnOrOffEvent -= ToggleFourInFour;
        _userInputsModel.OneInFourUserInput.EmitTurnedOnOrOffEvent -= ToggleOneInFour;
        _userInputsModel.TwoInFourUserInput.EmitTurnedOnOrOffEvent -= ToggleTwoInFour;
        _userInputsModel.EightInFourUserInput.EmitTurnedOnOrOffEvent -= ToggleEightInFour;
        _userInputsModel.SixteenInFourUserInput.EmitTurnedOnOrOffEvent -= ToggleSixteenInFour;
        _userInputsModel.OneInEightUserInput.EmitTurnedOnOrOffEvent -= ToggleOneInEight;

        _userInputsModel.AverageVolume.EmitTurnedOnOrOffEvent -= ToggleVolume;
        _userInputsModel.LowFrequencyVolume.EmitTurnedOnOrOffEvent -= ToggleLowFrequencyVolume;

        for (int i = 0; i < _userInputsModel.ExplosionKeys.Keys.Length; i++)
        {
            _userInputsModel.ExplosionKeys.Keys[i].EmitCollectionKeyTriggeredEvent -= StartExplosionCoroutine;
        }
    }


    private void RegisterExplosionKeysSubscriptions()
    {
        for (int i = 0; i < _userInputsModel.ExplosionKeys.Keys.Length; i++)
        {
            _userInputsModel.ExplosionKeys.Keys[i].EmitCollectionKeyTriggeredEvent += StartExplosionCoroutine;
            ExplosionKeys[i].SetActive(false);
        }
    }

    private void StartExplosionCoroutine(int index)
    {
        StartCoroutine(ShowForAWhile(index));
    }

    private IEnumerator ShowForAWhile(int index)
    {
        ExplosionKeys[index].SetActive(true);


        yield return new WaitForSeconds(3f);

        ExplosionKeys[index].SetActive(false);
    }

    private void RegisterMoodKeysSubscriptions()
    {
        for (int i = 0; i < _userInputsModel.MoodKeys.Keys.Length; i++)
        {
            _userInputsModel.MoodKeys.Keys[i].EmitCollectionKeyTriggeredEvent += MoodKeysImplementation;
        }
    }


    private void MoodKeysImplementation(int index)
    {
        for (int i = 0; i < _userInputsModel.MoodKeys.Keys.Length; i++)
        {
            if (i == index)
            {
                MoodKeys[i].SetActive(true);
            }
            else
            {
                MoodKeys[i].SetActive(false);
            }
        }
    }

    private void MelodyKeysImplementation()
    {
        for (int i = 0; i < _userInputsModel.MelodyKeys.Keys.Length; i++)
        {
            if (_userInputsModel.MelodyKeys.Keys[i].IsPressed)
            {
                MelodyKeys[i].SetActive(true);
            }
            else
            {
                MelodyKeys[i].SetActive(false);
            }
        }
    }

    private void DroneKeysImplementation()
    {
        for (int i = 0; i < _userInputsModel.DroneKeys.Keys.Length; i++)
        {
            if (_userInputsModel.DroneKeys.Keys[i].IsPressed)
            {
                DroneKeys[i].SetActive(true);
            }
            else
            {
                DroneKeys[i].SetActive(false);
            }
        }
    }

    private void BeatElements()
    {
        for (int i = 0; i < cubes.Count; i++)
        {
            if (i == 0)
            {
                cubes[i].transform.localScale = new Vector3(cubes[i].transform.localScale.x, _musicValuesModel.FourInFourValue, cubes[i].transform.localScale.z);
                Color currentColor = cubes[i].GetComponent<Renderer>().material.color;
                cubes[i].GetComponent<Renderer>().material.color = new Color(currentColor.r, currentColor.g, currentColor.b, _userInputsModel.FourInFourUserInput.FaderValue);
            }

            if (i == 1)
            {
                cubes[i].transform.localScale = new Vector3(cubes[i].transform.localScale.x, _musicValuesModel.OneInFourValue, cubes[i].transform.localScale.z);
                Color currentColor = cubes[i].GetComponent<Renderer>().material.color;
                cubes[i].GetComponent<Renderer>().material.color = new Color(currentColor.r, currentColor.g, currentColor.b, _userInputsModel.OneInFourUserInput.FaderValue);
            }

            if (i == 2)
            {
                cubes[i].transform.localScale = new Vector3(cubes[i].transform.localScale.x, _musicValuesModel.TwoInFourValue, cubes[i].transform.localScale.z);
                Color currentColor = cubes[i].GetComponent<Renderer>().material.color;
                cubes[i].GetComponent<Renderer>().material.color = new Color(currentColor.r, currentColor.g, currentColor.b, _userInputsModel.TwoInFourUserInput.FaderValue);
            }

            if (i == 3)
            {
                cubes[i].transform.localScale = new Vector3(cubes[i].transform.localScale.x, _musicValuesModel.EightInFourValue, cubes[i].transform.localScale.z);
                Color currentColor = cubes[i].GetComponent<Renderer>().material.color;
                cubes[i].GetComponent<Renderer>().material.color = new Color(currentColor.r, currentColor.g, currentColor.b, _userInputsModel.EightInFourUserInput.FaderValue);
            }

            if (i == 4)
            {
                cubes[i].transform.localScale = new Vector3(cubes[i].transform.localScale.x, _musicValuesModel.SixteenInFourValue, cubes[i].transform.localScale.z);
                Color currentColor = cubes[i].GetComponent<Renderer>().material.color;
                cubes[i].GetComponent<Renderer>().material.color = new Color(currentColor.r, currentColor.g, currentColor.b, _userInputsModel.SixteenInFourUserInput.FaderValue);
            }

            if (i == 5)
            {
                cubes[i].transform.localScale = new Vector3(cubes[i].transform.localScale.x, _musicValuesModel.OneInEightValue, cubes[i].transform.localScale.z);
                Color currentColor = cubes[i].GetComponent<Renderer>().material.color;
                cubes[i].GetComponent<Renderer>().material.color = new Color(currentColor.r, currentColor.g, currentColor.b, _userInputsModel.OneInEightUserInput.FaderValue);
            }
        }
    }
}

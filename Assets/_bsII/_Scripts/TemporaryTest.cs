using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporaryTest : MonoBehaviour
{
    public List<GameObject> cubes;
    public List<GameObject> MelodyKeys;
    public List<GameObject> DroneKeys;
    public List<GameObject> MoodKeys;
    public List<GameObject> ExplosionKeys;
    private MusicValuesModel _musicValuesModel;
    private UserInputsModel _userInputsModel;

    // Start is called before the first frame update
    void Start()
    {
        _musicValuesModel = GameObject.FindGameObjectWithTag("MusicValuesModel").GetComponent<MusicValuesModel>();
        _userInputsModel = GameObject.FindGameObjectWithTag("UserInputsModel").GetComponent<UserInputsModel>();
        RegisterMoodKeysSubscriptions();
        RegisterExplosionKeysSubscriptions();
    }

    // Update is called once per frame
    void Update()
    {
        BeatElements();
        MelodyKeysImplementation();
        DroneKeysImplementation();
    }

    private void RegisterExplosionKeysSubscriptions()
    {
        for (int i = 0; i < _userInputsModel.MoodKeys.Keys.Length; i++)
        {
            _userInputsModel.ExplosionKeys.Keys[i].EmitCollectionKeyTriggeredEvent += StartIt;
            ExplosionKeys[i].SetActive(false);
        }
    }

    private void StartIt(int index)
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

    public void OnDisable()
    {
        for (int i = 0; i < _userInputsModel.MoodKeys.Keys.Length; i++)
        {
            _userInputsModel.MoodKeys.Keys[i].EmitCollectionKeyTriggeredEvent -= MoodKeysImplementation;
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
            }

            if (i == 1)
            {
                cubes[i].transform.localScale = new Vector3(cubes[i].transform.localScale.x, _musicValuesModel.OneInFourValue, cubes[i].transform.localScale.z);
            }

            if (i == 2)
            {
                cubes[i].transform.localScale = new Vector3(cubes[i].transform.localScale.x, _musicValuesModel.TwoInFourValue, cubes[i].transform.localScale.z);
            }

            if (i == 3)
            {
                cubes[i].transform.localScale = new Vector3(cubes[i].transform.localScale.x, _musicValuesModel.EightInFourValue, cubes[i].transform.localScale.z);
            }

            if (i == 4)
            {
                cubes[i].transform.localScale = new Vector3(cubes[i].transform.localScale.x, _musicValuesModel.SixteenInFourValue, cubes[i].transform.localScale.z);
            }

            if (i == 5)
            {
                cubes[i].transform.localScale = new Vector3(cubes[i].transform.localScale.x, _musicValuesModel.OneInEightValue, cubes[i].transform.localScale.z);
            }
        }
    }
}

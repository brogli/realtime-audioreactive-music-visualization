using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporaryTest : MonoBehaviour
{
    public List<GameObject> cubes;
    private MusicValuesModel _musicValuesModel;
    private UserInputsModel _userInputsModel;

    // Start is called before the first frame update
    void Start()
    {
        _musicValuesModel = GameObject.FindGameObjectWithTag("MusicValuesModel").GetComponent<MusicValuesModel>();
        _userInputsModel = GameObject.FindGameObjectWithTag("UserInputsModel").GetComponent<UserInputsModel>();
    }

    // Update is called once per frame
    void Update()
    {
        //for (int i = 0; i < cubes.Count; i++)
        //{
        //    if (i == 0)
        //    {
        //        cubes[i].transform.localScale = new Vector3(cubes[i].transform.localScale.x, _musicValuesModel.FourInFourValue, cubes[i].transform.localScale.z);
        //    }

        //    if (i == 1)
        //    {
        //        cubes[i].transform.localScale = new Vector3(cubes[i].transform.localScale.x, _musicValuesModel.OneInFourValue, cubes[i].transform.localScale.z);
        //    }

        //    if (i == 2)
        //    {
        //        cubes[i].transform.localScale = new Vector3(cubes[i].transform.localScale.x, _musicValuesModel.TwoInFourValue, cubes[i].transform.localScale.z);
        //    }

        //    if (i == 3)
        //    {
        //        cubes[i].transform.localScale = new Vector3(cubes[i].transform.localScale.x, _musicValuesModel.EightInFourValue, cubes[i].transform.localScale.z);
        //    }

        //    if (i == 4)
        //    {
        //        cubes[i].transform.localScale = new Vector3(cubes[i].transform.localScale.x, _musicValuesModel.SixteenInFourValue, cubes[i].transform.localScale.z);
        //    }
        //}

        //Debug.Log(_userInputsModel.MelodyKeys.Keys[0].IsPressed);
    }
}

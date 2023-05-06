using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HairSceneImplementation : MonoBehaviour
{
    public GameObject hair;

    private UserInputsModel _userInputsModel;
    private MusicInputsModel _musicInputsModel;

    // Start is called before the first frame update
    void Start()
    {
        _userInputsModel = GameObject.FindGameObjectWithTag("UserInputsModel").GetComponent<UserInputsModel>();
        _musicInputsModel = GameObject.FindGameObjectWithTag("MusicInputsModel").GetComponent<MusicInputsModel>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

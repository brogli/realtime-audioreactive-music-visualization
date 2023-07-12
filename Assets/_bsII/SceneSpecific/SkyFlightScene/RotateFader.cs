using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class RotateFader : MonoBehaviour
{
    public AnimationCurve faderCurve1;
    public SkyFlighSceneImplementation sceneImplementationComponent;
    public float rotateMaxValue = 180f;

    private UserInputsModel _userInputsModel;
    private MusicInputsModel _musicInputsModel;

    [SerializeField]
    private float _fader1_dummy;

    // Start is called before the first frame update
    void Start()
    {
        _userInputsModel = GameObject.FindGameObjectWithTag("UserInputsModel").GetComponent<UserInputsModel>();
        _musicInputsModel = GameObject.FindGameObjectWithTag("MusicInputsModel").GetComponent<MusicInputsModel>();
    }

    // Update is called once per frame
    void Update()
    {
        sceneImplementationComponent.rotValue = rotateMaxValue * faderCurve1.Evaluate(_userInputsModel.FourInFourUserInput.FaderValue);
    }
}


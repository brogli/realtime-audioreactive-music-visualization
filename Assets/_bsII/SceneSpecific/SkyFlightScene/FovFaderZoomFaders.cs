using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class FovFaderZoomFaders : MonoBehaviour
{
    public AnimationCurve faderCurve1;
    public Camera mainCamera;

    private float _fovDefaultValue;
    private UserInputsModel _userInputsModel;
    private MusicInputsModel _musicInputsModel;

    [SerializeField]
    private float _fader1_dummy;
    [SerializeField]
    private float _fader2_dummy;

    // Start is called before the first frame update
    void Start()
    {
        _userInputsModel = GameObject.FindGameObjectWithTag("UserInputsModel").GetComponent<UserInputsModel>();
        _musicInputsModel = GameObject.FindGameObjectWithTag("MusicInputsModel").GetComponent<MusicInputsModel>();

        _fovDefaultValue = mainCamera.fieldOfView;
    }

    // Update is called once per frame
    void Update()
    {
        float modifiedFovValue = _fovDefaultValue - 80f * faderCurve1.Evaluate(_userInputsModel.LowFrequencyVolume.FaderValue);
        modifiedFovValue += 19f * faderCurve1.Evaluate(_userInputsModel.TwoInFourUserInput.FaderValue);
        mainCamera.fieldOfView = modifiedFovValue;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class DistFaders : MonoBehaviour
{
    public AnimationCurve distCurve1;
    public Volume PPVolume;

    private float _scaleDefaultValue;
    private float _xMultiplierDefaultValue;
    private float _yMultiplierDefaultValue;
    private UserInputsModel _userInputsModel;

    [SerializeField]
    private float _fader1_dummy;
    [SerializeField]
    private float _fader2_dummy;
    [SerializeField]
    private float _fader3_dummy;

    private LensDistortion _lensDistortionComponent;


    // Start is called before the first frame update
    void Start()
    {
        _userInputsModel = GameObject.FindGameObjectWithTag("UserInputsModel").GetComponent<UserInputsModel>();

        if (PPVolume.profile.TryGet(out LensDistortion lensDistortionComponentTmp))
        {
            _lensDistortionComponent = lensDistortionComponentTmp;
        }

        _scaleDefaultValue = ((float)_lensDistortionComponent.scale);
        _xMultiplierDefaultValue = ((float)_lensDistortionComponent.xMultiplier);
        _yMultiplierDefaultValue = ((float)_lensDistortionComponent.yMultiplier);
    }

    // Update is called once per frame
    void Update()
    {
        _lensDistortionComponent.scale.value = _scaleDefaultValue - (_scaleDefaultValue - _lensDistortionComponent.scale.min) * distCurve1.Evaluate(_userInputsModel.EightInFourUserInput.FaderValue);
        _lensDistortionComponent.xMultiplier.value = _xMultiplierDefaultValue - (_xMultiplierDefaultValue - _lensDistortionComponent.xMultiplier.min) * distCurve1.Evaluate(_userInputsModel.SixteenInFourUserInput.FaderValue);
        _lensDistortionComponent.yMultiplier.value = _yMultiplierDefaultValue - (_yMultiplierDefaultValue - _lensDistortionComponent.yMultiplier.min) * distCurve1.Evaluate(_userInputsModel.OneInEightUserInput.FaderValue);
    }
}

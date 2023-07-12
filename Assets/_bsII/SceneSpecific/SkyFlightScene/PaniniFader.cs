using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class PaniniFader : MonoBehaviour
{
    public AnimationCurve faderCurve1;
    public AnimationCurve faderCurve2;
    public Volume PPVolume;

    private float _paniniDefaultValue;
    private UserInputsModel _userInputsModel;
    private MusicInputsModel _musicInputsModel;

    [SerializeField]
    private float _fader1_dummy;

    private PaniniProjection _paniniProjectionComponent;

    // Start is called before the first frame update
    void Start()
    {
        _userInputsModel = GameObject.FindGameObjectWithTag("UserInputsModel").GetComponent<UserInputsModel>();
        _musicInputsModel = GameObject.FindGameObjectWithTag("MusicInputsModel").GetComponent<MusicInputsModel>();

        if (PPVolume.profile.TryGet(out PaniniProjection paniniProjectionComponentTmp))
        {
            _paniniProjectionComponent = paniniProjectionComponentTmp;
        }

        _paniniDefaultValue = ((float)_paniniProjectionComponent.distance);
    }

    // Update is called once per frame
    void Update()
    {
        float modifiedPaniniValue = _paniniDefaultValue + (_paniniDefaultValue + _paniniProjectionComponent.distance.max) * faderCurve1.Evaluate(_userInputsModel.AverageVolume.FaderValue);
        _paniniProjectionComponent.distance.value = modifiedPaniniValue + 0.3f * (faderCurve2.Evaluate(_userInputsModel.AverageVolume.FaderValue) * _musicInputsModel.AverageVolumeNormalizedEasedSmoothed);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class SkyFlighSceneImplementation : MonoBehaviour, IMusicInputsConsumer, IUserInputsConsumer
{
    public GameObject cameraParent;
    public Camera mainCamera;
    public Volume PPVolume;
    public Texture[] textureList;
    public float fovDefault;

    public float _rotateValueX = 0.4f;
    public float _rotateValueY = 0.4f;
    public float _rotateValueZ = 0.5f;

    public float rotValue = 0f;

    private UserInputsModel _userInputsModel;
    private MusicInputsModel _musicInputsModel;

    private Vector3 _camRotation;
    private Explotion1 _explotion1;
    private int _hdrTextureIndex = 0;


    private Bloom _bloomComponent;
    private HDRISky _hdriSkyComponent;
    private ColorCurves _colorCurvesComponent;
    private LensDistortion _lensDistortionComponent;
    private PaniniProjection _paniniProjectionComponent;

    // Start is called before the first frame update
    void Start()
    {
        _userInputsModel = GameObject.FindGameObjectWithTag("UserInputsModel").GetComponent<UserInputsModel>();
        _musicInputsModel = GameObject.FindGameObjectWithTag("MusicInputsModel").GetComponent<MusicInputsModel>();

        _camRotation = Vector3.zero;

        if (PPVolume.profile.TryGet(out Bloom bloomComponentTmp))
        {
            _bloomComponent = bloomComponentTmp;
        }

        if (PPVolume.profile.TryGet(out HDRISky hdriSkyComponentTmp))
        {
            _hdriSkyComponent = hdriSkyComponentTmp;
        }

        if (PPVolume.profile.TryGet(out ColorCurves colorCurvesComponentTmp))
        {
            _colorCurvesComponent = colorCurvesComponentTmp;
        }

        if (PPVolume.profile.TryGet(out LensDistortion lensDistortionComponentTmp))
        {
            _lensDistortionComponent = lensDistortionComponentTmp;
        }

        if (PPVolume.profile.TryGet(out PaniniProjection paniniProjectionComponentTmp))
        {
            _paniniProjectionComponent = paniniProjectionComponentTmp;
        }

        _explotion1 = GetComponent<Explotion1>();

        SubscribeMusicInputs();
        SubscribeUserInputs();
    }

    // Update is called once per frame
    void Update()
    {
        _camRotation.x += _rotateValueX;
        _camRotation.y += _rotateValueY;
        _camRotation.z += _rotateValueZ;
        cameraParent.transform.eulerAngles = _camRotation;
    }
    void OnDisable()
    {
        UnsubscribeMusicInputs();
        UnsubscribeUserInputs();
    }

    #region music events

    private void HandleFourInFourMusicEvent()
    {
        if (_userInputsModel.FourInFourUserInput.IsPressed)
        {
            int randomSign = Random.Range(0, 2) * 2 - 1;
            _camRotation.z += rotValue * randomSign;
        }
    }

    private void HandleTwoInFourMusicEvent()
    {
        //fader only (fov zoom in)
    }

    private void HandleSixteenInFourMusicEvent()
    {
        //fader only (dist1)
    }

    private void HandleEightInFourMusicEvent()
    {
        //fader only (dist2)
    }
    private void HandleOneInEightMusicEvent()
    {
        //fader only (dist3)
    }

    private void HandleOneInFourMusicEvent()
    {
        if (_userInputsModel.OneInFourUserInput.IsPressed)
        {
            _camRotation = Random.rotation.eulerAngles;
            cameraParent.transform.eulerAngles = _camRotation;
        }
    }

    public void SubscribeMusicInputs()
    {
        _musicInputsModel.EmitFourInFourEvent += HandleFourInFourMusicEvent;
        _musicInputsModel.EmitTwoInFourEvent += HandleTwoInFourMusicEvent;
        _musicInputsModel.EmitSixteenInFourEvent += HandleSixteenInFourMusicEvent;
        _musicInputsModel.EmitEightInFourEvent += HandleEightInFourMusicEvent;
        _musicInputsModel.EmitOneInEightEvent += HandleOneInEightMusicEvent;
        _musicInputsModel.EmitOneInFourEvent += HandleOneInFourMusicEvent;
    }

    public void UnsubscribeMusicInputs()
    {
        _musicInputsModel.EmitFourInFourEvent -= HandleFourInFourMusicEvent;
        _musicInputsModel.EmitTwoInFourEvent -= HandleTwoInFourMusicEvent;
        _musicInputsModel.EmitSixteenInFourEvent -= HandleSixteenInFourMusicEvent;
        _musicInputsModel.EmitEightInFourEvent -= HandleEightInFourMusicEvent;
        _musicInputsModel.EmitOneInEightEvent -= HandleOneInEightMusicEvent;
        _musicInputsModel.EmitOneInFourEvent -= HandleOneInFourMusicEvent;
    }

    #endregion

    #region userinputs

    public void SubscribeUserInputs()
    {
        _userInputsModel.FourInFourUserInput.EmitTurnedOnOrOffEvent += HandleFourInFourUserEvent;
        HandleFourInFourUserEvent(_userInputsModel.FourInFourUserInput.IsPressed);

        _userInputsModel.TwoInFourUserInput.EmitTurnedOnOrOffEvent += HandleTwoInFourUserEvent;
        HandleTwoInFourUserEvent(_userInputsModel.TwoInFourUserInput.IsPressed);

        _userInputsModel.EightInFourUserInput.EmitTurnedOnOrOffEvent += HandleEightInFourUserEvent;
        HandleEightInFourUserEvent(_userInputsModel.EightInFourUserInput.IsPressed);

        _userInputsModel.SixteenInFourUserInput.EmitTurnedOnOrOffEvent += HandleSixteenInFourUserEvent;
        HandleSixteenInFourUserEvent(_userInputsModel.SixteenInFourUserInput.IsPressed);

        _userInputsModel.AverageVolume.EmitTurnedOnOrOffEvent += HandleVolumeUserEvent;
        HandleVolumeUserEvent(_userInputsModel.AverageVolume.IsPressed);

        _userInputsModel.LowFrequencyVolume.EmitTurnedOnOrOffEvent += HandleLfVolumeUserEvent;
        HandleLfVolumeUserEvent(_userInputsModel.LowFrequencyVolume.IsPressed);

        foreach (var key in _userInputsModel.MelodyKeys.Keys)
        {
            key.EmitTurnedOnOrOffEvent += HandleMelodyKey;
        }

        foreach (var key in _userInputsModel.MoodKeys.Keys)
        {
            key.EmitCollectionKeyTriggeredEvent += HandleMoodKey;
        }

        foreach (var key in _userInputsModel.DroneKeys.Keys)
        {
            key.EmitTurnedOnOrOffEvent += HandleDroneKey;
        }

        foreach (var key in _userInputsModel.ExplosionKeys.Keys)
        {
            key.EmitCollectionKeyTriggeredEvent += HandleExplosionKeys;
        }

        _userInputsModel.OneInEightUserInput.EmitTurnedOnOrOffEvent += HandleOneInEightUserEvent;
        HandleOneInEightUserEvent(_userInputsModel.OneInEightUserInput.IsPressed);

        _userInputsModel.OneInFourUserInput.EmitTurnedOnOrOffEvent += HandleOneInFourUserEvent;
        HandleOneInFourUserEvent(_userInputsModel.OneInFourUserInput.IsPressed);
    }

    private void HandleOneInFourUserEvent(bool hasTurnedOn)
    {
        if (!hasTurnedOn)
        {
        }
        else
        {
        }
    }

    public void UnsubscribeUserInputs()
    {
        _userInputsModel.FourInFourUserInput.EmitTurnedOnOrOffEvent -= HandleFourInFourUserEvent;
        _userInputsModel.TwoInFourUserInput.EmitTurnedOnOrOffEvent -= HandleTwoInFourUserEvent;
        _userInputsModel.EightInFourUserInput.EmitTurnedOnOrOffEvent -= HandleEightInFourUserEvent;
        _userInputsModel.SixteenInFourUserInput.EmitTurnedOnOrOffEvent -= HandleSixteenInFourUserEvent;
        _userInputsModel.AverageVolume.EmitTurnedOnOrOffEvent -= HandleVolumeUserEvent;
        _userInputsModel.LowFrequencyVolume.EmitTurnedOnOrOffEvent -= HandleLfVolumeUserEvent;

        foreach (var key in _userInputsModel.MelodyKeys.Keys)
        {
            key.EmitTurnedOnOrOffEvent -= HandleMelodyKey;
        }

        foreach (var key in _userInputsModel.MoodKeys.Keys)
        {
            key.EmitCollectionKeyTriggeredEvent -= HandleMoodKey;
        }

        foreach (var key in _userInputsModel.DroneKeys.Keys)
        {
            key.EmitTurnedOnOrOffEvent -= HandleDroneKey;
        }

        foreach (var key in _userInputsModel.ExplosionKeys.Keys)
        {
            key.EmitCollectionKeyTriggeredEvent -= HandleExplosionKeys;
        }

        _userInputsModel.OneInEightUserInput.EmitTurnedOnOrOffEvent -= HandleOneInEightUserEvent;
        _userInputsModel.OneInFourUserInput.EmitTurnedOnOrOffEvent -= HandleOneInFourUserEvent;
    }

    private void HandleExplosionKeys(int index)
    {
        switch (index)
        {
            case 4:
                _explotion1.PlayCoroutine(1f, -90f);
                break;
            case 5:
                _explotion1.PlayCoroutine(1f, 18f);
                break;
            case 6:
            case 7:
            case 0:
            case 1:
            case 2:
            case 3:
            default:
                break;
        }

    }

    private void HandleDroneKey(bool hasTurnedOn, int index)
    {
        if (hasTurnedOn)
        {
            switch (index)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                default:
                    break;
            }
        }
        else if (!hasTurnedOn)
        {
            switch (index)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                default:
                    break;
            }
        }
    }

    private void HandleMoodKey(int index)
    {
        switch (index)
        {
            // Switch HDR Texture and cam rotation
            case 0:
                int tmpTextureIndex;
                do
                {
                    tmpTextureIndex = Random.Range(0, textureList.Length);
                } while (tmpTextureIndex == _hdrTextureIndex);

                _hdrTextureIndex = tmpTextureIndex;
                _hdriSkyComponent.hdriSky.value = textureList[_hdrTextureIndex];
                break;
            // Pick Default HDR
            case 1:
                _hdrTextureIndex = 0;
                _hdriSkyComponent.hdriSky.value = textureList[_hdrTextureIndex];
                break;
            // Random Slow Rotation
            case 3:
                _rotateValueX = Random.Range(-0.2f, 0.2f);
                _rotateValueY = Random.Range(-0.2f, 0.2f);
                _rotateValueZ = Random.Range(-0.5f, 0.5f);
                break;
            // Random Default Rotation
            case 4:
                _rotateValueX = Random.Range(-0.6f, 0.6f);
                _rotateValueY = Random.Range(-0.6f, 0.6f);
                _rotateValueZ = Random.Range(-1, 1f);
                break;
            // Random Fast Rotation
            case 5:
                _rotateValueX = Random.Range(-1.2f, 1.2f);
                _rotateValueY = Random.Range(-1.2f, 1.2f);
                _rotateValueZ = Random.Range(-2, 2f);
                break;
            // Random Crazy Rotation
            case 6:
                _rotateValueX = Random.Range(-5f, 5f);
                _rotateValueY = Random.Range(-5f, 5f);
                _rotateValueZ = Random.Range(-10, 10f);
                break;
            // Random Insane Rotation
            case 7:
                _rotateValueX = Random.Range(-20f, 20f);
                _rotateValueY = Random.Range(-20f, 20f);
                _rotateValueZ = Random.Range(-400, 400f);
                break;
            // Random Extremely Slow Rotation
            case 2:
                _rotateValueX = Random.Range(-0.08f, 0.08f);
                _rotateValueY = Random.Range(-0.08f, 0.08f);
                _rotateValueZ = Random.Range(-0.2f, 0.2f);
                break;
            default:
                break;
        }
    }

    private void HandleMelodyKey(bool hasTurnedOn, int index)
    {
        if (hasTurnedOn)
        {
            switch (index)
            {
                case 0:
                    _colorCurvesComponent.master.overrideState = true;
                    break;
                case 1:
                    _colorCurvesComponent.red.overrideState = true;
                    break;
                case 2:
                    _colorCurvesComponent.blue.overrideState = true;
                    break;
                case 3:
                    _colorCurvesComponent.green.overrideState = true;
                    break;
                case 4:
                    _colorCurvesComponent.hueVsSat.overrideState = true;
                    break;
                case 5:
                    _colorCurvesComponent.satVsSat.overrideState = true;
                    break;
                case 6:
                    _colorCurvesComponent.lumVsSat.overrideState = true;
                    break;
                case 7:
                    _colorCurvesComponent.hueVsHue.value.AddKey(0.5f, Random.Range(-1f, 1f));
                    break;
                default:
                    break;
            }
        }
        else if (!hasTurnedOn)
        {
            switch (index)
            {
                case 0:
                    _colorCurvesComponent.master.overrideState = false;
                    break;
                case 1:
                    _colorCurvesComponent.red.overrideState = false;
                    break;
                case 2:
                    _colorCurvesComponent.blue.overrideState = false;
                    break;
                case 3:
                    _colorCurvesComponent.green.overrideState = false;
                    break;
                case 4:
                    _colorCurvesComponent.hueVsSat.overrideState = false;
                    break;
                case 5:
                    _colorCurvesComponent.satVsSat.overrideState = false;
                    break;
                case 6:
                    _colorCurvesComponent.lumVsSat.overrideState = false;
                    break;
                case 7:
                    _colorCurvesComponent.hueVsHue.value.RemoveKey(0);
                    break;
                default:
                    break;
            }
        }
    }

    private void HandleVolumeUserEvent(bool hasTurnedOn)
    {
        if (hasTurnedOn)
        {
            _paniniProjectionComponent.active = true;
        }
        else
        {
            _paniniProjectionComponent.active = false;
        }
    }

    private void HandleLfVolumeUserEvent(bool hasTurnedOn)
    {
    }

    private void HandleSixteenInFourUserEvent(bool hasTurnedOn)
    {
        if (hasTurnedOn)
        {
            _lensDistortionComponent.intensity.value = 1f;
        }
        else if (!_userInputsModel.EightInFourUserInput.IsPressed && !_userInputsModel.OneInEightUserInput.IsPressed)
        {
            _lensDistortionComponent.intensity.value = 0f;
        }
    }

    private void HandleEightInFourUserEvent(bool hasTurnedOn)
    {
        if (hasTurnedOn)
        {
            _lensDistortionComponent.intensity.value = 1f;
        }
        else if (!_userInputsModel.SixteenInFourUserInput.IsPressed && !_userInputsModel.OneInEightUserInput.IsPressed)
        {
            _lensDistortionComponent.intensity.value = 0f;
        }
    }

    private void HandleOneInEightUserEvent(bool hasTurnedOn)
    {
        if (hasTurnedOn)
        {
            _lensDistortionComponent.intensity.value = 1f;
        }
        else if (!_userInputsModel.SixteenInFourUserInput.IsPressed && !_userInputsModel.EightInFourUserInput.IsPressed)
        {
            _lensDistortionComponent.intensity.value = 0f;
        }
    }

    private void HandleFourInFourUserEvent(bool hasTurnedOn)
    {
    }

    private void HandleTwoInFourUserEvent(bool hasTurnedOn)
    {
    }
    #endregion
}

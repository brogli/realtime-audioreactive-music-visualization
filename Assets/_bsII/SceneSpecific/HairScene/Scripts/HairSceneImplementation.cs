using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class HairSceneImplementation : MonoBehaviour, IMusicInputsConsumer, IUserInputsConsumer
{
    public GameObject hair;
    public Transform FourInFourLeft;
    public Transform FourInFourRight;
    public Transform TwoInFourRight;
    public Transform TwoInFourLeft;
    public List<Transform> EightInFours;
    public Light MainLight;
    public float MainLightMinIntensity;
    public float MainLightMaxIntensity;

    public GameObject SixteenInFourPrefab;
    public float EightInFourXmin;
    public float EightInFourXmax;
    public float EightInFourZmin;
    public float EightInFourZmax;
    public float EightInFourYspawn;

    public float FourInFourMovementFactor = 1;
    public float TwoInFourMovementFactor = 1;
    public float EightInFourMovementFactor = 1;

    public List<Animator> melodyKeys = new List<Animator>();
    public Volume SceneVolume;
    public Transform DroneWind;
    public Animator ExplosionSphere;
    public Animator ExplosionSphere2;
    public GameObject HairballPrefab;

    private UserInputsModel _userInputsModel;
    private MusicInputsModel _musicInputsModel;

    private float _fourInFourStartZ;
    private float _twoInFourStartY;
    private float _eightInFourStartY;
    private bool _isRightFourInFoursTurn = true;
    private bool _isRightTwoInFoursTurn = true;

    private bool _isFourInFourUserInputOn;
    private bool _isTwoInFourUserInputOn;
    private bool _isEightInFourActive;
    private bool _isSixteenInFourActive;
    private bool _isVolumeActive;
    private bool _isLfVolumeActive;
    private int _eightInFourCounter = 0;

    private HDAdditionalLightData _mainLightData;
    private Animator _mainLightAnimator;
    private Vector3 _mainLightInitPosition;
    private SceneColorOverlayPostProcess _sceneColorOverlayPostProcessVolume;

    // Start is called before the first frame update
    void Start()
    {
        _userInputsModel = GameObject.FindGameObjectWithTag("UserInputsModel").GetComponent<UserInputsModel>();
        _musicInputsModel = GameObject.FindGameObjectWithTag("MusicInputsModel").GetComponent<MusicInputsModel>();
        _fourInFourStartZ = FourInFourRight.position.z;
        _twoInFourStartY = TwoInFourRight.localPosition.y;
        _eightInFourStartY = EightInFours[0].localPosition.y;
        _mainLightData = MainLight.GetComponent<HDAdditionalLightData>();
        _mainLightAnimator = MainLight.GetComponent<Animator>();
        _mainLightInitPosition = new Vector3(MainLight.transform.position.x, MainLight.transform.position.y, MainLight.transform.position.z);


        if (!SceneVolume.sharedProfile.TryGet<SceneColorOverlayPostProcess>(out _sceneColorOverlayPostProcessVolume))
        {
            throw new NullReferenceException(nameof(_sceneColorOverlayPostProcessVolume));
        }

        SubscribeMusicInputs();
        SubscribeUserInputs();
    }

    void OnDisable()
    {
        _sceneColorOverlayPostProcessVolume.intensity.value = 0;
        UnsubscribeMusicInputs();
        UnsubscribeUserInputs();
    }

    void Update()
    {
        if (_isLfVolumeActive)
        {
            _mainLightData.intensity = (_musicInputsModel.LowFrequencyVolumeNormalizedEasedSmoothed * (MainLightMaxIntensity - MainLightMinIntensity)) + MainLightMinIntensity;
        }
        else if (_isVolumeActive)
        {
            _mainLightData.intensity = (_musicInputsModel.AverageVolumeNormalizedEasedSmoothed * (MainLightMaxIntensity - MainLightMinIntensity)) + MainLightMinIntensity;
        }

        if (_userInputsModel.OneInFourUserInput.IsPressed)
        {
            AnimateOneInFour();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_isFourInFourUserInputOn)
        {
            AnimateFourInFour();
        }

        if (_isTwoInFourUserInputOn)
        {
            AnimateTwoInFour();
        }

        if (_isEightInFourActive)
        {
            AnimateEightInFour();
        }

    }

    private void AnimateOneInFour()
    {
        var value = _musicInputsModel.OneInFourValue;
        _mainLightAnimator.SetFloat("motionTime", value);
    }

    private void AnimateEightInFour()
    {
        var eightInFourNewPosition = _eightInFourStartY + _musicInputsModel.EightInFourValue * EightInFourMovementFactor;
        Transform eightInFourToMove = EightInFours[_eightInFourCounter];
        eightInFourToMove.position = new Vector3(
                eightInFourToMove.position.x,
                eightInFourNewPosition,
                eightInFourToMove.position.z
            );
    }

    private void AnimateFourInFour()
    {
        var fourInFourNewPosition = _fourInFourStartZ + _musicInputsModel.FourInFourValue * FourInFourMovementFactor;
        Transform fourInFourToMove = _isRightFourInFoursTurn ? FourInFourRight : FourInFourLeft;
        fourInFourToMove.position = new Vector3(
                fourInFourToMove.position.x,
                fourInFourToMove.position.y,
                fourInFourNewPosition
            );

    }

    private void AnimateTwoInFour()
    {

        var twoInFourNewPosition = _twoInFourStartY + _musicInputsModel.TwoInFourValue * TwoInFourMovementFactor;
        Transform twoInFourToMove = _isRightTwoInFoursTurn ? TwoInFourRight : TwoInFourLeft;
        twoInFourToMove.position = new Vector3(
                twoInFourToMove.position.x,
                twoInFourNewPosition,
                twoInFourToMove.position.z
            );

    }



    #region music events

    private void HandleFourInFourMusicEvent()
    {
        Transform fourInFourToReset = _isRightFourInFoursTurn ? FourInFourRight : FourInFourLeft;

        fourInFourToReset.position = new Vector3(fourInFourToReset.position.x, fourInFourToReset.position.y, _fourInFourStartZ);

        _isRightFourInFoursTurn = !_isRightFourInFoursTurn;
    }

    private void HandleTwoInFourMusicEvent()
    {
        _isRightTwoInFoursTurn = !_isRightTwoInFoursTurn;
    }

    private void HandleSixteenInFourMusicEvent()
    {
        if (_isSixteenInFourActive)
        {
            var spawnPosition = new Vector3(UnityEngine.Random.Range(EightInFourXmin, EightInFourXmax), EightInFourYspawn, UnityEngine.Random.Range(EightInFourZmin, EightInFourZmax));
            Instantiate(SixteenInFourPrefab, spawnPosition, Quaternion.Euler(-90, 0, 0));
        }
    }

    private void HandleEightInFourMusicEvent()
    {
        _eightInFourCounter = (_eightInFourCounter + 1) % 4;
    }



    private void HandleOneInEightMusicEvent()
    {
        _sceneColorOverlayPostProcessVolume.intensity.value = 1;
        _sceneColorOverlayPostProcessVolume.color.value = Color.HSVToRGB(UnityEngine.Random.Range(0f, 1f), 1, 1);
    }

    public void SubscribeMusicInputs()
    {
        _musicInputsModel.EmitFourInFourEvent += HandleFourInFourMusicEvent;
        _musicInputsModel.EmitTwoInFourEvent += HandleTwoInFourMusicEvent;
        _musicInputsModel.EmitSixteenInFourEvent += HandleSixteenInFourMusicEvent;
        _musicInputsModel.EmitEightInFourEvent += HandleEightInFourMusicEvent;
    }

    public void UnsubscribeMusicInputs()
    {
        _musicInputsModel.EmitFourInFourEvent -= HandleFourInFourMusicEvent;
        _musicInputsModel.EmitTwoInFourEvent -= HandleTwoInFourMusicEvent;
        _musicInputsModel.EmitSixteenInFourEvent -= HandleSixteenInFourMusicEvent;
        _musicInputsModel.EmitEightInFourEvent -= HandleEightInFourMusicEvent;
        _musicInputsModel.EmitOneInEightEvent -= HandleOneInEightMusicEvent;
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
            // reset main light
            _mainLightAnimator.enabled = false;
            MainLight.transform.position = _mainLightInitPosition;
        } else
        {
            _mainLightAnimator.enabled = true;
        }
    }

    public void UnsubscribeUserInputs()
    {
        _userInputsModel.FourInFourUserInput.EmitTurnedOnOrOffEvent -= HandleFourInFourUserEvent;
        _userInputsModel.TwoInFourUserInput.EmitTurnedOnOrOffEvent -= HandleTwoInFourUserEvent;
        _userInputsModel.EightInFourUserInput.EmitTurnedOnOrOffEvent -= HandleEightInFourUserEvent;
        _userInputsModel.SixteenInFourUserInput.EmitTurnedOnOrOffEvent -= HandleSixteenInFourUserEvent;
        _userInputsModel.AverageVolume.EmitTurnedOnOrOffEvent -= HandleVolumeUserEvent;
        _userInputsModel.AverageVolume.EmitTurnedOnOrOffEvent -= HandleLfVolumeUserEvent;

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

    private void HandleOneInEightUserEvent(bool hasTurnedOn)
    {
        if (hasTurnedOn)
        {
            _musicInputsModel.EmitOneInEightEvent += HandleOneInEightMusicEvent;
        } else
        {
            _sceneColorOverlayPostProcessVolume.intensity.value = 0;
            _musicInputsModel.EmitOneInEightEvent -= HandleOneInEightMusicEvent;
        }
    }

    private void HandleExplosionKeys(int index)
    {
        switch (index)
        {
            case 4:
            case 6:
                ExplosionSphere.SetTrigger("explode");
                break;
            case 5:
            case 7:
                ExplosionSphere2.SetTrigger("explode");
                break;
            case 0:
            case 1:
            case 2:
            case 3:
                float spawnX = UnityEngine.Random.Range(-2.5f, 2.5f);
                float spawnZ = UnityEngine.Random.Range(-10f, -9.8f); 
                Vector3 spawnPos = new Vector3(spawnX, 0f, spawnZ);
                GameObject obj = Instantiate(HairballPrefab, spawnPos, Quaternion.identity);
                Destroy(obj, 1f);
                break;
            default:
                break;
        }

    }

    private void HandleDroneKey(bool hasTurnedOn, int index)
    {
        if (hasTurnedOn)
        {
            DroneWind.gameObject.SetActive(true);
            DroneWind.eulerAngles = new Vector3(0, index * 45, 0);
        }
        else
        {
            DroneWind.gameObject.SetActive(false);
        }
    }

    private void HandleMoodKey(int index)
    {
        if (index == 0)
        {
            _sceneColorOverlayPostProcessVolume.intensity.value = 0;
        }
        else
        {
            _sceneColorOverlayPostProcessVolume.intensity.value = 1;
            _sceneColorOverlayPostProcessVolume.color.value = Color.HSVToRGB((index - 1) * 0.14285f, 1, 1);
        }
    }

    private void HandleMelodyKey(bool hasTurnedOn, int index)
    {
        if (hasTurnedOn)
        {
            melodyKeys[index].Rebind();
            melodyKeys[index].Play("colorAnimation");
            melodyKeys[index].SetFloat("animationSpeed", 0);
        }
        if (!hasTurnedOn)
        {
            melodyKeys[index].SetFloat("animationSpeed", 1);
        }
    }

    private void HandleVolumeUserEvent(bool hasTurnedOn)
    {
        _isVolumeActive = hasTurnedOn;
        if (!hasTurnedOn && !_isLfVolumeActive)
        {
            _mainLightData.intensity = 1414;
        }
    }

    private void HandleLfVolumeUserEvent(bool hasTurnedOn)
    {
        _isLfVolumeActive = hasTurnedOn;
        if (!hasTurnedOn && !_isVolumeActive)
        {
            _mainLightData.intensity = 1414;
        }
    }

    private void HandleSixteenInFourUserEvent(bool hasTurnedOn)
    {
        _isSixteenInFourActive = hasTurnedOn;
    }

    private void HandleEightInFourUserEvent(bool hasTurnedOn)
    {
        _isEightInFourActive = hasTurnedOn;
        EightInFours.ForEach(e => { e.gameObject.SetActive(hasTurnedOn); });
    }

    private void HandleFourInFourUserEvent(bool hasTurnedOn)
    {
        _isFourInFourUserInputOn = hasTurnedOn;
        if (!_isFourInFourUserInputOn)
        {
            FourInFourLeft.gameObject.SetActive(false);
            FourInFourRight.gameObject.SetActive(false);
        }
        else
        {
            FourInFourLeft.gameObject.SetActive(true);
            FourInFourRight.gameObject.SetActive(true);
        }
    }

    private void HandleTwoInFourUserEvent(bool hasTurnedOn)
    {
        _isTwoInFourUserInputOn = hasTurnedOn;
        if (!_isTwoInFourUserInputOn)
        {
            TwoInFourLeft.gameObject.SetActive(false);
            TwoInFourRight.gameObject.SetActive(false);
        }
        else
        {
            TwoInFourLeft.gameObject.SetActive(true);
            TwoInFourRight.gameObject.SetActive(true);
        }
    }
    #endregion
}


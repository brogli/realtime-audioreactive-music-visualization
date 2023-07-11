using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class SkyFlighSceneImplementation : MonoBehaviour, IMusicInputsConsumer, IUserInputsConsumer
{
    public GameObject cameraParent;
    public AnimationCurve distCuve1;
    public Volume PPVolume;

    public float _rotateValueX = 0.4f;
    public float _rotateValueY = 0.4f;
    public float _rotateValueZ = 0.5f;

    private UserInputsModel _userInputsModel;
    private MusicInputsModel _musicInputsModel;

    private Vector3 _camRotation;

    Bloom bloomComponent;

    // Start is called before the first frame update
    void Start()
    {
        _userInputsModel = GameObject.FindGameObjectWithTag("UserInputsModel").GetComponent<UserInputsModel>();
        _musicInputsModel = GameObject.FindGameObjectWithTag("MusicInputsModel").GetComponent<MusicInputsModel>();

        SubscribeMusicInputs();
        SubscribeUserInputs();

        _camRotation = Vector3.zero;

        if (PPVolume.profile.TryGet(out Bloom tmp))
        {
            bloomComponent = tmp;
        }
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
        //_rotateValueX *= -1f;
        //_rotateValueY *= -1f;
        //_rotateValueZ *= -1f;
    }

    private void HandleTwoInFourMusicEvent()
    {

    }

    private void HandleSixteenInFourMusicEvent()
    {

    }

    private void HandleEightInFourMusicEvent()
    {

    }



    private void HandleOneInEightMusicEvent()
    {

    }

    private void HandleOneInFourMusicEvent()
    {
        if (_userInputsModel.FourInFourUserInput.IsPressed)
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
        }
        else
        {
        }
    }

    private void HandleExplosionKeys(int index)
    {
        switch (index)
        {
            case 4:
            case 6:
            case 5:
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
    }

    private void HandleMoodKey(int index)
    {
    }

    private void HandleMelodyKey(bool hasTurnedOn, int index)
    {
    }

    private void HandleVolumeUserEvent(bool hasTurnedOn)
    {
    }

    private void HandleLfVolumeUserEvent(bool hasTurnedOn)
    {
    }

    private void HandleSixteenInFourUserEvent(bool hasTurnedOn)
    {
    }

    private void HandleEightInFourUserEvent(bool hasTurnedOn)
    {
    }

    private void HandleFourInFourUserEvent(bool hasTurnedOn)
    {
    }

    private void HandleTwoInFourUserEvent(bool hasTurnedOn)
    {
    }
    #endregion
}

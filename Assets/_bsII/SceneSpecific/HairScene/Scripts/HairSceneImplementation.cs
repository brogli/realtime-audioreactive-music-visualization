using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HairSceneImplementation : MonoBehaviour, IMusicInputsConsumer, IUserInputsConsumer
{
    public GameObject hair;
    public Transform FourInFourLeft;
    public Transform FourInFourRight;
    public Transform TwoInFourRight;
    public Transform TwoInFourLeft;

    public float FourInFourMovementFactor = 1;
    public float TwoInFourMovementFactor = 1;

    private UserInputsModel _userInputsModel;
    private MusicInputsModel _musicInputsModel;

    private float _fourInFourStartZ;
    private float _twoInFourStartY;
    private bool _isRightFourInFoursTurn = true;
    private bool _isRightTwoInFoursTurn = true;

    private bool _isFourInFourUserInputOn;
    private bool _isTwoInFourUserInputOn;

    // Start is called before the first frame update
    void Start()
    {
        _userInputsModel = GameObject.FindGameObjectWithTag("UserInputsModel").GetComponent<UserInputsModel>();
        _musicInputsModel = GameObject.FindGameObjectWithTag("MusicInputsModel").GetComponent<MusicInputsModel>();
        _fourInFourStartZ = FourInFourRight.position.z;
        _twoInFourStartY = TwoInFourRight.localPosition.y;
        SubscribeMusicInputs();
        SubscribeUserInputs();
    }

    void OnDisable()
    {
        UnsubscribeMusicInputs();
        UnsubscribeUserInputs();
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

    public void SubscribeMusicInputs()
    {
        _musicInputsModel.EmitFourInFourEvent += HandleFourInFourMusicEvent;
        _musicInputsModel.EmitTwoInFourEvent += HandleTwoInFourMusicEvent;
    }

    public void UnsubscribeMusicInputs()
    {
        _musicInputsModel.EmitFourInFourEvent -= HandleFourInFourMusicEvent;
        _musicInputsModel.EmitTwoInFourEvent -= HandleTwoInFourMusicEvent;
    }

    #region userinputs

    public void SubscribeUserInputs()
    {
        _userInputsModel.FourInFourUserInput.EmitTurnedOnOrOffEvent += HandleFourInFourUserEvent;
        HandleFourInFourUserEvent(_userInputsModel.FourInFourUserInput.IsPressed);

        _userInputsModel.TwoInFourUserInput.EmitTurnedOnOrOffEvent += HandleTwoInFourUserEvent;
        HandleTwoInFourUserEvent(_userInputsModel.TwoInFourUserInput.IsPressed);
    }

    public void UnsubscribeUserInputs()
    {
        _userInputsModel.FourInFourUserInput.EmitTurnedOnOrOffEvent -= HandleFourInFourUserEvent;
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


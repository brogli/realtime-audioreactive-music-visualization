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
    public Transform EightInFourShadowRight;
    public Transform EightInFourShadowLeft;
    public float EightInFourShadowScaleFactor = 1;

    public GameObject SixteenInFourPrefab;
    public float EightInFourXmin;
    public float EightInFourXmax;
    public float EightInFourZmin;
    public float EightInFourZmax;
    public float EightInFourYspawn;

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
    private bool _isEightInFourActive;
    private bool _isSixteenInFourActive;

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

    void Update()
    {
        if (_isEightInFourActive)
        {
            var eightInFourAsSine = Mathf.Sin(_musicInputsModel.FourInFourValue * 2 * Mathf.PI) * EightInFourShadowScaleFactor;
            EightInFourShadowRight.localScale = new Vector3(eightInFourAsSine, EightInFourShadowRight.localScale.y, EightInFourShadowRight.localScale.z);

            EightInFourShadowLeft.localScale = new Vector3(eightInFourAsSine, EightInFourShadowLeft.localScale.y, EightInFourShadowLeft.localScale.z);
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

    public void SubscribeMusicInputs()
    {
        _musicInputsModel.EmitFourInFourEvent += HandleFourInFourMusicEvent;
        _musicInputsModel.EmitTwoInFourEvent += HandleTwoInFourMusicEvent;
        _musicInputsModel.EmitSixteenInFourEvent += HandleSixteenInFourMusicEvent;
    }

    public void UnsubscribeMusicInputs()
    {
        _musicInputsModel.EmitFourInFourEvent -= HandleFourInFourMusicEvent;
        _musicInputsModel.EmitTwoInFourEvent -= HandleTwoInFourMusicEvent;
        _musicInputsModel.EmitSixteenInFourEvent -= HandleSixteenInFourMusicEvent;
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
        HandleEightInFourUserEvent(_userInputsModel.SixteenInFourUserInput.IsPressed);
    }

    public void UnsubscribeUserInputs()
    {
        _userInputsModel.FourInFourUserInput.EmitTurnedOnOrOffEvent -= HandleFourInFourUserEvent;
        _userInputsModel.TwoInFourUserInput.EmitTurnedOnOrOffEvent -= HandleTwoInFourUserEvent;
        _userInputsModel.EightInFourUserInput.EmitTurnedOnOrOffEvent -= HandleEightInFourUserEvent;
        _userInputsModel.SixteenInFourUserInput.EmitTurnedOnOrOffEvent -= HandleSixteenInFourUserEvent;
    }

    private void HandleSixteenInFourUserEvent(bool hasTurnedOn)
    {
        _isSixteenInFourActive = hasTurnedOn;
    }

    private void HandleEightInFourUserEvent(bool hasTurnedOn)
    {
        _isEightInFourActive = hasTurnedOn;
        if (!_isEightInFourActive)
        {
            EightInFourShadowRight.gameObject.SetActive(false);
            EightInFourShadowLeft.gameObject.SetActive(false);
        }
        else
        {
            EightInFourShadowRight.gameObject.SetActive(true);
            EightInFourShadowLeft.gameObject.SetActive(true);
        }
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


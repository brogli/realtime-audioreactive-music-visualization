using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftRightMovementSceneImplementation : MonoBehaviour, IMusicInputsConsumer, IUserInputsConsumer
{
    [SerializeField]
    private Transform _fourAndEightInFourElement;
    private float _fourAndEightInFourOriginalX;
    private float _fourAndEightInFourOriginalY;

    [SerializeField]
    private float _fourInFourLeftXcoordinate = 5;
    [SerializeField]
    private float _fourInFourMovementFactor = 20;
    private bool _isFourInFourGoingRight = true;

    [SerializeField]
    private float _eightInFourLeftYcoordinate = 5;
    [SerializeField]
    private float _eightInFourMovementFactor = 20;
    private bool _isEightInFourGoingUp;

    private UserInputsModel _userInputsModel;
    private MusicInputsModel _musicInputsModel;


    // Start is called before the first frame update
    void Start()
    {
        _userInputsModel = GameObject.FindGameObjectWithTag("UserInputsModel").GetComponent<UserInputsModel>();
        _musicInputsModel = GameObject.FindGameObjectWithTag("MusicInputsModel").GetComponent<MusicInputsModel>();

        SubscribeMusicInputs();
        SubscribeUserInputs();
    }

    private void OnDisable()
    {
        UnsubscribeMusicInputs();
        UnsubscribeUserInputs();
    }

    void Update()
    {
        Vector3 fourInFourElementPosition = _fourAndEightInFourElement.position;
        if (_userInputsModel.FourInFourUserInput.IsPressed)
        {
            float xValue = _musicInputsModel.FourInFourValueExtrapolated;
            if (!_isFourInFourGoingRight)
            {
                xValue = 1 - xValue;
            }
            float xCoordinate = Easings.EaseInOutQuad(xValue) * _fourInFourMovementFactor - _fourInFourLeftXcoordinate;
            fourInFourElementPosition = new Vector3(xCoordinate, fourInFourElementPosition.y, fourInFourElementPosition.z);
        }

        if (_userInputsModel.EightInFourUserInput.IsPressed)
        {
            float yValue = _musicInputsModel.EightInFourValueExtrapolated;
            if (!_isEightInFourGoingUp)
            {
                yValue = 1 - yValue;
            }
            float yCoordinate = Easings.EaseInOutQuad(yValue) * _eightInFourMovementFactor - _eightInFourLeftYcoordinate;
            fourInFourElementPosition = new Vector3(fourInFourElementPosition.x, yCoordinate, fourInFourElementPosition.z);
        }

        _fourAndEightInFourElement.position = fourInFourElementPosition;

        _fourAndEightInFourElement.Rotate(Vector3.right * 50 * Time.deltaTime, Space.World);
    }

    #region music inputs


    public void SubscribeMusicInputs()
    {
        _musicInputsModel.EmitFourInFourEvent += HandleFourInFourMusicEvent;
        _musicInputsModel.EmitEightInFourEvent += HandleEightInFourMusicEvent;
        _musicInputsModel.EmitTwoInFourEvent += HandleTwoInFourMusicEvent;
    }

    public void UnsubscribeMusicInputs()
    {
        _musicInputsModel.EmitFourInFourEvent -= HandleFourInFourMusicEvent;
        _musicInputsModel.EmitEightInFourEvent -= HandleEightInFourMusicEvent;
        _musicInputsModel.EmitTwoInFourEvent -= HandleTwoInFourMusicEvent;
    }

    private void HandleTwoInFourMusicEvent()
    {
        if (_userInputsModel.TwoInFourUserInput.IsPressed)
        {
            StartCoroutine("TwoInFourCoroutine");
        }
    }

    private IEnumerator TwoInFourCoroutine()
    {
        Vector3 originalScale = _fourAndEightInFourElement.transform.parent.localScale;

        _fourAndEightInFourElement.transform.parent.localScale = new Vector3(originalScale.x, originalScale.y + 2, originalScale.z);
        while (Vector3.Distance(_fourAndEightInFourElement.transform.parent.localScale, originalScale) > 0.1f)
        {
            var currentScale = _fourAndEightInFourElement.transform.parent.localScale;
            _fourAndEightInFourElement.transform.parent.localScale = new Vector3(currentScale.x, currentScale.y - 0.1f, currentScale.z);
            yield return null;
        }
        _fourAndEightInFourElement.transform.parent.localScale = originalScale;
    }

    private void HandleEightInFourMusicEvent()
    {
        _isEightInFourGoingUp = !_isEightInFourGoingUp;
    }

    private void HandleFourInFourMusicEvent()
    {
        _isFourInFourGoingRight = !_isFourInFourGoingRight;
    }
    #endregion


    #region user inputs

    public void SubscribeUserInputs()
    {
        _userInputsModel.FourInFourUserInput.EmitTurnedOnOrOffEvent += HandleFourInFourUserInput;
        _userInputsModel.EightInFourUserInput.EmitTurnedOnOrOffEvent += HandleEightInFourUserInput;
    }

    public void UnsubscribeUserInputs()
    {
        _userInputsModel.FourInFourUserInput.EmitTurnedOnOrOffEvent -= HandleFourInFourUserInput;
        _userInputsModel.EightInFourUserInput.EmitTurnedOnOrOffEvent -= HandleEightInFourUserInput;
    }

    private void HandleFourInFourUserInput(bool hasTurnedOn)
    {
        if (hasTurnedOn)
        {
            _fourAndEightInFourOriginalX = _fourAndEightInFourElement.position.x;
        } else
        {
            _fourAndEightInFourElement.position = new Vector3(_fourAndEightInFourOriginalX, _fourAndEightInFourElement.position.y, _fourAndEightInFourElement.position.z);
        }
    }

    private void HandleEightInFourUserInput(bool hasTurnedOn)
    {
        if (hasTurnedOn)
        {
            _fourAndEightInFourOriginalY = _fourAndEightInFourElement.position.y;
        }
        else
        {
            _fourAndEightInFourElement.position = new Vector3(_fourAndEightInFourElement.position.x, _fourAndEightInFourOriginalY, _fourAndEightInFourElement.position.z);
        }
    }
    #endregion
}

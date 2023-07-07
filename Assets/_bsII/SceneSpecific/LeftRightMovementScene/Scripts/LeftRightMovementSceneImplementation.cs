using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftRightMovementSceneImplementation : MonoBehaviour
{
    [SerializeField]
    private Transform _fourInFourElement;

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

    }

    private void OnDisable()
    {
        UnsubscribeMusicInputs();
    }

    void Update()
    {
        Vector3 fourInFourElementPosition = _fourInFourElement.position;
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

        _fourInFourElement.position = fourInFourElementPosition;

        _fourInFourElement.Rotate(Vector3.right * 50 * Time.deltaTime, Space.World);
    }

    #region music inputs


    private void SubscribeMusicInputs()
    {
        _musicInputsModel.EmitFourInFourEvent += HandleFourInFourMusicEvent;
        _musicInputsModel.EmitEightInFourEvent += HandleEightInFourMusicEvent;
        _musicInputsModel.EmitTwoInFourEvent += HandleTwoInFourMusicEvent;
    }

    private void UnsubscribeMusicInputs()
    {
        _musicInputsModel.EmitFourInFourEvent -= HandleFourInFourMusicEvent;
        _musicInputsModel.EmitEightInFourEvent -= HandleEightInFourMusicEvent;
        _musicInputsModel.EmitTwoInFourEvent -= HandleTwoInFourMusicEvent;
    }

    private void HandleTwoInFourMusicEvent()
    {
        //Vector3 originalScale = _fourInFourElement.transform.parent.localScale;

        //_fourInFourElement.transform.parent.localScale = new Vector3(originalScale.x, originalScale.y + 5, originalScale.z);
        if (_userInputsModel.TwoInFourUserInput.IsPressed)
        {
            StartCoroutine("TwoInFourCoroutine");
        }
    }

    private IEnumerator TwoInFourCoroutine()
    {
        Vector3 originalScale = _fourInFourElement.transform.parent.localScale;

        _fourInFourElement.transform.parent.localScale = new Vector3(originalScale.x, originalScale.y + 2, originalScale.z);
        while (Vector3.Distance(_fourInFourElement.transform.parent.localScale, originalScale) > 0.1f)
        {
            var currentScale = _fourInFourElement.transform.parent.localScale;
            _fourInFourElement.transform.parent.localScale = new Vector3(currentScale.x, currentScale.y - 0.1f, currentScale.z);
            yield return null;
        }
        Debug.Log("coroutin edone");
        _fourInFourElement.transform.parent.localScale = originalScale;
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
}

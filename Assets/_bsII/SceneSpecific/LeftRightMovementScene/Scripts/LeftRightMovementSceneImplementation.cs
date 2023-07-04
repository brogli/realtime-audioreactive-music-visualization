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
        if (_userInputsModel.FourInFourUserInput.IsPressed)
        {
            float xValue = _musicInputsModel.FourInFourValue;
            if (!_isFourInFourGoingRight)
            {
                xValue = 1 - xValue;
            }
            _fourInFourElement.position = new Vector3(Easings.EaseInOutQuad(xValue) * _fourInFourMovementFactor - _fourInFourLeftXcoordinate, _fourInFourElement.position.y, _fourInFourElement.position.z);
        }
    }

    #region music inputs


    private void SubscribeMusicInputs()
    {
        _musicInputsModel.EmitFourInFourEvent += HandleFourInFourMusicEvent;
    }

    private void UnsubscribeMusicInputs()
    {
        _musicInputsModel.EmitFourInFourEvent -= HandleFourInFourMusicEvent;
    }


    private void HandleFourInFourMusicEvent()
    {
        _isFourInFourGoingRight = !_isFourInFourGoingRight;
    }
    #endregion
}

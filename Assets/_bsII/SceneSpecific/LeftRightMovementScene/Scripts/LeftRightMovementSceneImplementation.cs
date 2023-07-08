using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class LeftRightMovementSceneImplementation : MonoBehaviour, IMusicInputsConsumer, IUserInputsConsumer
{
    [SerializeField]
    private Transform _fourAndEightInFourElement;
    private Mesh _fourAndEightInFourMesh;
    private Vector3[] _fourAndEightInFourOriginalVertices;
    private float _fourAndEightInFourOriginalX;
    private float _fourAndEightInFourOriginalY;
    [SerializeField]
    private float _perlinFactor = 1.0f;

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

        _fourAndEightInFourMesh = _fourAndEightInFourElement.GetComponent<MeshFilter>().mesh;
        _fourAndEightInFourOriginalVertices = _fourAndEightInFourMesh.vertices;
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

        _fourAndEightInFourElement.transform.Rotate(Vector3.right * 50 * Time.deltaTime, Space.World);
        AnimateVolume();


    }

    private void AnimateVolume()
    {
        float perlinFactor = _perlinFactor;
        if (_userInputsModel.AverageVolume.IsPressed)
        {
            perlinFactor = _musicInputsModel.AverageVolumeNormalizedEasedSmoothed * 100;
        }

        if (_userInputsModel.LowFrequencyVolume.IsPressed)
        {
            perlinFactor = _musicInputsModel.LowFrequencyVolumeNormalizedEased * 200;
        }
        Vector3[] vertices = _fourAndEightInFourMesh.vertices;
        for (int i = 0; i < vertices.Length; i++)
        {
            float perlinValue = Perlin.Noise(Time.timeSinceLevelLoad + (vertices[i].x * perlinFactor), Time.timeSinceLevelLoad + (vertices[i].y * perlinFactor), Time.timeSinceLevelLoad + (vertices[i].z * perlinFactor)) * .5f + 1.5f;

            var length = vertices[i].magnitude;
            var desiredLength = length * perlinValue;
            var weight = desiredLength / length;
            vertices[i] = weight * _fourAndEightInFourOriginalVertices[i];

        }
        _fourAndEightInFourMesh.vertices = vertices;
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
        foreach (var key in _userInputsModel.ExplosionKeys.Keys)
        {
            key.EmitCollectionKeyTriggeredEvent += HandleExplosionKey;
        }

    }

    public void UnsubscribeUserInputs()
    {
        _userInputsModel.FourInFourUserInput.EmitTurnedOnOrOffEvent -= HandleFourInFourUserInput;
        _userInputsModel.EightInFourUserInput.EmitTurnedOnOrOffEvent -= HandleEightInFourUserInput;
        foreach (var key in _userInputsModel.ExplosionKeys.Keys)
        {
            key.EmitCollectionKeyTriggeredEvent -= HandleExplosionKey;
        }
    }

    private void HandleExplosionKey(int index)
    {
        Debug.Log(index);
        if (index >= 0 && index <= 3)
        {
            // drums
        }
        else
        {
            // big explosions

            for (var i = 1; i <= 8; i++)
            {
                Vector3 spawnPosition = _fourAndEightInFourElement.position + GetOffsetVector(index, i);
                var gameObjecto = Instantiate(_fourAndEightInFourElement.gameObject, spawnPosition, _fourAndEightInFourElement.rotation);
                var rigidBody = gameObjecto.AddComponent<Rigidbody>();
                rigidBody.useGravity = false;
                rigidBody.velocity = GetOffsetVector(index, i);
                gameObjecto.AddComponent<RotateThySelf>();
                gameObjecto.AddComponent<FadeOutAndDestroyThySelf>();
                Destroy(gameObjecto, 5f);
            }
            for (var i = 1; i <= 8; i++)
            {
                Vector3 spawnPosition = _fourAndEightInFourElement.position + GetOffsetVector(index, -i);
                var gameObjecto = Instantiate(_fourAndEightInFourElement.gameObject, spawnPosition, _fourAndEightInFourElement.rotation);
                var rigidBody = gameObjecto.AddComponent<Rigidbody>();
                rigidBody.useGravity = false;
                rigidBody.velocity = GetOffsetVector(index, -i);
                gameObjecto.AddComponent<RotateThySelf>();
                gameObjecto.AddComponent<FadeOutAndDestroyThySelf>();
                Destroy(gameObjecto, 5f);
            }

        }
        //switch (index)
        //{
        //    case 4:
        //    case 6:
        //        ExplosionSphere.SetTrigger("explode");
        //        break;
        //    case 5:
        //    case 7:
        //        ExplosionSphere2.SetTrigger("explode");
        //        break;
        //    case 0:
        //    case 1:
        //    case 2:
        //    case 3:
        //        // drums
        //        break;
        //    default:
        //        break;
        //}
    }

    private Vector3 GetOffsetVector(float keyIndex, float iterationIndex)
    {
        Vector3 offset = Vector3.zero;

        if (keyIndex == 4)
        {
            offset = new Vector3(0, iterationIndex, 0);
        }
        else if (keyIndex == 5)
        {
            offset = new Vector3(-iterationIndex, iterationIndex, 0);
        }
        else if (keyIndex == 6)
        {
            offset = new Vector3(-iterationIndex, 0, 0);
        }
        else if (keyIndex == 7)
        {
            offset = new Vector3(-iterationIndex, -iterationIndex, 0);
        }

        return offset;
    }

    private void HandleFourInFourUserInput(bool hasTurnedOn)
    {
        if (hasTurnedOn)
        {
            _fourAndEightInFourOriginalX = _fourAndEightInFourElement.position.x;
        }
        else
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

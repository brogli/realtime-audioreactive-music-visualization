using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace LeftRightMovementScene
{
    public class SceneImplementation : MonoBehaviour, IMusicInputsConsumer, IUserInputsConsumer
    {
        [SerializeField]
        private List<Transform> _fourAndEightInFourElements;
        private int _fourInFourIndex = 0;
        private List<Mesh> _fourInFourMeshes = new();
        private List<Vector3[]> _fourInFourOriginalVertices = new();
        private List<float> _fourAndEightInFourOriginalX = new();
        private List<float> _fourAndEightInFourOriginalY = new();
        [SerializeField]
        private Transform _fourAndEightInFourParent;
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

        [SerializeField]
        private GameObject _explosionDrumPrefab;

        [SerializeField]
        private GameObject _sixteenInFourHelperObj;
        [SerializeField]
        private GameObject _sixteenInFourPrefab;
        [SerializeField]
        private Transform _sixteenInFourParent;
        private List<GameObject> _sixteenInFourObjs = new();
        private List<FadeOutQuick> _sixteenInFourFadeoutComponents = new();
        private int _sixteenInFourIndex = 0;

        [SerializeField]
        private Transform _oneInEightObject;
        private Animator _oneInEightAnimator;
        private bool _isCamMovingBack = false;
        private float _prevOneInEightValue = 0;

        [SerializeField]
        private GameObject _dronePrefab;
        private GameObject[] _droneObjects = new GameObject[8];

        [SerializeField]
        private Volume _sceneVolume;
        private SceneColorOverlayPostProcess _sceneColorOverlayPostProcessVolume;

        [SerializeField]
        private List<Renderer> _melodyKeyRenderers;
        [SerializeField]
        private float _melodyKeyTargetIntensity = 1;
        private List<LerpToTargetColor> _lerpToTargetColorComponents = new();
        private Color _melodyKeyOriginalEmissiveColor;

        private UserInputsModel _userInputsModel;
        private MusicInputsModel _musicInputsModel;

        // Start is called before the first frame update
        void Start()
        {
            _userInputsModel = GameObject.FindGameObjectWithTag("UserInputsModel").GetComponent<UserInputsModel>();
            _musicInputsModel = GameObject.FindGameObjectWithTag("MusicInputsModel").GetComponent<MusicInputsModel>();

            SubscribeMusicInputs();
            SubscribeUserInputs();

            SetupFourInFour();

            _oneInEightAnimator = _oneInEightObject.GetComponent<Animator>();

            if (!_sceneVolume.sharedProfile.TryGet(out _sceneColorOverlayPostProcessVolume))
            {
                throw new NullReferenceException(nameof(_sceneColorOverlayPostProcessVolume));
            }

            _melodyKeyOriginalEmissiveColor = _melodyKeyRenderers[0].material.GetColor("_EmissiveColor");
            foreach (var item in _melodyKeyRenderers)
            {
                _lerpToTargetColorComponents.Add(item.gameObject.GetComponent<LerpToTargetColor>()); 
            }
        }

        private void OnDisable()
        {
            UnsubscribeMusicInputs();
            UnsubscribeUserInputs();
        }

        void Update()
        {
            Vector3 fourInFourElementPosition = _fourAndEightInFourElements[_fourInFourIndex].position;
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

            _fourAndEightInFourElements[_fourInFourIndex].position = fourInFourElementPosition;

            _fourAndEightInFourElements[_fourInFourIndex].transform.Rotate(Vector3.right * 50 * Time.deltaTime, Space.World);
            AnimateVolume();

            if (_musicInputsModel.OneInEightValue >= 0.7f && _prevOneInEightValue < 0.7f)
            {
                HandleOneInEightMusic();
            }
            _prevOneInEightValue = _musicInputsModel.OneInEightValue;
        }

        private void SetupFourInFour()
        {
            foreach (var obj in _fourAndEightInFourElements)
            {
                var mesh = obj.GetComponent<MeshFilter>().mesh;
                _fourInFourMeshes.Add(mesh);
                _fourInFourOriginalVertices.Add(mesh.vertices);
                _fourAndEightInFourOriginalX.Add(obj.position.x);
                _fourAndEightInFourOriginalY.Add(obj.position.y);
            }

            for (int i = 1; i < _fourAndEightInFourElements.Count; i++)
            {
                _fourAndEightInFourElements[i].gameObject.SetActive(false);
            }

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
            Vector3[] vertices = _fourInFourMeshes[_fourInFourIndex].vertices;
            for (int i = 0; i < vertices.Length; i++)
            {
                float perlinValue = Perlin.Noise(Time.timeSinceLevelLoad + (vertices[i].x * perlinFactor), Time.timeSinceLevelLoad + (vertices[i].y * perlinFactor), Time.timeSinceLevelLoad + (vertices[i].z * perlinFactor)) * .5f + 1.5f;

                var length = vertices[i].magnitude;
                var desiredLength = length * perlinValue;
                var weight = desiredLength / length;
                vertices[i] = weight * _fourInFourOriginalVertices[_fourInFourIndex][i];

            }
            _fourInFourMeshes[_fourInFourIndex].vertices = vertices;
        }

        #region music inputs


        public void SubscribeMusicInputs()
        {
            _musicInputsModel.EmitFourInFourEvent += HandleFourInFourMusicEvent;
            _musicInputsModel.EmitEightInFourEvent += HandleEightInFourMusicEvent;
            _musicInputsModel.EmitTwoInFourEvent += HandleTwoInFourMusicEvent;
            _musicInputsModel.EmitSixteenInFourEvent += HandleSixteenInFourMusicEvent;
            _musicInputsModel.EmitOneInFourEvent += HandleOneInFourMusicEvent;
        }

        public void UnsubscribeMusicInputs()
        {
            _musicInputsModel.EmitFourInFourEvent -= HandleFourInFourMusicEvent;
            _musicInputsModel.EmitEightInFourEvent -= HandleEightInFourMusicEvent;
            _musicInputsModel.EmitTwoInFourEvent -= HandleTwoInFourMusicEvent;
            _musicInputsModel.EmitSixteenInFourEvent -= HandleSixteenInFourMusicEvent;
            _musicInputsModel.EmitOneInFourEvent -= HandleOneInFourMusicEvent;
        }

        private void HandleOneInFourMusicEvent()
        {
            if (_userInputsModel.OneInFourUserInput.IsPressed)
            {
                _fourAndEightInFourElements[_fourInFourIndex].gameObject.SetActive(false);
                _fourInFourIndex = (_fourInFourIndex + 1) % _fourAndEightInFourElements.Count;
                _fourAndEightInFourElements[_fourInFourIndex].gameObject.SetActive(true);
            }
        }

        private void HandleOneInEightMusic()
        {
            if (_userInputsModel.OneInEightUserInput.IsPressed)
            {
                if (!_isCamMovingBack)
                {
                    _oneInEightAnimator.SetBool("isRotatingBack", false);
                    _oneInEightAnimator.SetFloat("backSpeedMultiplier", 0);
                    _oneInEightAnimator.Play("roate180deg");
                }
                else
                {
                    _oneInEightAnimator.SetFloat("backSpeedMultiplier", 1);
                }
                _isCamMovingBack = !_isCamMovingBack;
            }
        }

        private void HandleSixteenInFourMusicEvent()
        {
            if (_userInputsModel.SixteenInFourUserInput.IsPressed)
            {
                _sixteenInFourIndex = (_sixteenInFourIndex + 1) % (_sixteenInFourObjs.Count);
                _sixteenInFourFadeoutComponents[_sixteenInFourIndex].StartFadeout();
            }
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
            Vector3 originalScale = _fourAndEightInFourElements[_fourInFourIndex].transform.parent.localScale;

            _fourAndEightInFourElements[_fourInFourIndex].transform.parent.localScale = new Vector3(originalScale.x, originalScale.y + 2, originalScale.z);
            while (Vector3.Distance(_fourAndEightInFourElements[_fourInFourIndex].transform.parent.localScale, originalScale) > 0.1f)
            {
                var currentScale = _fourAndEightInFourElements[_fourInFourIndex].transform.parent.localScale;
                _fourAndEightInFourElements[_fourInFourIndex].transform.parent.localScale = new Vector3(currentScale.x, currentScale.y - 0.1f, currentScale.z);
                yield return null;
            }
            _fourAndEightInFourElements[_fourInFourIndex].transform.parent.localScale = originalScale;
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
            _userInputsModel.SixteenInFourUserInput.EmitTurnedOnOrOffEvent += HandleSixteenInFourUserInput;

            foreach (var key in _userInputsModel.DroneKeys.Keys)
            {
                key.EmitTurnedOnOrOffEvent += HandleDroneKey; ;
            }

            foreach (var key in _userInputsModel.MoodKeys.Keys)
            {
                key.EmitCollectionKeyTriggeredEvent += HandleMoodKey;
            }

            foreach (var key in _userInputsModel.MelodyKeys.Keys)
            {
                key.EmitTurnedOnOrOffEvent += HandleMelodyKey; ;
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
            _userInputsModel.SixteenInFourUserInput.EmitTurnedOnOrOffEvent -= HandleSixteenInFourUserInput;

            foreach (var key in _userInputsModel.DroneKeys.Keys)
            {
                key.EmitTurnedOnOrOffEvent -= HandleDroneKey; ;
            }

            foreach (var key in _userInputsModel.MoodKeys.Keys)
            {
                key.EmitCollectionKeyTriggeredEvent -= HandleMoodKey;
            }

            foreach (var key in _userInputsModel.MelodyKeys.Keys)
            {
                key.EmitTurnedOnOrOffEvent -= HandleMelodyKey; ;
            }
        }

        private void SetEmissiveColor(Material material, Color color, float colorFactor)
        {
            material.SetColor("_EmissiveColor", color * colorFactor);
        }

        private void HandleMelodyKey(bool hasTurnedOn, int index)
        {
            if (hasTurnedOn)
            {
                Color targetColor = _melodyKeyOriginalEmissiveColor * _melodyKeyTargetIntensity;
                SetEmissiveColor(_melodyKeyRenderers[index].material, targetColor, 1);
            } else
            {
                Color targetColor = _melodyKeyOriginalEmissiveColor * _melodyKeyTargetIntensity;
                _lerpToTargetColorComponents[index].PlayCoroutine(targetColor, _melodyKeyOriginalEmissiveColor, 1, SetEmissiveColor, _melodyKeyRenderers[index].material);
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

        private void HandleDroneKey(bool hasTurnedOn, int index)
        {
            if (hasTurnedOn)
            {
                var spawnPosition = new Vector3((UnityEngine.Random.value >= 0.5 ? 12 : -12), 0, 0);
                var _droneParent = Instantiate(_dronePrefab, spawnPosition, Quaternion.identity);
                _droneObjects[index] = _droneParent;
                for (int i = 0; i < _droneParent.transform.childCount; i++)
                {
                    var child = _droneParent.transform.GetChild(i);
                    child.gameObject.AddComponent<RotateThySelf>();
                    var rotatecomponent = child.gameObject.GetComponent<RotateThySelf>();
                    rotatecomponent.RotationSpeedFactor = 4 * i + 3;
                    rotatecomponent.RotationDirection = GetRotationVectors(index);

                    for (int j = 0; j < child.transform.childCount; j++)
                    {
                        var grandChild = child.transform.GetChild(j);
                        grandChild.gameObject.AddComponent<FadeOutQuick>();
                        var component = grandChild.gameObject.GetComponent<FadeOutQuick>();
                        component.ShouldDestroyItselfWhenDone = true;
                        component.ReductionSpeedFactor = 0.75f;
                    }
                }
            }
            else if (!hasTurnedOn)
            {
                var _droneParent = _droneObjects[index];

                for (int i = 0; i < _droneParent.transform.childCount; i++)
                {
                    var child = _droneParent.transform.GetChild(i);

                    for (int j = 0; j < child.transform.childCount; j++)
                    {
                        var grandChild = child.transform.GetChild(j);
                        var component = grandChild.gameObject.GetComponent<FadeOutQuick>();
                        component.StartFadeout();
                    }
                }
            }
        }

        private Vector3 GetRotationVectors(int index)
        {
            var right = new Vector3(1, 0, 0);
            var downRight = new Vector3(1, -1, 0);
            var down = new Vector3(0, -1, 0);
            var downLeft = new Vector3(-1, -1, 0);
            var left = new Vector3(-1, 0, 0);
            var upLeft = new Vector3(-1, 1, 0);
            var up = new Vector3(0, 1, 0);
            var upRight = new Vector3(1, 1, 0);

            var rotationVectors = new List<Vector3> {
                right,
                downRight,
                down,
                downLeft,
                left,
                upLeft,
                up,
                upRight,
            };

            return rotationVectors[index];
        }

        private void HandleSixteenInFourUserInput(bool hasTurnedOn)
        {
            if (hasTurnedOn)
            {
                if (_sixteenInFourObjs.Count == 0)
                {
                    // first time? create the objs
                    Vector3[] vertices = _sixteenInFourHelperObj.GetComponent<MeshFilter>().sharedMesh.vertices;
                    HashSet<Vector3> vertexPositions = new HashSet<Vector3>();
                    foreach (var vertex in vertices)
                    {
                        vertexPositions.Add(vertex * 600);
                    }

                    foreach (var position in vertexPositions)
                    {
                        var obj = Instantiate(_sixteenInFourPrefab, position, Quaternion.identity, _sixteenInFourParent);
                        _sixteenInFourObjs.Add(obj);
                        var fadeoutComponent = obj.GetComponent<FadeOutQuick>();
                        fadeoutComponent.ReductionSpeedFactor = 4f;
                        _sixteenInFourFadeoutComponents.Add(fadeoutComponent);
                    }

                    _sixteenInFourParent.gameObject.AddComponent<RotateThySelf>();

                }
                else
                {
                    foreach (var obj in _sixteenInFourObjs)
                    {
                        obj.SetActive(true);
                    }
                }

                _sixteenInFourParent.gameObject.GetComponent<RotateThySelf>().RotationDirection = UnityEngine.Random.onUnitSphere;
            }
            else
            {
                foreach (var obj in _sixteenInFourObjs)
                {
                    obj.SetActive(false);
                }
            }
        }

        private void HandleExplosionKey(int index)
        {
            Debug.Log(index);
            if (index >= 0 && index <= 3)
            {
                // drums
                float y = UnityEngine.Random.Range(-18f, 18f);
                float z = _isCamMovingBack ? -32 : 32;
                float x = -22;
                switch (index)
                {
                    case 0:
                        x = -61;
                        HandleExplosionDrumBehaviour(_explosionDrumPrefab, new Vector3(x, y, z));
                        break;
                    case 1:
                        x = -22;
                        HandleExplosionDrumBehaviour(_explosionDrumPrefab, new Vector3(x, y, z));
                        break;
                    case 2:
                        x = 22;
                        HandleExplosionDrumBehaviour(_explosionDrumPrefab, new Vector3(x, y, z));
                        break;
                    case 3:
                        x = 61;
                        HandleExplosionDrumBehaviour(_explosionDrumPrefab, new Vector3(x, y, z));
                        break;
                    default:
                        break;
                }

            }
            else
            {
                // big explosions

                for (var i = 1; i <= 8; i++)
                {
                    Vector3 spawnPosition = _fourAndEightInFourElements[_fourInFourIndex].position + GetOffsetVector(index, i);
                    var gameObjecto = Instantiate(_fourAndEightInFourElements[_fourInFourIndex].gameObject, spawnPosition, _fourAndEightInFourElements[_fourInFourIndex].rotation);
                    var rigidBody = gameObjecto.AddComponent<Rigidbody>();
                    rigidBody.useGravity = false;
                    rigidBody.velocity = GetOffsetVector(index, i);
                    gameObjecto.AddComponent<RotateThySelf>();
                    gameObjecto.AddComponent<FadeOutAndDestroyThySelf>();
                }
                for (var i = 1; i <= 8; i++)
                {
                    Vector3 spawnPosition = _fourAndEightInFourElements[_fourInFourIndex].position + GetOffsetVector(index, -i);
                    var gameObjecto = Instantiate(_fourAndEightInFourElements[_fourInFourIndex].gameObject, spawnPosition, _fourAndEightInFourElements[_fourInFourIndex].rotation);
                    var rigidBody = gameObjecto.AddComponent<Rigidbody>();
                    rigidBody.useGravity = false;
                    rigidBody.velocity = GetOffsetVector(index, -i);
                    gameObjecto.AddComponent<RotateThySelf>();
                    gameObjecto.AddComponent<FadeOutAndDestroyThySelf>();
                }

            }
        }

        private void HandleExplosionDrumBehaviour(GameObject prefab, Vector3 spawnPosition)
        {
            var obj = Instantiate(prefab, spawnPosition, Quaternion.identity, _oneInEightObject.transform);
            obj.AddComponent<RotateThySelf>();
            var rotateComponent = obj.GetComponent<RotateThySelf>();
            rotateComponent.RotationDirection = UnityEngine.Random.onUnitSphere;
            obj.AddComponent<FadeOutAndDestroyThySelf>();
            var fadeOutComponent = obj.GetComponent<FadeOutAndDestroyThySelf>();
            fadeOutComponent.TimeUntilDestroyInSeconds = 2;

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
                
            }
            else
            {
                _fourAndEightInFourElements[_fourInFourIndex].position = new Vector3(_fourAndEightInFourOriginalX[_fourInFourIndex], _fourAndEightInFourElements[_fourInFourIndex].position.y, _fourAndEightInFourElements[_fourInFourIndex].position.z);
            }
        }

        private void HandleEightInFourUserInput(bool hasTurnedOn)
        {
            if (hasTurnedOn)
            {
                
            }
            else
            {
                _fourAndEightInFourElements[_fourInFourIndex].position = new Vector3(_fourAndEightInFourElements[_fourInFourIndex].position.x, _fourAndEightInFourOriginalY[_fourInFourIndex], _fourAndEightInFourElements[_fourInFourIndex].position.z);
            }
        }
        #endregion
    }
}

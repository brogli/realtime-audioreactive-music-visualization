using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene1DroneKeyImplementation : MonoBehaviour
{


    public List<Transform> StartPositions = new();
    public List<Transform> TargetPositions = new();


    public GameObject DroneKeySpawnContainer;

    public GameObject Original;
    public float TimeBetweenEmissionsInSeconds;
    public int MaxParticlesPerDroneKey;
    public float MoveSpeed;

    private List<List<GameObject>> _allParticles = new();
    private List<int> _indexesOfObjectToMove = new();
    private List<bool> _isDroneKeyActiveIndex = new();
    private float _timeSinceLastEmissionInSeconds;
    private bool _haveMaxParticlesSpawned;
    private UserInputsModel _userInputsModel;



    // Start is called before the first frame update
    void Start()
    {
        _userInputsModel = GameObject.FindGameObjectWithTag("UserInputsModel").GetComponent<UserInputsModel>();

        for (int i = 0; i < 8; i++)
        {
            _allParticles.Add(new List<GameObject>());
            _indexesOfObjectToMove.Add(0);
            _isDroneKeyActiveIndex.Add(false);
        }

        for (int i = 0; i < 8; i++)
        {
            ToggleDroneKey(_userInputsModel.DroneKeys.Keys[i].IsPressed, i);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var step = MoveSpeed * Time.deltaTime;

        _timeSinceLastEmissionInSeconds += Time.deltaTime;
        TrySpawnParticle();

        MoveParticles(step);
    }

    private void MoveParticles(float step)
    {
        for (int i = 0; i < _allParticles.Count; i++)
        {

            foreach (var particle in _allParticles[i])
            {
                particle.transform.localPosition = Vector3.MoveTowards(particle.transform.localPosition, TargetPositions[i].localPosition, step);

            }
        }
    }

    private void TrySpawnParticle()
    {
        if (_timeSinceLastEmissionInSeconds > TimeBetweenEmissionsInSeconds)
        {
            // spawn
            _timeSinceLastEmissionInSeconds = 0;

            for (int i = 0; i < _allParticles.Count; i++)
            {

                if (_allParticles[i].Count >= MaxParticlesPerDroneKey)
                {
                    if (!_haveMaxParticlesSpawned)
                    {
                        _haveMaxParticlesSpawned = true;
                    }
                    var objectToMove = _allParticles[i][_indexesOfObjectToMove[i]];
                    _indexesOfObjectToMove[i] = (_indexesOfObjectToMove[i] + 1) % _allParticles[i].Count;
                    objectToMove.transform.localPosition = StartPositions[i].localPosition;
                }
                else
                {
                    GameObject particle = Instantiate(Original, StartPositions[i].position, Quaternion.identity);
                    particle.transform.SetParent(DroneKeySpawnContainer.transform, true);
                    if (!_isDroneKeyActiveIndex[i])
                    {
                        particle.SetActive(false);
                    }
                    _allParticles[i].Add(particle);
                }
            }
        }
    }

    public void ToggleDroneKey(bool hasTurnedOn, int index)
    {
        _isDroneKeyActiveIndex[index] = hasTurnedOn;
        _allParticles[index].ForEach(gameObject =>
        {
            gameObject.SetActive(hasTurnedOn);
        });
    }
}

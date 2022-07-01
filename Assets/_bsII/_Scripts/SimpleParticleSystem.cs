using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleParticleSystem : MonoBehaviour
{
    public GameObject Original;
    public Transform StartPosition;
    public Transform EndPosition;
    public GameObject Container;
    public int MaxParticles;
    public float MoveSpeed;
    public float TimeBetweenEmissionsInSeconds;
    private float _timeSinceLastEmissionInSeconds;
    private int _indexOfObjectToMove;

    public bool IsSpawningFinished { get; private set; }
    public List<GameObject> Particles { get; private set; }

    public void Start()
    {
        Particles = new();
    }

    public void FixedUpdate()
    {
        var step = MoveSpeed * Time.deltaTime;

        _timeSinceLastEmissionInSeconds += Time.deltaTime;
        TrySpawnParticle(TimeBetweenEmissionsInSeconds);

        MoveParticles(MoveSpeed);
    }


    private void MoveParticles(float step)
    {
        foreach (var particle in Particles)
        {
            particle.transform.localPosition = Vector3.MoveTowards(particle.transform.localPosition, EndPosition.position, step);
        }
    }

    private void TrySpawnParticle(float timeBetweenEmissionsInSeconds)
    {
        if (_timeSinceLastEmissionInSeconds > timeBetweenEmissionsInSeconds)
        {
            // spawn
            _timeSinceLastEmissionInSeconds = 0;

            if (Particles.Count >= MaxParticles)
            {
                IsSpawningFinished = true;
                var objectToMove = Particles[_indexOfObjectToMove];
                _indexOfObjectToMove = (_indexOfObjectToMove + 1) % Particles.Count;
                objectToMove.transform.localPosition = StartPosition.position;
            }
            else
            {
                GameObject particle = Instantiate(Original, StartPosition.position, Quaternion.identity);
                particle.transform.SetParent(Container.transform, true);

                // turn them off initially
                particle.SetActive(false);

                Particles.Add(particle);
            }
        }
    }
}

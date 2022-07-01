using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Scene1OneInSixteenImplementation : MonoBehaviour
{
    public GameObject ParticleSystemsContainer;
    public GameObject SpawnParent;
    private List<SimpleParticleSystem> _particleSystems;
    private bool _areParticlesReady = false;
    private readonly List<GameObject> _particles = new(); // "Triangle" GameObjects containing a core and a border
    private MusicValuesModel _musicValuesModel;
    private List<int> indicesOfObjectsToShow;
    private readonly System.Random random = new();


    // Start is called before the first frame update
    void Start()
    {
        _particleSystems = ParticleSystemsContainer.GetComponents<SimpleParticleSystem>().ToList();
        _musicValuesModel = GameObject.FindGameObjectWithTag("MusicValuesModel").GetComponent<MusicValuesModel>();
        SubscribeToMusicInputs();
    }

    public void OnDisable()
    {
        UnsubscribeToMusicInput();
    }
    private void SubscribeToMusicInputs()
    {
        _musicValuesModel.EmitSixteenInFourEvent += SelectRandomIndices;
    }

    private void UnsubscribeToMusicInput()
    {
        _musicValuesModel.EmitSixteenInFourEvent -= SelectRandomIndices;
    }

    private void SelectRandomIndices()
    {
        if (indicesOfObjectsToShow != null)
        {
            foreach (int index in indicesOfObjectsToShow)
            {
                _particles[index].SetActive(false);
            }

        }
        int amountOfNumbers = _particles.Count;
        indicesOfObjectsToShow = Enumerable.Range(0, amountOfNumbers)
                                     .Select(i => new Tuple<int, int>(random.Next(amountOfNumbers), i))
                                     .OrderBy(i => i.Item1)
                                     .Select(i => i.Item2)
                                     .Take(_particles.Count / 5)
                                     .ToList();

        foreach (int index in indicesOfObjectsToShow)
        {
            _particles[index].SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_areParticlesReady)
        {
            AnimateParticles();
        }
        else
        {
            TrySetupParticles();
        }
    }

    private void AnimateParticles()
    {
        var sixteenInFourValue = 1f - Easings.EaseInCubic(_musicValuesModel.SixteenInFourValue);
        foreach (int index in indicesOfObjectsToShow)
        {
            _particles[index].transform.GetComponentsInChildren<Renderer>().ToList().ForEach(renderer =>
            {
                renderer.sharedMaterial.SetColor("_EmissiveColor", Color.white * (sixteenInFourValue) * 4);
                renderer.sharedMaterial.SetColor("_BaseColor", new Color(0, 0, 0, (sixteenInFourValue) * 4));
            });
        }
    }

    private void TrySetupParticles()
    {
        if (_particleSystems[0].IsSpawningFinished && _particleSystems[1].IsSpawningFinished && !_areParticlesReady)
        {
            _areParticlesReady = true;
            
            foreach (var system in _particleSystems)
            {
                system.Particles.ForEach(particle =>
                {
                    _particles.Add(particle.transform.GetChild(0).gameObject);
                    _particles.Add(particle.transform.GetChild(1).gameObject);
                    particle.SetActive(true);
                    particle.transform.GetChild(0).gameObject.SetActive(false);
                    particle.transform.GetChild(1).gameObject.SetActive(false);
                });
            }
        }
    }

    public void ToggleSixteenInFour(bool isNowActive)
    {
        SpawnParent.SetActive(isNowActive); 
    }
}

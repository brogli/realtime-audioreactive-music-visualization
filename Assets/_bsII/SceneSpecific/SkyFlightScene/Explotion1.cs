using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class Explotion1 : MonoBehaviour
{
    public AnimationCurve effectCurve1;
    public Camera mainCamera;

    private float _duration;
    private float _scale;
    private float _fovDefault;

    private bool _isCoroutineRunning = false;
    // Start is called before the first frame update
    void Start()
    {
        _fovDefault = mainCamera.fieldOfView;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayCoroutine(float duration, float scale)
    {
        _duration = duration;
        _scale = scale;

        if (_isCoroutineRunning)
        {
            StopCoroutine("Explotion1Coroutine");
            _isCoroutineRunning = false;
        }
        StartCoroutine("Explotion1Coroutine");
    }

    private IEnumerator Explotion1Coroutine()
    {
        _isCoroutineRunning = true;

        float journey = 0f;

        mainCamera.fieldOfView = _fovDefault;
        while (journey <= _duration)
        {
            journey = journey + Time.deltaTime;
            float percent = Mathf.Clamp01(journey / _duration);

            Debug.Log(effectCurve1.Evaluate(percent));
            mainCamera.fieldOfView = _fovDefault + _scale * effectCurve1.Evaluate(percent);

            yield return null;
        }
        mainCamera.fieldOfView = _fovDefault;

        _isCoroutineRunning = false;

    }
}
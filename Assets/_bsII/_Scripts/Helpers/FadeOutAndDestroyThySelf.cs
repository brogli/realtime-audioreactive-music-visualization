using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Waits for "ShouldWaitWithFadingOut" to be false (which is default), then starts fading out. 
/// It tries to fade out in the amount of seconds set in "TimeUntilDestroyInSeconds"
/// </summary>
public class FadeOutAndDestroyThySelf : MonoBehaviour
{
    public bool ShouldWaitWithFadingOut { private get; set; } = false;
    public float TimeUntilDestroyInSeconds { get; set; } = 5;
    private Color _originalColor = Color.red;
    private bool _ableToRetreiveColor = true;
    private float _interpolationValue = 0;
    private float _frameTimeSum = 0;
    private float _amountOfFramesPassed = 0;
    private Material _material;
    private string _colorName = "_BaseColor";

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, TimeUntilDestroyInSeconds);
        if (TryGetComponent(out Renderer renderer))
        {
            try
            {
                _originalColor = renderer.material.GetColor(_colorName);
                _material = renderer.material;
            }
            catch (Exception)
            {
                Debug.LogWarning($"Couldn't retrieve color {_colorName}");
                _ableToRetreiveColor = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_ableToRetreiveColor && !ShouldWaitWithFadingOut)
        {
            Color currentColor = _material.GetColor(_colorName);
            float newAlpha = Mathf.Lerp(_originalColor.a, 0, _interpolationValue);
            Color newColor = new Color(currentColor.r, currentColor.g, currentColor.b, newAlpha);
            _material.SetColor(_colorName, newColor);

            _frameTimeSum += Time.deltaTime;
            _amountOfFramesPassed += 1;
            float averageFrameTime = _frameTimeSum / _amountOfFramesPassed;
            float timeLeft = TimeUntilDestroyInSeconds - _frameTimeSum;

            // -3 frames so we surely have blended out _before_ we destroy:
            float estimatedAmountOfFramesLeft = (timeLeft / averageFrameTime) - 3;

            float increment = (1 - _interpolationValue) / estimatedAmountOfFramesLeft;
            _interpolationValue += increment;
        }
    }
}

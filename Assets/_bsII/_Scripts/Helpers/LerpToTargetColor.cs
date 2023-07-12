using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpToTargetColor : MonoBehaviour
{

    private Color _originalColor = Color.white;
    private Color _targetColor = Color.black;
    private float _speedFactor = 1f;
    Action<Material, Color, float> _actionToCall;
    private Material _materialToWriteTo;

    private bool _isCoroutineRunning = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayCoroutine(Color originalColor, Color targetColor, float speedFactor, Action<Material, Color, float> actionToCall, Material materialToWriteTo)
    {
        _originalColor = originalColor;
        _targetColor = targetColor;
        _speedFactor = speedFactor;
        _actionToCall = actionToCall;
        _materialToWriteTo = materialToWriteTo;

        if (_isCoroutineRunning)
        {
            StopCoroutine("FadeCoroutine");
            _isCoroutineRunning = false;
        }
        StartCoroutine("FadeCoroutine");
    }

    public void PlayCoroutine(Material materialToWriteTo)
    {
        PlayCoroutine(_originalColor, _targetColor, _speedFactor, DefaultAction, materialToWriteTo);
    }

    private IEnumerator FadeCoroutine()
    {
        _isCoroutineRunning = true;
        var t = 0f;

        while (t < 1)
        {
            t += Time.deltaTime * _speedFactor;

            if (t > 1)
            {
                t = 1;
            }

            Color newColor = Color.Lerp(_originalColor, _targetColor, t);
            _actionToCall.Invoke(_materialToWriteTo, newColor, 1);

            yield return null;
        }
        _isCoroutineRunning = false;

    }

    private void DefaultAction(Material material, Color color, float colorFactor)
    {
        material.SetColor("_BaseColor", color * colorFactor);
    }
}

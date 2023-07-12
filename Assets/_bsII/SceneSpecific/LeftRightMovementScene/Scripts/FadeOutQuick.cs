using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOutQuick : MonoBehaviour
{
    public bool ShouldDestroyItselfWhenDone { private get; set; } = false;
    public float ReductionSpeedFactor { get; set; } = 1f;
    public bool IsRunning { get; private set; } = false;
    private Material _material;
    // Start is called before the first frame update
    void Start()
    {
        _material = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsRunning)
        {
            Color currentColor = _material.GetColor("_BaseColor");
            Color newColor = new Color(currentColor.r, currentColor.g, currentColor.b, currentColor.a - ReductionSpeedFactor * Time.deltaTime);

            _material.SetColor("_BaseColor", newColor);

            if (newColor.a < 0.01f)
            {
                StopFadeout();
            }
        }
    }

    private void StopFadeout()
    {
        _material.SetColor("_BaseColor", new Color(0, 0, 0, 0));
        IsRunning = false;
        if (ShouldDestroyItselfWhenDone)
        {
            Destroy(this.gameObject);
        }
    }

    public void StartFadeout()
    {
        _material.SetColor("_BaseColor", new Color(0, 0, 0, 1));
        IsRunning = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOutAndDestroyThySelf : MonoBehaviour
{
    public float TimeUntilDestroy { get; set; } = 5;
    private Color _originalColor = Color.red;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, TimeUntilDestroy);
        if (TryGetComponent(out Renderer renderer))
        {
            try
            {
                _originalColor = renderer.material.GetColor("_BaseColor");
            }
            catch (System.Exception)
            {
                Debug.LogWarning("Couldn't retrieve color _BaseColor");
            }
        }
        Debug.Log(_originalColor);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

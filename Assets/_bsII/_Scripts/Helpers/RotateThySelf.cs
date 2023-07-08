using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateThySelf : MonoBehaviour
{
    public Vector3 RotationDirection { get; set; } = Vector3.right;
    public float RotationSpeedFactor { get; set; } = 50;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(RotationDirection * Time.deltaTime * RotationSpeedFactor, Space.World);
    }
}

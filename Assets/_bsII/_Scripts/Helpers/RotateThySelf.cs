using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateThySelf : MonoBehaviour
{
    [SerializeField]
    private Vector3 _rotationDirection = Vector3.right;
    [SerializeField]
    private float _rotationSpeedFactor = 50;

    public Vector3 RotationDirection { get => _rotationDirection; set => _rotationDirection = value; }
    public float RotationSpeedFactor { get => _rotationSpeedFactor; set => _rotationSpeedFactor = value; }
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

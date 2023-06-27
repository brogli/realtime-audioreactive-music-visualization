using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TestingMotionTime : MonoBehaviour
{
    [SerializeField]
    private float _motionTime;

    private Animator _animator;
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        _animator.SetFloat("motionTime", _motionTime);
    }
}

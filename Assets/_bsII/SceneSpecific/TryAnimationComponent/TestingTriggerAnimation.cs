using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingTriggerAnimation : MonoBehaviour
{
    private Animator _animation;
    // Start is called before the first frame update
    void Start()
    {
        _animation = GetComponent<Animator>();
        //_animation.Play()
        _animation.Play("Base Layer.test-clip01");
        _animation.SetFloat("speedMultiplier", 0);
        //_animation.Play("layer0.test-clip02");
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeSinceLevelLoad > 5)
        {
            _animation.SetFloat("speedMultiplier", 1);
        }
    }
}

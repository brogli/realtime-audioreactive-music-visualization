using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene1ExplosionType2Test : MonoBehaviour
{
    public GameObject explosionType2;
    public Vector3 spawnPosition;

    public bool toggle = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (toggle)
        {
            toggle = false;
            DoToggle();
        }   
    }

    private void DoToggle()
    {
        Instantiate(explosionType2, spawnPosition, Quaternion.identity);
    }
}

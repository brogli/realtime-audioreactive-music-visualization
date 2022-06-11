using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateOnAwake : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

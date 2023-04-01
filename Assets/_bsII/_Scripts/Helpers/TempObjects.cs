using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempObjects : MonoBehaviour
{
    void Awake()
    {
        Destroy(this.gameObject);
    }
}

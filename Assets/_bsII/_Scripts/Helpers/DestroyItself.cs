using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyItself : MonoBehaviour
{
    public void DestroyItSelf()
    {
        Destroy(this.transform.parent.gameObject);
    }
}

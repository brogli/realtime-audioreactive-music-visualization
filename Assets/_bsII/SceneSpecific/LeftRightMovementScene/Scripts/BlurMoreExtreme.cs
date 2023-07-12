using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class BlurMoreExtreme : MonoBehaviour
{
    DepthOfField dofComponent;

    // Start is called before the first frame update
    void Start()
    {
        Volume volume = gameObject.GetComponent<Volume>();
        DepthOfField tmp;
        if (volume.profile.TryGet(out tmp))
        {
            dofComponent = tmp;
            //Debug.Log("success");
            //Debug.Log(dofComponent.farMaxBlur);
            dofComponent.farMaxBlur = 100f;
            //dofComponent.over = 100f;
            //dofComponent.farFocusStart =
            //Debug.Log(dofComponent.farMaxBlur);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

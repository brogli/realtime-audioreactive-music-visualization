using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class MainCamInitializor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var mainCam = GameObject.FindGameObjectWithTag("MainCamera");
        VolumeProfile profile = mainCam.GetComponent<Volume>().sharedProfile;
        if (!profile.TryGet<CameraCopy>(out var camCopy))
        {
            camCopy = profile.Add<CameraCopy>();
        }

        camCopy.active = true;
        camCopy.intensity.value = 1.0f;
        camCopy.intensity.overrideState = true;
    }
}

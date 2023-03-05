using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenHandler : MonoBehaviour
{
    private const float middleBetween32to9and16to9 = 2.666f;

    public bool Has2ndDisplay { get; private set; }
    public bool Is16to9 { get; private set; }
    public bool Is32to9 { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        TryActivate2ndScreen();
        ValidateResolutions();
        SetRenderingResolutionOnPrimaryDisplay();
    }

    private void ValidateResolutions()
    {
        if (Display.displays[0].systemWidth < 1280 || Display.displays[0].systemHeight < 720)
        {
            throw new NotSupportedException("System requires at least a resolution of at least 1280*720.");
        }
    }

    private void TryActivate2ndScreen()
    {
        Debug.Log("displays connected: " + Display.displays.Length);

        if (Display.displays.Length < 2)
        {
            Debug.Log("Only 1 display connected");
            return;
        }

        if (Display.displays.Length == 2)
        {
            Has2ndDisplay = true;
            Debug.Log("activating second display ");
            Display.displays[1].Activate();
            
        }
        if (Display.displays.Length > 2)
        {
            throw new NotSupportedException("System does support a maximum of 2 screens, more were detected.");
        }
    }

    private void SetRenderingResolutionOnPrimaryDisplay()
    {
        var nativeAspectRatio = Display.displays[0].systemWidth / Display.displays[0].systemHeight;

        if (nativeAspectRatio > middleBetween32to9and16to9)
        {
            Is32to9 = true;
            Debug.Log("Setting 32:9 resolution");
            Screen.SetResolution(2560, 720, true);
        } else
        {
            Is16to9 = true;
            Debug.Log("Setting 16:9 resolution");
            Screen.SetResolution(1280, 720, true);
        }
    }
}

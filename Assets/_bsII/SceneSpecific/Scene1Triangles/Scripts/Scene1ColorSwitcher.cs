using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class Scene1ColorSwitcher : MonoBehaviour
{
    public GameObject SomeFourInFour;
    public GameObject SomeEightInFour;
    public GameObject SomeTwoInFour;
    public GameObject SomeMelodyObject;
    public GameObject SomeDronekeyObject;
    public LocalVolumetricFog VolumetricFog;

    private Camera _mainCamera;
    private Material _melodyKeysSharedMaterial;
    private Material _droneKeysSharedMaterial;
    private float _defaultColorIntensity = 8f;

    public Color FourInFourColor { get; private set; } = Color.white;
    public Color EightInFourColor { get; private set; } = Color.white;
    public Color TwoInFourColor { get; private set; } = Color.white;
    public Color SixteenInFourColor { get; private set; } = Color.white;
    public Color OneInFourColor { get; private set; } = Color.white;


    private List<List<Color>> _colorPalettes;
    private List<List<(float, string)>> _intensityHexColorpairs;
    // Start is called before the first frame update
    void Start()
    {
        _mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        _melodyKeysSharedMaterial = SomeMelodyObject.GetComponent<Renderer>().sharedMaterial;
        _droneKeysSharedMaterial = SomeDronekeyObject.GetComponent<Renderer>().sharedMaterial;

        InitializeColorCodes();
        InitializeColorPalettes();
        ResetToDefaultColors();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SwitchColors(int index)
    {
        if (index == 0)
        {
            ResetToDefaultColors();
        }
        else
        {
            _mainCamera.GetComponent<HDAdditionalCameraData>().backgroundColorHDR = _colorPalettes[index - 1][0];
            TwoInFourColor = _colorPalettes[index - 1][1];
            FourInFourColor = _colorPalettes[index - 1][2];
            EightInFourColor = _colorPalettes[index - 1][3];
            SixteenInFourColor = _colorPalettes[index - 1][4];
            _melodyKeysSharedMaterial.SetColor("_EmissiveColor", _colorPalettes[index - 1][5]);
            _droneKeysSharedMaterial.SetColor("_EmissiveColor", _colorPalettes[index - 1][6]);
            VolumetricFog.parameters.albedo = _colorPalettes[index - 1][7];
            OneInFourColor = _colorPalettes[index - 1][8];
        }
    }

    public void OnDisable()
    {
        ResetToDefaultColors();
    }

    private void ResetToDefaultColors()
    {
        if (_mainCamera != null)
        {
            _mainCamera.GetComponent<HDAdditionalCameraData>().backgroundColorHDR = Color.black;
        }
        FourInFourColor = Color.white * _defaultColorIntensity;
        EightInFourColor = Color.white * _defaultColorIntensity;
        TwoInFourColor = Color.white * _defaultColorIntensity;
        SixteenInFourColor = Color.white * _defaultColorIntensity;
        VolumetricFog.parameters.albedo = Color.white;
        _melodyKeysSharedMaterial.SetColor("_EmissiveColor", Color.white * _defaultColorIntensity);
        _droneKeysSharedMaterial.SetColor("_EmissiveColor", Color.white * _defaultColorIntensity);
        OneInFourColor = Color.white * _defaultColorIntensity * 2;
    }

    private void InitializeColorCodes()
    {
        // camera/background, 2-4, 4-4, 8-4, 16-4, melody, drone, fog, 1-4
        _intensityHexColorpairs = new()
        {                               // cam/bg          2-4                  4-4                  8-4                  16-4                melody             drone                fog                1-4
            new List<(float, string)>{ (1f, "#421C76"), (8.66f, "#FB5607"), (20.09f, "#FF006E"), (14.86f, "#3A86FF"), (8.66f, "#FB5607"), (6.45f, "#FFBE0B"), (15.09f, "#FF006E"), (1f, "#3A86FF"), (50.09f, "#FF006E") },
            new List<(float, string)>{ (1f, "#300820"), (30.9f, "#0F4C5C"), (30.9f, "#0F4C5C"), (21.56f, "#E36414") , (21.56f, "#E36414"), (16.76f, "#9A031E"), (30.9f, "#0F4C5C"), (1f, "#9A031E"),  (51.56f, "#E36414") },
            new List<(float, string)>{ (1f, "#0E1A2C"), (11.7f, "#FCCA46"), (19.5f, "#FE7F2D"), (14.5f, "#A1C181"), (11.7f, "#FCCA46"), (9.0f, "#619B8A"), (15.5f, "#FE7F2D"), (1f, "#FCCA46"), (46.76f, "#9A031E") },
            new List<(float, string)>{ (1f, "#1D3557"), (20.0f, "#457B9D"), (13.02f, "#F1FAEE"), (9.0f, "#A8DADC"), (20.0f, "#457B9D"), (9.0f, "#E63946"), (9.0f, "#E63946"), (1f, "#F1FAEE"), (9.0f, "#E63946") },
            new List<(float, string)>{ (1f, "#003D35"), (17.26f, "#9B5DE5"), (10.0f, "#FEE440"), (13.0f, "F15BB5"), (8.66f, "#FB5607"), (11.82f, "#00BBF9"), (10.0f, "#FEE440"), (1f, "#F1FAEE"), (60.0f, "#FEE440") },
            new List<(float, string)>{ (1f, "#1C004C"), (13.35f, "#E00081"), (13.35f, "#E0A600"), (13.48f, "#E04900"), (8.05f, "#FFBD00"), (13.35f, "#E0A600"), (29.09f, "#9E0059"), (1f, "#0099FF"), (50f, "#0099FF") },
            new List<(float, string)>{ (1f, "#0A3004"), (9.26f, "#08BDBD"), (17.75f, "#FF9914"), (13.04f, "#F21B3F"), (9.26f, "#08BDBD"), (4.88f, "#ABFF4F"), (17.75f, "#FF9914"), (1f, "#ABFF4F"), (43.04f, "#F21B3F") }
        };

    }

    private void InitializeColorPalettes()
    {
        _colorPalettes = new();
        foreach (List<(float, string)> intensityColorCombos in _intensityHexColorpairs)
        {
            List<Color> colorPalette = new();
            foreach ((float intensity, string hexColor) intensityColorCombo in intensityColorCombos)
            {
                Color color;
                ColorUtility.TryParseHtmlString(intensityColorCombo.hexColor, out color);
                color *= intensityColorCombo.intensity;
                colorPalette.Add(color);
            }
            _colorPalettes.Add(colorPalette);
        }
    }
}

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

    private Camera _mainCamera;
    private Material _fourInFourSharedMaterial;
    private Material _eightInFourSharedMaterial;
    private Material _twoInFourSharedMaterial;
    private Material _melodyKeysSharedMaterial;

    public Color BackgroundColor { get; private set; } = Color.white;
    public Color FourInFourColor { get; private set; } = Color.white;
    public Color EightInFourColor { get; private set; } = Color.white;
    public Color MelodyKeyColor { get; private set; } = Color.white;
    public Color TwoInFourColor { get; private set; } = Color.white;

    private List<List<Color>> _colorPalettes;
    private List<Color> _defaultColorPalette;
    private List<List<string>> _colorCodes;
    // Start is called before the first frame update
    void Start()
    {
        _mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        _fourInFourSharedMaterial = SomeFourInFour.GetComponent<Renderer>().sharedMaterial;
        _eightInFourSharedMaterial = SomeEightInFour.GetComponent<Renderer>().sharedMaterial;
        _twoInFourSharedMaterial = SomeTwoInFour.GetComponent<Renderer>().sharedMaterial;
        _melodyKeysSharedMaterial = SomeMelodyObject.GetComponent<Renderer>().sharedMaterial;

        InitializeColorCodes();
        InitializeColorPalettes();
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
            _mainCamera.GetComponent<HDAdditionalCameraData>().backgroundColorHDR = _colorPalettes[index][0];
            FourInFourColor = _colorPalettes[index][1];
            EightInFourColor = _colorPalettes[index][2];
            _melodyKeysSharedMaterial.SetColor("_EmissiveColor", _colorPalettes[index][3] * 4);
            TwoInFourColor = _colorPalettes[index][4];
        }
    }

    public void OnDisable()
    {
        ResetToDefaultColors();
    }

    private void ResetToDefaultColors()
    {
        _mainCamera.GetComponent<HDAdditionalCameraData>().backgroundColorHDR = Color.black;
        FourInFourColor = Color.white;
        EightInFourColor = Color.white;
        MelodyKeyColor = Color.white;
        TwoInFourColor = Color.white;
    }

    private void InitializeColorCodes()
    {
        _colorCodes = new()
        {
            new List<string>()
        {
            "#264653", "#2a9d8f", "#e9c46a", "#f4a261", "#e76f51"
        },
            new List<string>()
        {
            "#606c38", "#283618", "#fefae0", "#dda15e", "#bc6c25"
        },
            new List<string>()
        {
            "#e63946", "#f1faee", "#a8dadc", "#457b9d", "#1d3557"
        },
            new List<string>()
        {
            "#003049", "#d62828", "#f77f00", "#fcbf49", "#eae2b7"
        },
            new List<string>()
        {
            "#8ecae6", "#219ebc", "#023047", "#ffb703", "#fb8500"
        },
            new List<string>()
        {
            "#ffbe0b", "#fb5607", "#ff006e", "#8338ec", "#3a86ff"
        },
            new List<string>()
        {
            "#ef476f", "#ffd166", "#06d6a0", "#118ab2", "#073b4c"
        },
            new List<string>()
        {
            "#9b5de5", "#f15bb5", "#fee440", "#00bbf9", "#00f5d4"
        },
            new List<string>()
        {
            "#ff595e", "#ffca3a", "#8ac926", "#1982c4", "#6a4c93"
        },
            new List<string>()
        {
            "#390099", "#9e0059", "#ff0054", "#ff5400", "#ffbd00"
        },
            new List<string>()
        {
            "#0c0f0a", "#ff206e", "#fbff12", "#41ead4", "#ffffff"
        },
            new List<string>()
        {
            "#780000", "#c1121f", "#fdf0d5", "#003049", "#669bbc"
        },
            new List<string>()
        {
            "#386641", "#6a994e", "#a7c957", "#f2e8cf", "#bc4749"
        },
            new List<string>()
        {
            "#23c9ff", "#7cc6fe", "#ccd5ff", "#e7bbe3", "#c884a6"
        },
            new List<string>()
        {
            "#262626", "#acbfa4", "#e2e8ce", "#ff7f11", "#ff1b1c"
        },
            new List<string>()
        {
            "#042a2b", "#5eb1bf", "#cdedf6", "#ef7b45", "#d84727"
        }
        };
    }

    private void InitializeColorPalettes()
    {
        _defaultColorPalette = new()
        {
            Color.black,
            Color.white,
            Color.white,
            Color.white,
            Color.white
        };

        _colorPalettes = new();
        foreach (List<string> colorCodeGroup in _colorCodes)
        {
            List<Color> colorPalette = new();
            foreach (string hexColor in colorCodeGroup)
            {
                Color color;
                ColorUtility.TryParseHtmlString(hexColor, out color);
                colorPalette.Add(color);
            }
            _colorPalettes.Add(colorPalette);
        }
    }
}

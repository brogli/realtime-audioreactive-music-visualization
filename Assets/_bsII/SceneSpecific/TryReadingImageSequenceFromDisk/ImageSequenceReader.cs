using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ImageSequenceReader : MonoBehaviour
{
    [SerializeField]
    private int _currentFrameNumber = 0;
    private int _lastFrameNumber = 0;

    Texture2D[] _imageSequence;

    private Material _material;
    // Start is called before the first frame update
    async void Start()
    {
        string folderPath = Application.persistentDataPath + "/" + "image-sequences" + "/" + "test-sequence";
        string[] files = Directory.GetFiles(folderPath);


        Texture2D[] imageSequence = new Texture2D[files.Length];

        _material = GetComponent<Renderer>().sharedMaterial;

        for (int i = 0; i < files.Length; i++)
        {
            imageSequence[i] = await LoadFileAsync(files[i]);
        }

        _imageSequence = imageSequence;
    }

    // Update is called once per frame
    void Update()
    {
        if (_currentFrameNumber != _lastFrameNumber && _currentFrameNumber < _imageSequence.Length && _currentFrameNumber >= 0)
        {
            if (_imageSequence != null && _imageSequence[_currentFrameNumber] != null)
            {
                _material.mainTexture = _imageSequence[_currentFrameNumber];
            }
            _lastFrameNumber = _currentFrameNumber;
        }
    }

    private Texture2D LoadPNG(string filePath)
    {

        Texture2D tex = null;
        byte[] fileData;

        if (File.Exists(filePath))
        {
            fileData = File.ReadAllBytes(filePath);
            tex = new Texture2D(2, 2);
            tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
        }
        return tex;
    }

    private async Task<Texture2D> LoadFileAsync(string filePath)
    {
        byte[] result;
        Texture2D tex = null;

        using (FileStream SourceStream = File.Open(filePath, FileMode.Open))
        {
            result = new byte[SourceStream.Length];
            await SourceStream.ReadAsync(result, 0, (int)SourceStream.Length);
        }
        tex = new Texture2D(2, 2);
        tex.LoadImage(result); //..this will auto-resize the texture dimensions.
        return tex;
    }
}

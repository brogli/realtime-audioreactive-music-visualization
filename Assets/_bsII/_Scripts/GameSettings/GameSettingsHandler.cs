using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public class GameSettingsHandler : MonoBehaviour
{
    public GameSettings GameSettings { get; private set; }
    private string _gameSettingsPath;
    private const string _gameSettingsFileName = "GameSettings.json";
    private const string _defaultGameSettingsName = "DefaultGameSettings";
    private UserInputsModel _userInputsModel;

    public void Awake()
    {
        _gameSettingsPath = Application.persistentDataPath + "/" + _gameSettingsFileName;
    }

    public void Start()
    {
        _userInputsModel = GameObject.FindGameObjectWithTag("UserInputsModel").GetComponent<UserInputsModel>();
        _userInputsModel.ReloadGameSettings.EmitTriggerEvent += ReloadGameSettings;
        _userInputsModel.ResetGameSettingsToDefaults.EmitTriggerEvent += ResetGameSettingsToDefault;
        ReloadGameSettings();
        
    }

    public void OnDisable()
    {
        _userInputsModel.ReloadGameSettings.EmitTriggerEvent -= ReloadGameSettings;
        _userInputsModel.ResetGameSettingsToDefaults.EmitTriggerEvent -= ResetGameSettingsToDefault;
    }

    public void Update()
    {
    }

    public async void ReloadGameSettings()
    {
        Debug.Log("Reloading Game Settings");
        if (DoesFileExist(_gameSettingsPath))
        {
            string jsonString = await ReadFromFile(_gameSettingsPath);
            GameSettings = JsonConvert.DeserializeObject<GameSettings>(jsonString);
        }
        else
        {
            string defaultGameSettingsJson = ReadFromDefaults();
            WriteToFile(defaultGameSettingsJson, _gameSettingsPath);
            GameSettings = JsonConvert.DeserializeObject<GameSettings>(defaultGameSettingsJson);
        }
        Debug.Log(GameSettings);
    }

    public void ResetGameSettingsToDefault()
    {
        Debug.Log("Resetting game settings to default");
        string defaultGameSettingsJson = ReadFromDefaults();
        WriteToFile(defaultGameSettingsJson, _gameSettingsPath);
        ReloadGameSettings();
    }

    private string ReadFromDefaults()
    {
        return Resources.Load<TextAsset>(_defaultGameSettingsName).text;
    }

    private async Task<string> ReadFromFile(string path)
    {
        using (StreamReader reader = new(path))
        {
            string fileText = await reader.ReadToEndAsync();
            return fileText;
        }
    }

    private bool DoesFileExist(string path)
    {
        return File.Exists(path);
    }

    private async void WriteToFile(string jsonString, string path)
    {
        using (StreamWriter writer = new(path))
        {
            await writer.WriteAsync(jsonString);
        }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public class GameSettingsHandler : MonoBehaviour, IUserInputsConsumer
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
        SubscribeUserInputs();
        ReloadGameSettings();

    }

    public void OnDisable()
    {
        UnsubscribeUserInputs();
    }
    
    public void SubscribeUserInputs()
    {
        _userInputsModel = GameObject.FindGameObjectWithTag("UserInputsModel").GetComponent<UserInputsModel>();
        _userInputsModel.ReloadGameSettings.EmitKeyTriggeredEvent += ReloadGameSettings;
        _userInputsModel.ResetGameSettingsToDefaults.EmitKeyTriggeredEvent += ResetGameSettingsToDefault;
    }

    public void UnsubscribeUserInputs()
    {
        _userInputsModel.ReloadGameSettings.EmitKeyTriggeredEvent -= ReloadGameSettings;
        _userInputsModel.ResetGameSettingsToDefaults.EmitKeyTriggeredEvent -= ResetGameSettingsToDefault;
    }

    public void Update()
    {
    }

    public async void ReloadGameSettings()
    {
        Debug.Log("Reloading Game Settings");
        if (DoesFileExist(_gameSettingsPath))
        {
            Debug.Log("File does exist, reading values");
            string jsonString = await ReadFromFile(_gameSettingsPath);
            GameSettings = JsonConvert.DeserializeObject<GameSettings>(jsonString);
        }
        else
        {
            Debug.Log("File does not exist, writing and reading from defaults");
            string defaultGameSettingsJson = ReadFromDefaults();
            WriteToFile(defaultGameSettingsJson, _gameSettingsPath);
            GameSettings = JsonConvert.DeserializeObject<GameSettings>(defaultGameSettingsJson);
        }
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
        string fileText;

        using (StreamReader reader = new(path))
        {
            fileText = await reader.ReadToEndAsync();
        }
        return fileText;
    }

    private bool DoesFileExist(string path)
    {
        return File.Exists(path);
    }

    private void WriteToFile(string jsonString, string path)
    {
        using (StreamWriter writer = new(path))
        {
            writer.Write(jsonString);
        }
    }

}

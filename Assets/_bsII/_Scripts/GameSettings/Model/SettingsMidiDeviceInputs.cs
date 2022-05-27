using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SettingsMidiDeviceInputs
{
    public Dictionary<string, SettingsNoteInput> noteInputs;
    public Dictionary<string, SettingsCcInput> ccInputs;
}

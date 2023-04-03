using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// If nested is true, then "containerTargetProperty" represents the name of a property in MusicInputsModel of type "containerType". This variable
/// contains a nested property of name "targetProperty" and type "type".
/// If nested is false, then targetProperty represents directly the name of a property in MusicInputsModel.
/// A SettingsCcInput value in the GameSettingsJson is usually associated with a key consisting of midichannel_ccNumber, e.g. 2_32 for channel 2, cc 32.
/// </summary>
[System.Serializable]
public class SettingsCcInput
{
    public string targetProperty;
    public InputType type;
    public bool nested;
    public InputType containerType;
    public string containerTargetProperty;
}

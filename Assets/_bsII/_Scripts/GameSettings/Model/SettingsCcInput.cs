using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SettingsCcInput
{
    public string targetProperty;
    public InputType type;
    public bool nested;
    public InputType containerType;
    public string containerTargetProperty;
}

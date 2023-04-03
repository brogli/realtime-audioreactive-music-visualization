using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A SettingsNoteInput value in the GameSettingsJson is usually associated with a key consisting of midichannel_noteNumber, e.g. 2_32 for channel 2, note 32.
/// The "targetProperty" is used to pull the property of same name in the MusicInputsModel. "type" represents the type of the targetProperty or the type of its
/// elements if it's a collection. If "collection" is true, then the "targetProperty" points to a collection and "targetIndex" is used to associate the correct
/// element of the collection.
/// </summary>
[System.Serializable]
public class SettingsNoteInput
{
    public bool collection;
    public string targetProperty;
    public InputType type;
    public int targetIndex;
}

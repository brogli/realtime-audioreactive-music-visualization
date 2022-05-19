using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteProcessor : MonoBehaviour
{
    private DeviceToLogicalInputMapper _deviceToLogicalInputMapper;

    // Start is called before the first frame update
    void Start()
    {
        _deviceToLogicalInputMapper = GameObject.FindGameObjectWithTag("DeviceToLogicalInputMapper").GetComponent<DeviceToLogicalInputMapper>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ProcessNoteOn(byte channel, byte note, byte velocity)
    {
        _deviceToLogicalInputMapper.GetLogicalInputByChannelAndNote(channel, note).SetNewStateIfNecessary(true, velocity / 127.0f);

    }

    public void ProcessNoteOff(byte channel, byte note)
    {
        _deviceToLogicalInputMapper.GetLogicalInputByChannelAndNote(channel, note).SetNewStateIfNecessary(false, 0);
    }
}

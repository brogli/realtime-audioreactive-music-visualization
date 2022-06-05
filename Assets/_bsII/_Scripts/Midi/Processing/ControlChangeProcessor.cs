using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlChangeProcessor : MonoBehaviour
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

    internal void ProcessControlChange(byte channel, byte number, byte value)
    {
        _deviceToLogicalInputMapper.GetLogicalInputByChannelAndCcNumber(channel, number)?.SetNewStateIfNecessary(value > 1, value / 127.0f);
    }
}
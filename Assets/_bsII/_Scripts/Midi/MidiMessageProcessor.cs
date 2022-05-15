using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MidiMessageProcessor : MonoBehaviour
{
    private BeatclockProcessor _beatclockProcessor;

    // Start is called before the first frame update
    void Start()
    {
        _beatclockProcessor = GameObject.FindGameObjectWithTag("BeatclockProcessor").GetComponent<BeatclockProcessor>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal void ProcessNoteOn(string name, byte channel, byte note, byte velocity)
    {
        Debug.Log("note on");
    }

    internal void ProcessNoteOff(string name, byte channel, byte note)
    {
        Debug.Log("note off");
    }

    internal void ProcessControlChange(string name, byte channel, byte number, byte value)
    {
        Debug.Log("cc change");
    }

    internal void ProcessBeatclock()
    {
        _beatclockProcessor.ProcessBeatclock();
    }
}

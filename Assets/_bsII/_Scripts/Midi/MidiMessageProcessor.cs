using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MidiMessageProcessor : MonoBehaviour
{
    private BeatclockProcessor _beatclockProcessor;
    private NoteProcessor _noteProcessor;
    private ControlChangeProcessor _controlChangeProcessor;

    // Start is called before the first frame update
    void Start()
    {
        _beatclockProcessor = GameObject.FindGameObjectWithTag("BeatclockProcessor").GetComponent<BeatclockProcessor>();
        _noteProcessor = GameObject.FindGameObjectWithTag("NoteProcessor").GetComponent<NoteProcessor>();
        _controlChangeProcessor = GameObject.FindGameObjectWithTag("ControlChangeProcessor").GetComponent<ControlChangeProcessor>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal void ProcessNoteOn(string name, byte channel, byte note, byte velocity)
    {
        _noteProcessor.ProcessNoteOn(channel, note, velocity);
    }

    internal void ProcessNoteOff(string name, byte channel, byte note)
    {
        _noteProcessor.ProcessNoteOff(channel, note);
    }

    internal void ProcessControlChange(string name, byte channel, byte number, byte value)
    {
        _controlChangeProcessor.ProcessControlChange(channel, number, value);
    }

    internal void ProcessBeatclock()
    {
        _beatclockProcessor.ProcessBeatclock();
    }
}

using UnityEngine;
using System.Collections.Generic;
using RtMidi.LowLevel;

// This class is almost entirely copied from "MidiInTest.cs" in https://github.com/keijiro/jp.keijiro.rtmidi/tree/master/Assets
// where it is released under an MIT-like licence.
sealed class MidiMessageReceiver : MonoBehaviour
{
    #region Private members

    MidiProbe _probe;
    List<MidiInPort> _ports = new List<MidiInPort>();
    private MidiMessageProcessor _midiMessageProcessor;

    // Does the port seem real or not?
    // This is mainly used on Linux (ALSA) to filter automatically generated
    // virtual ports.
    bool IsRealPort(string name)
    {
        return !name.Contains("Through") && !name.Contains("RtMidi");
    }

    // Scan and open all the available output ports.
    void ScanPorts()
    {
        for (var i = 0; i < _probe.PortCount; i++)
        {
            var name = _probe.GetPortName(i);
            Debug.Log("MIDI-in port found: " + name);

            _ports.Add(IsRealPort(name) ? new MidiInPort(i)
            {
                OnNoteOn = (byte channel, byte note, byte velocity) =>
                    {
                        Debug.Log(string.Format("{0} channel: [{1}] On {2} ({3})", name, channel, note, velocity));
                        _midiMessageProcessor.ProcessNoteOn(name, channel, note, velocity);
                    },

                OnNoteOff = (byte channel, byte note) =>
                    {
                        Debug.Log(string.Format("{0} channel: [{1}] Off {2}", name, channel, note));
                        _midiMessageProcessor.ProcessNoteOff(name, channel, note);
                    },

                OnControlChange = (byte channel, byte number, byte value) =>
                    {
                        //Debug.Log(string.Format("{0} channel: [{1}] CC {2} ({3})", name, channel, number, value));
                        _midiMessageProcessor.ProcessControlChange(name, channel, number, value);
                    },
                OnClockMessgeReceived = (byte channel) =>
                   {
                       //Debug.Log(string.Format("{0} channel: {1}, CLOCK RECEIVED", name, channel));
                       _midiMessageProcessor.ProcessBeatclock();
                   }
            } : null
            );
        }
    }

    // Close and release all the opened ports.
    void DisposePorts()
    {
        foreach (var p in _ports) p?.Dispose();
        _ports.Clear();
    }

    #endregion

    #region MonoBehaviour implementation

    void Start()
    {
        _midiMessageProcessor = GameObject.FindGameObjectWithTag("MidiMessageProcessor").GetComponent<MidiMessageProcessor>();
        _probe = new MidiProbe(MidiProbe.Mode.In);
    }

    void Update()
    {
        // Rescan when the number of ports changed.
        if (_ports.Count != _probe.PortCount)
        {
            Debug.Log("Rescanning Midi Ports");
            DisposePorts();
            ScanPorts();
        }

        // Process queued messages in the opened ports.
        foreach (var p in _ports) p?.ProcessMessages();
    }

    void OnDestroy()
    {
        _probe?.Dispose();
        DisposePorts();
    }

    #endregion
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// The code of this class resulted from following along the tutorial series "Audio Visualization - Unity/C# Tutorial [...]"
/// https://www.youtube.com/watch?v=5pmoP1ZOoNs&list=PL3POsQzaCw53p2tA6AWf7_AWgplskR0Vo&index=1, and the series
/// "Microphone Input Visuals - Unity/C# Tutorial [...]" https://www.youtube.com/watch?v=rnZ52SlVJj8&list=PL3POsQzaCw50AsSQBoVi9efaz8cP3d8ho
/// both created by Peter Olthof from the Peer Play youtube channel.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class FftProcessor : MonoBehaviour
{
    //mic stuff
    public AudioClip AudioClip;
    public bool UseMicrophone;
    private readonly int _micNumber = 0;
    private string _selectedMicDevice;
    public AudioMixerGroup MixerGroupMicrophone, MixerGroupMaster;

    //constants
    public float AverageVolumeMultiplier = 50;
    public float LowFrequencyVolumeMultiplier = 200;



    private AudioSource _audioSource;
    private MusicValuesModel _musicValuesModel;
    private float[] AudioSamples = new float[1024];
    private float[] FrequencyBand = new float[8];
    private float CustomLowFrequencies;
    private float AverageVolume;

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _musicValuesModel = GameObject.FindGameObjectWithTag("MusicValuesModel").GetComponent<MusicValuesModel>();

        //mic stuff
        if (UseMicrophone)
        {
            if (Microphone.devices.Length > 0)
            {
                foreach (var microphone in Microphone.devices)
                {
                    Debug.Log(microphone);
                }
                _selectedMicDevice = Microphone.devices[_micNumber].ToString();
                Debug.Log("selecting always first one: " + _selectedMicDevice);
                _audioSource.outputAudioMixerGroup = MixerGroupMicrophone;
                _audioSource.clip = Microphone.Start(_selectedMicDevice, true, 1, AudioSettings.outputSampleRate);
                //_audioSource.loop = true;
                while (!(Microphone.GetPosition(null) > 0)) { }
                _audioSource.Play();
            }
            else
            {
                UseMicrophone = false;
            }
        }
        else
        {
            _audioSource.outputAudioMixerGroup = MixerGroupMaster;
            _audioSource.clip = AudioClip;
            _audioSource.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        GetSpectrumAudioSource();
        MakeFrequencyBands();
        GetCustomLowFrequencies();
        CalculateAverageVolume();
    }


    private void GetSpectrumAudioSource()
    {
        _audioSource.GetSpectrumData(AudioSamples, 0, FFTWindow.Triangle);
    }

    private void GetCustomLowFrequencies()
    {
        CustomLowFrequencies = ((AudioSamples[0] + AudioSamples[1] + AudioSamples[2] + AudioSamples[3] + AudioSamples[4] + AudioSamples[5] +
            AudioSamples[6] + AudioSamples[7] + AudioSamples[8] + AudioSamples[9] + AudioSamples[10] + AudioSamples[11]) / 12) * LowFrequencyVolumeMultiplier;
        _musicValuesModel.LowFrequencyVolume = CustomLowFrequencies;
    }

    private void CalculateAverageVolume()
    {
        float newAverageVolume = 0;
        for (int i = 0; i < FrequencyBand.Length; i++)
        {
            newAverageVolume += FrequencyBand[i];
        }
        AverageVolume = newAverageVolume;
        _musicValuesModel.AverageVolumeRaw = AverageVolume;
    }

    private void MakeFrequencyBands()
    {
        int count = 0;
        for (int i = 0; i < 8; i++)
        {
            float average = 0;
            int sampleCount = (int)Mathf.Pow(2, i) * 2;

            if (i == 7)
            {
                sampleCount += 2;
            }
            for (int j = 0; j < sampleCount; j++)
            {
                average += AudioSamples[count] * (count + 1);
                count++;
            }
            average /= count;
            FrequencyBand[i] = average * AverageVolumeMultiplier;
        }
    }

}

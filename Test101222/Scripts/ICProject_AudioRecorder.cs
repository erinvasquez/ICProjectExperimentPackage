using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using UnityEngine.Audio;


[RequireComponent(typeof(AudioSource))]
/// <summary>
/// NOTES:
/// https://forum.unity.com/threads/blow-detection-using-ios-microphone.118215/
/// 
/// 
/// </summary>
public class ICProject_AudioRecorder : MonoBehaviour {
    // Start recording with built-in Microphone and play the recorded audio right away
    public float[] mySamples;
    public float minThreshold = 0.0f;
    
    public float frequency = 0.0f;
    public MusicNote currentNote;

    public float currentAveragedVolume = 0.0f;


    public int recordingTimeInSeconds = 60 * 30;
    private int numberOfSamples = 8192;
    public int channels = 2;
    public int audioSampleRate;// = AudioSettings.outputSampleRate; // 44100 otherwise?
    public int rateDivider = 2;

    public string microphoneName;
    public FFTWindow fftWindow;

    public bool MonitorAudioOn = true;

    //private List<string> options = new List<string>();
    private AudioSource microphoneAudioSource;
    private AudioClip audioClip;
    public AudioMixer masterMixer;

    public string filename;
    public DateTime saveNow;
    public DateTime saveUtcNow;
    public CSVWriter excelWriter;

    // public ExperimentList currentExperiments;
    //public ExperimentLog currentLog;

    public ExperimentIDSO currentExperimentID;


    void Start() {

        RequestPermission();

        // Get microphone audio source from this game object
        microphoneAudioSource = GetComponent<AudioSource>();
        string name = Microphone.devices[0];
        Debug.Log($"Microphone 0 is {name}");


        currentNote = new MusicNote(SharpNotes.C, 0);


        // set samples
        audioSampleRate = AudioSettings.outputSampleRate / rateDivider;
        mySamples = new float[AudioSettings.outputSampleRate / rateDivider];

        // Find our CSV Writer
        excelWriter = GameObject.Find("CSVWriter").GetComponent<CSVWriter>();


        // set our default filename
        saveNow = DateTime.Now;
        saveUtcNow = DateTime.UtcNow;
        filename = $"Experiment Recording";



        microphoneAudioSource.loop = true;

        // TODO: replace this with a button press or use it somewhere else maybe?

    }

    public void Update() {

        //GetAudioOutputData();

        if (Microphone.IsRecording(microphoneName)) {
            while (!(Microphone.GetPosition(microphoneName) > 0)) {
            }

            currentAveragedVolume = GetAveragedVolume();
            frequency = GetFundamentalFrequency();
            currentNote.SetMusicNoteFromFrequency(frequency);




        } else {
            // Microphone doesn't work
        }


    }


    public void SetFileName(string _desiredFilename) {
        filename = _desiredFilename;
    }

    public string GetFilename() {
        return filename;
    }


    public void GetAudioOutputData() {

        if (Microphone.IsRecording(Microphone.devices[0])) {
            while (!(Microphone.GetPosition(Microphone.devices[0]) > 0)) {
                Debug.Log("Microphone.GetPosition !> 0");
            }

            microphoneAudioSource.GetOutputData(mySamples, channels);

        }

    }

    public void Test_Save() {
        Debug.Log($"Attempting save recording under {filename}");
        SavWav.Save(filename, audioClip);
        microphoneAudioSource.Stop();
        Stop();

    }

    ///////////////////////////////////////////////////////////////////////////////////////////////

    /// <summary>
    /// 
    /// </summary>
    public void ExportAudio() {
        Debug.Log($"Attempting to save recording under {filename}");
        
        //SavWav.TrimSilence(audioClip, 0.5f);

        SavWav.Save(filename, audioClip);


    }

    /// <summary>
    /// 
    /// </summary>
    private void PlayRecordedAudio() {
        if (audioClip == null)
            return;

        microphoneAudioSource.clip = audioClip;
        microphoneAudioSource.Play();
    }

    /// <summary>
    /// 
    /// </summary>
    public void Stop() {

        if (!HasConnectedMicDevices()) {
            return;
        }

        if (!IsRecordingNow(microphoneName)) {
            return;
        }

        microphoneAudioSource.Stop();
        Microphone.End(microphoneName);

    }

    /// <summary>
    /// 
    /// </summary>
    public void Record() {
        Debug.Log("Starting record");

        if (!HasConnectedMicDevices()) {
            return;
        }

        filename = $"Experiment Recording {currentExperimentID.ID}";

        UpdateMicrophone();
        //Debug.Log("Recording started with " + microphoneName);

        microphoneAudioSource.clip = audioClip;

        if (MonitorAudioOn) {
            microphoneAudioSource.Play();
        } else {
            microphoneAudioSource.Stop();
        }

        microphoneAudioSource.loop = true;

    }





    /// <summary>
    /// 
    /// </summary>
    public void RequestPermission() {
        // CusromMicrophone.RequestMicrophonePermission();
    }

    /// <summary>
    /// 
    /// </summary>
    void UpdateMicrophone() {
        // Make sure our audiosource is stopped first
        microphoneAudioSource.Stop();

        // Start recording to audioclip from the mic
        audioClip = Microphone.Start(microphoneName, true, recordingTimeInSeconds, audioSampleRate);
        microphoneAudioSource.clip = audioClip;

        // "Mute the sound with an Audio Mixer group because we don't want the player to hear it"
        if (Microphone.IsRecording(microphoneName)) {

            while (!(Microphone.GetPosition(microphoneName) > 0)) {
            } // "Wait until the recording has started"

            // Start playing the audio source?
            // audioSource.Play();

        } else {
            Debug.Log(microphoneName + " doesn't work!");
        }


    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static bool HasConnectedMicDevices() {
        return Microphone.devices.Length > 0;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="deviceName"></param>
    /// <returns></returns>
    public static bool IsRecordingNow(string deviceName) {
        return Microphone.IsRecording(deviceName);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public float GetAveragedVolume() {
        return GetAveragedVolume(256);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_samples"></param>
    /// <returns></returns>
    public float GetAveragedVolume(int _samples) {
        float[] data = new float[_samples];
        float a = 0;

        microphoneAudioSource.GetOutputData(data, 0);

        foreach (float s in data) {
            a += Mathf.Abs(s);
        }

        return a / _samples;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public float GetFundamentalFrequency() {

        float fundamentalFrequency = 0.0f;
        float[] data = new float[numberOfSamples];
        
        microphoneAudioSource.GetSpectrumData(data, 0, fftWindow);
        
        float s = 0.0f;
        int i = 0;
        for (int j = 1; j < numberOfSamples; j++) {
            if (data[j] > minThreshold) // volumn must meet minimum threshold
            {
                if (s < data[j]) {
                    s = data[j];
                    i = j;
                }
            }
        }
        
        fundamentalFrequency = i * audioSampleRate / numberOfSamples;

        return fundamentalFrequency;
    }

}
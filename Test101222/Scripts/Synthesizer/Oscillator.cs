using System.Collections;
using UnityEngine;

/// <summary>
/// 
/// VR friendly Version of our oscillator
/// 
/// Things to do:
/// - add, divide, multiply, average different waveforms together
/// - Add sliders/knobs to control multipliers to frequency/mix ratios
///   of different waveforms
/// - use rng to select new frequencies after certain periods of time
/// - make more sounds
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class Oscillator : MonoBehaviour {

    /// <summary>
    /// In Hz, the current note being produced
    /// </summary>
    [SerializeField]
    float frequency = 440f;

    /// <summary>
    /// Phase increment based on sampling frequency
    /// </summary>
    private double increment;

    /// <summary>
    /// Stays between 0 and 2 PI,
    /// phase for our sin wave
    /// 
    /// Used as a float value sometimes
    /// </summary>
    private double phase;

    /// <summary>
    /// Unity defaults to 48,000 
    /// </summary>
    private double sampling_frequency = 48000.0;

    /// <summary>
    /// How loud it is basically, but not really
    /// moves between -1 and 1?
    /// </summary>
    float gain;
    public float volume = 0.2f;

    private Waveforms waveform = Waveforms.SinWave;
    Waveforms currentWaveform;

    /// <summary>
    /// The current MusicNote this oscillator is set to try to play
    /// </summary>
    public MusicNote currentNote;

    /// <summary>
    /// True if the oscillator is to play
    /// a sound
    /// </summary>
    private bool isPlaying = false;

    private void Start() {
        currentWaveform = waveform;
        currentNote = new MusicNote(SharpNotes.D, 2); // I like D2 as our default note, just cause
        frequency = currentNote.GetETFrequency();

        gain = volume;

        Debug.Log("Starting frequency: " + currentNote.noteName.ToString() + currentNote.octave + " " + currentNote.GetETFrequency().ToString() + "Hz");


    }

    private void OnValidate() {
        currentWaveform = waveform;

    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="freq"></param>
    /// <param name="v"></param>
    public void StartPlay(float freq, float v) {
        frequency = freq;
        volume = v;
        isPlaying = true;
    }

    /// <summary>
    /// Called when we press play.
    /// Sets isPlaying to true while using the previous
    /// frequency and volume
    /// </summary>
    public void StartPlay() {

        isPlaying = true;

    }

    /// <summary>
    /// Start playing at a certain volume
    /// </summary>
    /// <param name="_volume"></param>
    public void StartPlay(float _volume) {
        volume = _volume;
        isPlaying = true;
    }

    /// <summary>
    /// Called when we've released our play button,
    /// sets isPlaying to false to stop our oscillator
    /// </summary>
    public void EndPlay() {

        isPlaying = false;

        //gain = 0;
    }

    public void TogglePlay() {
        isPlaying = !isPlaying;
    }

    public bool IsPlaying() {
        return isPlaying;
    }

    /// <summary>
    /// Set this oscillator's frequency
    /// </summary>
    /// <param name="f"></param>
    public void SetFrequency(float f) {
        frequency = f;
    }

    /// <summary>
    /// Return our oscillator's current frequency
    /// </summary>
    /// <returns></returns>
    public double GetFrequency() {
        return frequency;
    }

    public void SetNote(MusicNote _note) {
        currentNote = _note;
    }

    public MusicNote GetMusicNote() {
        return currentNote;
    }


    /// <summary>
    /// Set our oscillator's gain
    /// </summary>
    /// <param name="g"></param>
    public void SetGain(float g) {
        gain = g;
    }

    /// <summary>
    /// Returns our osicllator's gain
    /// </summary>
    /// <returns></returns>
    public float GetGain() {
        return gain;
    }

    public void SetVolume(float v) {
        volume = v;
    }

    public float GetVolume() {
        return volume;
    }

    /// <summary>
    /// Get our gain in decibels
    /// </summary>
    /// <returns></returns>
    public float GetdB() {
        return 20f * Mathf.Log10((float)gain);
    }

    /// <summary>
    /// Set the waveform to be used by our oscillator
    /// </summary>
    /// <param name="form"></param>
    public void SetWaveform(int form) {
        SetWaveform((Waveforms)form);
    }

    /// <summary>
    /// Set the waveform to be used by our oscillator
    /// </summary>
    /// <param name="form"></param>
    public void SetWaveform(Waveforms form) {
        currentWaveform = form;
    }

    /// <summary>
    /// Get the waveform type this oscillator
    /// currently uses.
    /// </summary>
    /// <returns></returns>
    public Waveforms GetWaveform() {
        return currentWaveform;
    }

    /// <summary>
    /// Called on the audio thread, (not the main one)
    /// Unity uses this to "insert a custom filter
    /// into the audio DSP chain". I'm guessing that means
    /// we make audio happen.
    /// 
    /// "Called every time a chunk of audio is sent to the filter
    /// (~20ms or so depending on sample rate and platform), an array of floats
    /// from [-1f,1f]" "Can procedurally generate audio"
    /// </summary>
    /// <param name="_data"></param>
    /// <param name="_channels"></param>
    private void OnAudioFilterRead(float[] _data, int _channels) {
        // this code run every 1/samplerate seconds

        if (!isPlaying) {
            return;
        }
        _data = ProcessMonoAudioData(_data, _channels);




    }

    private float[] ProcessMonoAudioData(float[] _data, int _channels) {
        increment = frequency * 2 * Mathf.PI / sampling_frequency;

        for (int sampleIndex = 0; sampleIndex < _data.Length; sampleIndex += _channels) {

            phase += increment;

            // Apply our frequency and phase to a certain waveform equation

            // Add our Sin Wave data to our current sample using our current frequency
            _data[sampleIndex] = GetSinWaveform();

            // if we have two channels, copy to the next before increment
            if (_channels == 2) {
                _data[sampleIndex + 1] += _data[sampleIndex];
            }

            // Reset phase to 0 when it reaches 2 pi
            if (phase > (Mathf.PI * 2)) {
                phase = 0.0;
            }

        }

        return _data;
    }



    // Converts our frequency to angular velocity w (or omega)
    /// <summary>
    /// Get our frequency as an angular velocity
    /// </summary>
    /// <param name="freq"></param>
    /// <returns></returns>
    float AngularVelocity(float freq) {
        return freq * 2.0f * Mathf.PI;
    }

    /// <summary>
    /// Gets a sinewave
    /// </summary>
    /// <returns>A float</returns>
    float GetSinWaveform() {

        // phase is just set to 2 PI * frequency/sampling frequency

        return gain * Mathf.Sin((float)phase);
    }

    /// <summary>
    /// "Sounds like ann old nintendo"
    /// </summary>
    /// <returns></returns>
    float GetSquareWaveform() {

        if (gain * GetSinWaveform() >= 0 * gain) {
            return gain * 0.6f;
        } else {
            return -gain * 0.6f;
        }

    }

    /// <summary>
    /// "Squeaky and Quacky"
    /// Gets a triangle wave
    /// </summary>
    /// <returns></returns>
    float GetTriangleWaveform() {

        return gain * Mathf.PingPong((float)phase, 1.0f);

    }

    float GetSawWaveform() {
        // y = 2A/pi * (sigma n = 1 to s (-sin((n) * f * 2 * pi * x)) / n)
        // where S is the number of sin waves
        // A is amplitude
        // f is the frequency
        // OR

        double dOutput = 0.0;

        for (double n = 1.0; n < 100.0; n++) {
            dOutput += (GetSinWaveform() * frequency) / n;
        }

        return (float)dOutput * (2.0f / Mathf.PI);
    }

    float GetHarshSawWaveform() {
        // y= 2A/pi * (f * pi mod(1.0/f) - pi/2)

        return (2f / Mathf.PI) * ((float)frequency * (float)Mathf.PI * ((float)phase % (1f / (float)frequency) - ((float)Mathf.PI / 2f)));

    }

    float AddWaves(float _wave1, float _wave2) {
        return _wave1 + _wave2;
    }

}

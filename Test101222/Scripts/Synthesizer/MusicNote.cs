using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// "A set of all pitches that a whole
/// number of octaves apart"
/// </summary>
[SerializeField]
public class MusicNote {

    /// <summary>
    /// Our note's letter name, ex.
    /// "DSharp" or "F". We're keeping everything to sharps for now,
    /// since using flats later can be confusing
    /// </summary>
    public SharpNotes noteName;

    /// <summary>
    /// Current octave for this note
    /// </summary>
    public int octave;

    /// <summary>
    /// The frequency for this pitch at the current octave
    /// </summary>
    private float equalTemperamentfrequency;

    public MusicNote(SharpNotes name, int octaveNumber) {
        noteName = name;
        octave = octaveNumber;

        equalTemperamentfrequency = GetETFrequency();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pitch"></param>
    /// <returns></returns>
    public bool IsGreaterThan(MusicNote pitch) {

        // if our octave is greater, the frequency is greater
        if (octave > pitch.octave) {
            return true;
        } else if (octave == pitch.octave) {
            // else if the octave is the same,
            // we're only greater if our note name is

            if (noteName > pitch.noteName) {
                return true;
            }

        }

        // If our octave is less than our observed one, we're definitely not greater

        return false;
    }

    /// <summary>
    /// to calculate our Equal Temperament frequency,
    /// we need to ifnd how many half steps from A4
    /// we are
    /// </summary>
    /// <returns></returns>
    public HalfStepsFromA4 GetHalfStepsFromA4() {

        return (HalfStepsFromA4)System.Enum.Parse(typeof(HalfStepsFromA4), noteName.ToString().ToUpper() + octave.ToString());

    }

    /// <summary>
    /// Get the music note 1 Half Step
    /// (1 semitone) up
    /// </summary>
    /// <returns></returns>
    public MusicNote GetHalfStepUp() {
        int note = (int)noteName;
        int oct = octave;

        // DO NOT GET anything higher than C8
        // DO NOT GET CSHARP 8

        if (note == (int)SharpNotes.B) {
            // note is on the next octave, reset to C

            note = (int)SharpNotes.C;
            oct++;
        } else if (note == (int) SharpNotes.C && oct == 8){
            // do nothing, this is max



        } else {
            note++;
        }

        return new MusicNote((SharpNotes)note, oct);
    }

    public MusicNote GetHalfStepDown() {
        int note = (int)noteName;
        int oct = octave;

        if (note == (int)SharpNotes.C) {

            // if octave is 0, this is the lowest note
            if (oct == 0) {
                return this;
            } else {
                // note is on the octave below, reset to B
                note = (int)SharpNotes.B;
                oct--;
            }

        } else {
            // just lower the note name and keep octave
            note--;
        }

        return new MusicNote((SharpNotes)note, oct);

    }

    /// <summary>
    /// Get the music note 1 whole step,
    /// or 2 Half steps (semitones) up
    /// </summary>
    /// <returns></returns>
    public MusicNote GetWholeStepUp() {
        int note = (int)noteName;
        int oct = octave;

        // If this note is SharpNotes.GSHARP, up the octave
        if (noteName == SharpNotes.B) {
            note = (int)SharpNotes.CSHARP;
            oct++;

        } else if (noteName == SharpNotes.ASHARP) {
            // G too, since we move two half steps to our A of next octave
            note = (int)SharpNotes.C;
            oct++;

        } else {
            // upping this note by 2 wont affect octaves of notes under ASHARP or B
            note += 2;
        }

        return new MusicNote((SharpNotes)note, oct);
    }

    public void SetMusicNoteFromFrequency(float _frequency) {
        MusicNote tempNote = new MusicNote(SharpNotes.C, 0); // start with lowest note

        bool haveNote = false;



        while (!haveNote) {
            float tempFreq = tempNote.GetETFrequency();
            // check if our frequency is above our temp note

            // if we've reached our last note, then it's just C8
            if (tempNote.noteName == SharpNotes.C && tempNote.octave == 8) {
                //haveNote = true;
                break;
            }

            if (_frequency > tempFreq) {
                // frequency is larger than our temp frequency
                //Debug.Log($"{_frequency} > {tempFreq}");

                // now check if it's lower than the next after temp
                if (_frequency < tempNote.GetHalfStepUp().GetETFrequency()) {
                    // if it is,
                    //  check if frequency is closer to temp frequency or next note frequency
                    //  get difference between frequency and temp frequency
                    //  get difference between next note frequency and frequency
                    //  whichever difference is smaller, set next temp note to that note
                    float tempDiff = _frequency - tempFreq;
                    float nextHighestDiff = tempNote.GetHalfStepUp().GetETFrequency() - _frequency;

                    //Debug.Log($"{_frequency} - {tempFreq} = {tempDiff}");

                    // if the note is way off, nextHighestDiff will be negative
                    if (nextHighestDiff < 0) {
                        //Debug.Log($"Frequency {_frequency} is higher than temp note and next highest note, getting next temp note");
                        tempNote = tempNote.GetHalfStepUp();
                        continue;
                    }

                    if (tempDiff < nextHighestDiff) {
                        // lower difference is smaller,
                        // frequency is above temp note but closer to it than the next one
                        // I think we have our note 
                        haveNote = true;
                        //Debug.Log($"frequency {_frequency} closer to temp note under it than next temp note, finished");

                    } else {
                        // higher difference is smaller, set our temp note to the higher note and continue
                        tempNote = tempNote.GetHalfStepUp();
                        //Debug.Log($"Frequency {_frequency} is closer to next temp note than temp note, getting next up");

                    }



                } else {
                    // frequency isn't lower than next temp note
                    //  set our new temp note to this next note, and continue
                    tempNote = tempNote.GetHalfStepUp();
                    //Debug.Log($"Frequency {_frequency} isn't lower than next temp note, getting next up");
                }


            } else {
                // frequency is less than temp frequency,
                //Debug.Log($"{_frequency} < {tempFreq}");

                haveNote = true;

            }

        }





        Debug.Log($"Setting noteName to {tempNote.noteName} and octave to {tempNote.octave}");
        noteName = tempNote.noteName;
        octave = tempNote.octave;


    }


    /// <summary>
    /// Gets equal temprament frequency for this pitch
    /// with an octave of 0
    /// </summary>
    /// <param name="steps">Number of half steps away from A4</param>
    /// <returns></returns>
    public float GetETFrequency() {
        int aForForty = 440;

        float a = Mathf.Pow(2f, (1f / 12f));


        return (float)aForForty * Mathf.Pow(a, (float)GetHalfStepsFromA4());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public int GetOctave() {
        return octave;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_octave"></param>
    public void setOctave(int _octave) {
        octave = _octave;
    }

    /// <summary>
    /// Get the MIDI note number
    /// </summary>
    /// <returns></returns>
    public float GetP() {
        return 9 + (12 * Mathf.Log(equalTemperamentfrequency, 2f) / 440f);
    }

    public string GetName() {
        return noteName.ToString();
    }

    public override string ToString() {

        return noteName.ToString() + octave;
    }

    /// <summary>
    /// Get this note as a SHARP note mask
    /// </summary>
    /// <returns></returns>
    public int ToNoteMask() {
        // Get the bit to represent this note
        // A is 0, so should represent
        // 0b100000000000
        int result = (int)System.Enum.Parse(typeof(SharpNoteBits), noteName.ToString());

        return result;
    }
}

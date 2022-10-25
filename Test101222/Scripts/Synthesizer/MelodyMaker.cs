using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MelodyMaker : MonoBehaviour {

    [SerializeField]
    public MajorScale currentScale;
    Oscillator oscillator;
    public MusicNote currentNote;


    /// <summary>
    /// The amount of time in seconds between changing notes
    /// </summary>
    private float waitTime;


    public double BPM = 85.0;
    public float volume = 0.2f;


    private IEnumerator myCoroutine;

    // Start is called before the first frame update
    void Start() {
        //currentScale = new MajorScale(new MusicNote(SharpNotes.D, 2));
        oscillator = GetComponent<Oscillator>();
        currentNote = currentScale.tonic;

        //oscillator.StartPlay(currentNote.GetETFrequency(), 1f);
        oscillator.StartPlay();

        Debug.Log(currentScale);

        waitTime = (float)(60f / (BPM));

        //myCoroutine = NoteCoroutine();
        StartCoroutine(NoteCoroutine());
    }

    // Update is called once per frame
    void Update() {


        //StartCoroutine(myCoroutine);

    }


    private IEnumerator NoteCoroutine() {

        while (true) {

            Debug.Log("NoteCoroutine: " + (int)Time.time);

            currentNote = currentScale.GetRandomNote();
            oscillator.SetFrequency(currentNote.GetETFrequency());
            oscillator.SetVolume(volume);
            //oscillator.StartPlay(currentNote.GetETFrequency(), volume);

            yield return new WaitForSeconds(waitTime);
        }


    }

}

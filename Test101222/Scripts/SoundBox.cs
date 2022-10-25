using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundBox : MonoBehaviour {

    public AudioSource audioSource;


    public void OnTriggerEnter(Collider collider) {

        //Debug.Log("Entered");
        audioSource.Play();


    }

}

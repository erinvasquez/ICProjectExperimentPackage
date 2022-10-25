using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity;

public class CollisionSound : MonoBehaviour 
{
    public AudioSource audioSource;
    public void OnTriggerEnter(Collider collider)
    {
        audioSource.Play();

        Debug.Log("triggered");

    }

}

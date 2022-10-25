using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour {


    [SerializeField]
    public SpawnPointSO thisSpawnPoint;

    // Start is called before the first frame update
    void Start() {
        Debug.Log($"Setting spawn point scriptable object to {transform.position}");
        thisSpawnPoint.SpawnPoint = transform.position;
    }

    // Update is called once per frame
    void Update() {

    }
}

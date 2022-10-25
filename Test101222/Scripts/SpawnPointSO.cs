using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SpawnPointSO : ScriptableObject {

    [SerializeField]
    private Vector3 spawnPoint;

    public Vector3 SpawnPoint {
        get { return spawnPoint; }
        set { spawnPoint = value; }
    }


}

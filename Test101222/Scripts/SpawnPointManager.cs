using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointManager : MonoBehaviour {

    [SerializeField]
    public SpawnPointSO localSpawnPoint;
    [SerializeField]
    private GameStateSO gameState;

    public GameObject localPlayer;

    // Start is called before the first frame update
    void Start() {
        localPlayer = GameObject.Find("OfflineLocalPlayer");
        //localPlayer.gameObject.SetActive(false);

    }

}

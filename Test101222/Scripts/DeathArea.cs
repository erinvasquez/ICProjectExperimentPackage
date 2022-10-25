using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathArea : MonoBehaviour {

    LevelTimer levelTimer;

    [SerializeField]
    private GameStateSO gameState;

    public void Start() {

        if (GameObject.Find("LevelManager") != null) {
            levelTimer = GameObject.Find("LevelManager").GetComponent<LevelTimer>();
        } else {
            Debug.Log("Start Area: no level manager");
        }

    }

    public void OnTriggerEnter(Collider other) {
        // Debug.Log("Entered");

        if (other == null || other.gameObject.name != "OfflineLocalPlayer" || gameState.CurrentState != GameState.GamemodeStart) {
            return;
        }

        Debug.Log($"{other.name} entered Death area");

        

        if (levelTimer != null) {
            levelTimer.StopTimer();
        }

        Debug.Log("Death area changing state to Gamemode lose");
        gameState.CurrentState = GameState.GamemodeLose;

    }

}

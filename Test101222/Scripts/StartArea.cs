using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartArea : MonoBehaviour {

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
        //Debug.Log("Entered");

    }


    public void OnTriggerExit(Collider other) {

        Debug.Log($"{other.gameObject.name} exited start area...");

        // Return if this isn't GamemodeSetup
        if (gameState.CurrentState != GameState.GamemodeSetup || other == null) {
            return;
        }


        if (other.gameObject.name == "DesktopLocalPlayer" || other.gameObject.name == "VR Player") {
            Debug.Log($"{other.gameObject.name} exited Start area");

            gameState.CurrentState = GameState.GamemodeStart;

            if (levelTimer != null) {
                levelTimer.StartTimer();
            }

        } else {
            return;
        }

        


    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndArea : MonoBehaviour {

    LevelTimer levelTimer;

    [SerializeField]
    private GameStateSO currentGameState;

    public void Start() {

        if (GameObject.Find("LevelTimer") != null) {
            levelTimer = GameObject.Find("LevelTimer").GetComponent<LevelTimer>();
        }

    }

    public void OnTriggerEnter(Collider other) {


        Debug.Log($"{other.name} entered End area...");

        if (other == null || currentGameState.CurrentState != GameState.GamemodeStart) {
            return;
        }

        if (other.name == "DesktopLocalPlayer" || other.name == "VR Player") {
            Debug.Log($"{other.name} entered End area");

            if (levelTimer != null) {
                levelTimer.StopTimer();
            }

            Debug.Log("EndArea Changing state to Gamemode Win");
            currentGameState.CurrentState = GameState.GamemodeWin;

        } else {
            return;
        }

        

        

    }


    public void OnTriggerExit(Collider other) {
    }

}

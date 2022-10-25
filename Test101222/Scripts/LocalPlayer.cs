using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Put on a local player object.
/// </summary>
public class LocalPlayer : MonoBehaviour {

    [SerializeField]
    public SpawnPointSO localSpawnPoint;
    [SerializeField]
    private GameStateSO currentGameState;

    public GameState oldState { get; private set; }

    public GameObject desktopPlayerObject;
    public GameObject vrPlayerObject;
    public GameObject otherCamera;

    public bool isVRPlayer = false;


    public void Start() {

        Debug.Log($"Local spawn point read as: {localSpawnPoint.SpawnPoint}");


        /*
        if (GameObject.Find("Desktop Player").gameObject != null) {

            desktopPlayerObject = GameObject.Find("Desktop Player").gameObject;
            desktopPlayerObject.transform.position = localSpawnPoint.SpawnPoint;

        }
        */




        //otherCamera = GameObject.Find("Spectate Camera").gameObject;
        //vrPlayerObject = GameObject.Find("VR Player").gameObject;


        //desktopPlayerObject.SetActive(false);
        //otherCamera.SetActive(true);
        //UsePlayer();


    }

    private void Update() {

        if(currentGameState.CurrentState != oldState) {

            oldState = currentGameState.CurrentState;


            switch (oldState) {
                case GameState.MainMenu:
                    HandleMainMenu();
                    break;
                case GameState.SpawningFriendly:
                    HandleSpawningFriendly();
                    break;
                case GameState.GamemodeWin:
                    HandleGamemodeWin();
                    break;
                case GameState.GamemodeLose:
                    HandleGamemodeLose();
                    break;
            }

        }

    }


    public void HandleMainMenu() {
        //Debug.Log("Local Player detected menu");

        
        if (desktopPlayerObject.activeSelf || !otherCamera.activeSelf) {

            //Debug.Log("Local player is still active, or other camera is active");
            //desktopPlayerObject.SetActive(false);
            //otherCamera.SetActive(true);

            UseSpectateCamera();


        }


    }

    public void HandleSpawningFriendly() {
        //Debug.Log("SpawningFriendly, Localplayer to active");

        //otherCamera.SetActive(false);
        //desktopPlayerObject.SetActive(true);

        UsePlayer();


        Vector3 newSpawnPoint = localSpawnPoint.SpawnPoint;

        //Debug.Log($"Moving local player from {localPlayerObject.transform.position} to {newSpawnPoint}");
        desktopPlayerObject.transform.position = newSpawnPoint;

        //Debug.Log($"Moved local player to {localPlayerObject.transform.position} ({newSpawnPoint})");

       // Debug.Log("Local Player Changing game manager state to \"spawning enemies\"");

        // TODO: handle changing this game state some other way through the Game manager itself directly?

        currentGameState.CurrentState = GameState.SpawningEnemies;
        GameManager.Instance.HandleState();

    }

    public void HandleGamemodeWin() {

        if(desktopPlayerObject.activeSelf || !otherCamera.activeSelf) {
            Vector3 newSpawnPoint = localSpawnPoint.SpawnPoint;

            //Debug.Log($"Local player moving from {localPlayerObject.transform.position} to {newSpawnPoint}");
            desktopPlayerObject.transform.position = newSpawnPoint;

            //Debug.Log($"Local player moving to {localPlayerObject.transform.position} ({newSpawnPoint}) and disabling, spectate camera on");


            //otherCamera.SetActive(true);
            //desktopPlayerObject.SetActive(false);

            UseSpectateCamera();


        }

        
    }

    public void HandleGamemodeLose() {

        if (desktopPlayerObject.activeSelf || !otherCamera.activeSelf) {
            //Debug.Log("Local player has lost the game, setting spectate camera on");

            desktopPlayerObject.transform.position = localSpawnPoint.SpawnPoint;



            //otherCamera.SetActive(true);
            //desktopPlayerObject.SetActive(false);

            UseSpectateCamera();

        }

    }

    public void UsePlayer() {

        if (!isVRPlayer) {
            UseDesktopPlayer();
        } else {
            UseVRPlayer();
        }
    }


    public void UseDesktopPlayer() {

        if(otherCamera.activeSelf) {
            otherCamera.SetActive(false);
        }

        if(vrPlayerObject.activeSelf) {
            vrPlayerObject.SetActive(false);
        }

        
        desktopPlayerObject.SetActive(true);

    }

    public void UseVRPlayer() {

        if (otherCamera.activeSelf) {
            otherCamera.SetActive(false);
        }

        if (otherCamera.activeSelf) {
            desktopPlayerObject.SetActive(false);
        }

        
        vrPlayerObject.SetActive(true);


    }

    public void UseSpectateCamera() {

        if (vrPlayerObject.activeSelf) {
            vrPlayerObject.SetActive(false);
        }

        if (desktopPlayerObject.activeSelf) {
            desktopPlayerObject.SetActive(false);
        }


        otherCamera.SetActive(true);

    }


}

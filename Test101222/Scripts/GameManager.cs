using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager Instance { get; private set; }

    public GameState oldState { get; private set; }

    [SerializeField]
    private GameStateSO currentGameState;


    //public ICProject_AudioRecorder audioRecorder;
    public CSVWriter csvWriter;
    //public GameObject SpectateCamera;
    //public GameObject desktopPlayer;
    //public GameObject vrPlayer;


    //public ExperimentList myExperiments;
    //public ExperimentLog myLog = new ExperimentLog();
    //public float timeSinceLastLog = 0.0f;
    //public float logCooldown = 2.0f;


    public int maxGameTimeInSeconds = 60 * 30;
    public int deathScreenTimeInSeconds = 60 * 5;
    public float timeAfterDeathInSeconds = 0f;

    /// <summary>
    /// Increment this for every game we do, and make it editable by RA staff, UI etc
    /// </summary>
    public int currentExperimentID = 0;
    public int currentExperimentIndex = 0;
    [SerializeField]
    private ExperimentIDSO masterExperimentID;


    /// <summary>
    /// Set this in our UI
    /// </summary>
    public MapConditions currentMapCondition = MapConditions.Control;

    /// <summary>
    /// 
    /// </summary>
    public SocialSkillsConditions currentSocialSkillCondition = SocialSkillsConditions.High;





    /// <summary>
    /// 
    /// </summary>
    void Awake() {
        Instance = this;
        //DontDestroyOnLoad(this);

    }

    // Start is called before the first frame update
    void Start() {

        //audioRecorder = GameObject.Find("ICProject_AudioRecorder").GetComponent<ICProject_AudioRecorder>();
        //csvWriter = GameObject.Find("CSVWriter").GetComponent<CSVWriter>();
        //SpectateCamera = GameObject.Find("Spectate Camera");
        //desktopPlayer = GameObject.Find("Desktop Player");
        //vrPlayer = GameObject.Find("VR Player");

        //myExperiments = new ExperimentList();
        //myExperiments.dataArray = new List<Experiment>();

        //myLog = new ExperimentLog();
        //myLog.logEntries = new List<LogEntry>();

        
        
        currentGameState.CurrentState = GameState.MainMenu;
        oldState = currentGameState.CurrentState;
        HandleState();


    }

    // Update is called once per frame
    void Update() {

        // Check if we need to handle a new state
        if (currentGameState.CurrentState != oldState) {
            Debug.Log("Gamemanager saved state not equal to actual current game state");
            Debug.Log($"GameManager update changing local state var from {oldState} to {currentGameState.CurrentState}");

            oldState = currentGameState.CurrentState;
            HandleState();
        }

        // Check if the time after we've started has reached the maximum game time
        if (Time.time /* + _timeStarted */> maxGameTimeInSeconds) {
            Debug.Log("Reached Max Game time in seconds!");
        }

        // If the game is over, keep track of time after death
        if (currentGameState.CurrentState == GameState.GamemodeLose) {

            if (timeAfterDeathInSeconds > deathScreenTimeInSeconds) {

                //Debug.Log($"Dead and GameManager updating local state var from {State} to {currentGameState.CurrentState}");
                currentGameState.CurrentState = GameState.MainMenu;
                //HandleState();



            } else {

                timeAfterDeathInSeconds += Time.deltaTime;

            }


        } else if(currentGameState.CurrentState == GameState.GamemodeStart) {
            // If the game has started and we haven't won or lost yet (GamemodeWin or GamemodeLose)

            /*
            if(timeSinceLastLog > logCooldown) {
                timeSinceLastLog = 0f;
                LogEntry newEntry = new LogEntry($"{Time.time}",
                    $"Experiment {masterExperimentID.ID} Map Condtion: {myExperiments.dataArray[currentExperimentIndex].mapCondition} Skill Condition: {myExperiments.dataArray[currentExperimentIndex].socialSkillsCondition}",
                    $"{audioRecorder.GetAveragedVolume()}", $"{audioRecorder.frequency}",audioRecorder.currentNote);
                myLog.logEntries.Add(newEntry);
                Debug.Log($"Logging entry: {newEntry}");

            } else {
                timeSinceLastLog += Time.deltaTime;
            }
            */

        }


    }

    /// <summary>
    /// Called by MainMenu, LocalPlayer and this GameManager
    /// </summary>
    /// <param name="_newState"></param>
    public void HandleState() {

        oldState = currentGameState.CurrentState;

        switch (currentGameState.CurrentState) {

            case GameState.MainMenu:
                HandleMainMenu();
                break;
            case GameState.Waiting:
                // Called by Main Menu
                HandleWaiting();
                break;
            case GameState.SpawningFriendly:
                HandleSpawningFriendly();
                break;
            case GameState.SpawningEnemies:
                HandleSpawningEnemies();
                break;
            case GameState.GamemodeSetup:
                HandleGamemodeSetup();
                break;
            case GameState.GamemodeStart:
                HandleGamemodeStart();
                break;
            case GameState.GamemodeWin:
                HandleGamemodeWin();
                break;
            case GameState.GamemodeLose:
                HandleGamemodeLose();
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(currentGameState.CurrentState), currentGameState.CurrentState, null);

        }

    }

    private void HandleMainMenu() {

        //Debug.Log("Handling MainMenu");


        if(DebriefMenu.Instance != null) {
            EXP_MenuManager.Instance.CloseMenu();
        }

        //currentExperimentID++;
        //AudioManager.Instance.PlayTrack(0);

        if(RA_Menu.Instance != null) {

            RA_Menu.Show();

        } else {
            //Debug.Log("Main menu instance is null");
            RA_Menu.Show();
        }

        






    }

    /// <summary>
    /// 
    /// </summary>
    private void HandleWaiting() {
       // Debug.Log("Handling waiting state");
        //Debug.Log("playing new track");

        //AudioManager.Instance.PlayTrack(1);
        timeAfterDeathInSeconds = 0f;


        // Closing this menu for now
        if (RA_Menu.Instance != null) {
            EXP_MenuManager.Instance.CloseMenu();
        }


        // use this as our default experiment, but keep track of this with some UI that we'll set up later
        //Debug.Log($"Incrementing experiment ID{currentExperimentID} to {currentExperimentID + 1}");
        Experiment currentExperiment = new Experiment($"{masterExperimentID.ID}", currentMapCondition, currentSocialSkillCondition);
        //currentExperimentID++;
        //myExperiments.dataArray.Add(currentExperiment);

        /*
        LogEntry newEntry = new LogEntry($"{Time.time}", $"Starting {masterExperimentID.ID} with Map Condition: {myExperiments.dataArray[currentExperimentIndex].mapCondition} Social Condition: {myExperiments.dataArray[currentExperimentIndex].socialSkillsCondition}",
            $"{audioRecorder.GetAveragedVolume()}",$"{audioRecorder.frequency}", audioRecorder.currentNote);
        myLog.logEntries.Add(newEntry);
        //Debug.Log($"Logging entry: {newEntry}");
        */



        
        


        // unload the start scene
        //Debug.Log("Attempting Unloading of scene StartScene to Offline_Level1");
        int sceneCount = SceneManager.sceneCount;

        for (int a = 0; a < sceneCount; a++) {

            string sceneName = SceneManager.GetSceneAt(a).name;

            if (sceneName.Equals("StartScene")) {
                //Debug.Log($"{sceneName} detected, unload");
                SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(a));
            }

        }


        //SceneManager.UnloadSceneAsync("StartScene");
        //SceneManager.LoadScene("Offline_Level1", LoadSceneMode.Additive);


        //Debug.Log("GameManager setting state SpawningFriendly");
        currentGameState.CurrentState = GameState.SpawningFriendly;
        oldState = currentGameState.CurrentState;

        HandleState();

    }

    private void HandleSpawningFriendly() {
        //Debug.Log("Handling Spawn Friendly states");
    }

    private void HandleSpawningEnemies() {
        //Debug.Log("Handling Spawn Enemies State");

        //Debug.Log("Game Manager changing game state to Gamemode Setup");
        currentGameState.CurrentState = GameState.GamemodeSetup;
        oldState = currentGameState.CurrentState;

        HandleState();
    }



    private void HandleGamemodeSetup() {
        //Debug.Log("Handling GamemodeSetup");


    }

    private void HandleGamemodeStart() {
        //Debug.Log("Handling GamemodeStart");

        //audioRecorder.Record();


    }

    private void HandleGamemodeWin() {
        //Debug.Log("Handling GamemodeWin");

        // Stop logging and write our full log to an experiment file with the appropriate name
        WriteLogToCSV();


        // Get ready for next experiment
        masterExperimentID.ID++;
        currentExperimentID = masterExperimentID.ID;
        currentExperimentIndex++;

        

        //AudioManager.Instance.PlayTrack(0);

        if(RA_Menu.Instance != null) {

            RA_Menu.Show();

        } else {
            //Debug.Log("Main Menu instance is null");
            RA_Menu.Show();

        }
       
        // unload the start scene
        int sceneCount = SceneManager.sceneCount;

        for (int a = 0; a < sceneCount; a++) {

            string sceneName = SceneManager.GetSceneAt(a).name;

            if (sceneName.Equals("Offline_Level1")) {
                //Debug.Log($"{sceneName} detected, unload");
                SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(a));
            }

        }

        //Debug.Log("Loading Start Scene");
        //SceneManager.LoadSceneAsync("StartScene", LoadSceneMode.Additive);


        //Debug.Log("Stopping record, and exporting audio");
        //audioRecorder.Stop();
        //audioRecorder.ExportAudio();

    }

    private void HandleGamemodeLose() {

        //Debug.Log("Handling GamemodeLose");
        // handle stop logging and write out excel file of logs
        WriteLogToCSV();

        // prepare next experiment
        masterExperimentID.ID++;
        currentExperimentID = masterExperimentID.ID;
        currentExperimentIndex++;

        //Debug.Log("Stopping record, and exporting audio");

        //audioRecorder.Stop();
        //audioRecorder.ExportAudio();

        DebriefMenu.Show();

    }

    public void WriteLogToCSV() {
        // set up our data first

        /*
        string _filename = $"Experiment ID {myExperiments.dataArray[currentExperimentIndex].ID} {myExperiments.dataArray[currentExperimentIndex].mapCondition} {myExperiments.dataArray[currentExperimentIndex].socialSkillsCondition} Log";
        string _categories = "Log Time, Log Message, Volume Value, Frequency Value, Music Note";
        string _data = "";

        // now extract our logs and add to our data string
        for(int a = 0; a < myLog.logEntries.Count; a++) {

            _data += $"{myLog.logEntries[a]}\n";
        }
        */

        // then add to our method
        //csvWriter.WriteToFile(_filename, _categories, _data);

    }

    public void SetID(int _ID) {
        masterExperimentID.ID = _ID;
        currentExperimentID = _ID;
    }

    public void SetMapCondition(MapConditions _condition) {
        currentMapCondition = _condition;
    }

    public void SetSocialSkillCondition(SocialSkillsConditions _condition) {
        currentSocialSkillCondition = _condition;
    }


    private void OnDestroy() {
        Instance = null;
    }

    private void OnApplicationQuit() {
        currentGameState.CurrentState = GameState.MainMenu;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelTimer : MonoBehaviour {

    [SerializeField]
    private GameStateSO currentGameState;

    public CSVWriter csvWriter;
    public string currentTimerLog = "";

    public float logTime = 0.0f;
    public float logCooldown = 0.25f;

    public float timeSpentPlayingInSeconds = 0.0f;
    public float startTime = -1.0f;
    public float endTime = -1.0f;

    public bool timerStarted = false;
    public bool timerPaused = false;
    public bool timerEnded = false;
    public bool logWritten = false;

    private void Start() {

        csvWriter = GameObject.Find("CSVWriter").GetComponent<CSVWriter>();

    }


    private void Update() {

        if (!timerStarted && currentGameState.CurrentState == GameState.GamemodeStart) {

            StartTimer();

        } else if (timerStarted && !timerPaused) {
            timeSpentPlayingInSeconds += Time.deltaTime;

            if (logTime > logCooldown) {
                logTime = 0;
                //Debug.Log(GetTimerCSVLine());

                currentTimerLog += GetTimerCSVLine() + "\n";



            } else {
                logTime += Time.deltaTime;
            }

        }

    }

    public void StartTimer() {

        if (!timerStarted) {
            startTime = Time.time;
            timerStarted = true;
            timerEnded = false;
            //Debug.Log("Starting at " + startTime);
        } else {
            Debug.LogError("Timer has already started!");
        }
    }

    public void TogglePauseTimer() {

        timerPaused = !timerPaused;

    }

    public void StopTimer() {

        endTime = Time.time;
        timerEnded = true;
        Debug.Log(endTime);

        Debug.Log("Writing CSV");
        csvWriter.WriteToFile("ExperimentLeaderboard", "Start Time,Total Time", currentTimerLog);

        currentTimerLog = "";
        logWritten = true;

        ResetTimer();
    }

    public void ResetTimer() {

        timeSpentPlayingInSeconds = 0f;
        startTime = -1f;
        endTime = -1f;
        timerStarted = false;
        timerEnded = false;

    }

    public string GetTimerCSVLine() {
        return $"{startTime},{timeSpentPlayingInSeconds}";
    }

    public string GetTimerLog() {

        if (timerStarted) {
            return $"Timer Started: {timerStarted}, Current Time: {timeSpentPlayingInSeconds}";
        } else if (timerEnded) {
            return $"Timer Started: {timerStarted}, End Time: {endTime}";
        } else {
            return "";
        }


    }

}

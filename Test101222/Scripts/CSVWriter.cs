using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.InputSystem;


/// <summary>
/// Experiment data that will be saved to a CSV Excel file
/// </summary>
[System.Serializable]
public class Experiment {

    public string ID;
    public MapConditions mapCondition;
    public SocialSkillsConditions socialSkillsCondition;

    public Experiment(string _ID, MapConditions _mapCondition, SocialSkillsConditions _socialSkillsConditions) {
        ID = _ID;
        mapCondition = _mapCondition;
        socialSkillsCondition = _socialSkillsConditions;
    }

}

/// <summary>
/// A list of experiments conducted
/// </summary>
[System.Serializable]
public class ExperimentList {
    public List<Experiment> dataArray;
}

/// <summary>
/// Log data that will be saved to a CSV Excel file
/// </summary>
[System.Serializable]
public class LogEntry {

    public string logTime;
    public string logMessage;
    public string volumeValue;
    public string frequencyValue;
    public MusicNote musicNote;

    public LogEntry(string _logTime, string _logMessage, string _volumeValue, string _frequencyValue, MusicNote _musicNote) {

        logTime = _logTime;
        logMessage = _logMessage;
        volumeValue = _volumeValue;
        frequencyValue = _frequencyValue;
        musicNote = _musicNote;

    }

    public override string ToString() {
        return $"{logTime},{logMessage},{volumeValue},{frequencyValue},{musicNote}";
    }

}

/// <summary>
/// A list of the current log entries saved
/// </summary>
[System.Serializable]
public class ExperimentLog {
    public List<LogEntry> logEntries;
}

/// <summary>
/// Have this handle all of our CSV stuff
/// Getting data from the system and triggering
/// when to handle it, etc
/// </summary>
public class CSVWriter : MonoBehaviour {

    public void Start() {

    }

    /// <summary>
    /// Write a CSV/Excel file with the given filename, categories, and data
    /// </summary>
    /// <param name="_filename">The Desired Filename before appending .csv to the file</param>
    /// <param name="_categories">The CSV Categories, or top row of the Excel Sheet</param>
    /// <param name="_data"></param>
    public void WriteToFile(string _filename, string _categories, string _data) {
        _filename = $"{Application.dataPath}/{_filename}.csv";

        Debug.Log($"Attempting write to CSV file {_filename}");

        TextWriter tw = new StreamWriter(_filename, false);
        tw.WriteLine(_categories);
        tw.Close();

        tw = new StreamWriter(_filename, true);
        tw.WriteLine(_data); // Write instead of WriteLine for now
        tw.Close();

    }


}

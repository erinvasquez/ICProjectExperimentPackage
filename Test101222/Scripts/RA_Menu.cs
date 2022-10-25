using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class RA_Menu : SimpleMenu<RA_Menu> {

    public static Button GoButton;

    public TMP_InputField participantIDField;
    public TMP_Dropdown mapConditionDropdown;
    public TMP_Dropdown skillConditionDropdown;


    public void OnPressGo() {
        // Load the next menu
        RA_Menu.Close();
        RA_Menu_II.Show();
    }

    /// <summary>
    /// TODO: ensure input is numbers only
    /// </summary>
    /// <param name="_ID"></param>
    public void OnSetParticpantID(string _ID) {
        GameManager.Instance.currentExperimentID = int.Parse(_ID);
    }

    public void OnSelectMapCondition(int _condition) {

        Debug.Log($"Changing Map condition from {GameManager.Instance.currentMapCondition} to {(MapConditions) _condition}");

        GameManager.Instance.currentMapCondition = (MapConditions)_condition;
    }

    public void OnSelectSocialSkillCondition(int _condition) {
        Debug.Log($"Changing Social Skill condition from {GameManager.Instance.currentSocialSkillCondition} to {(SocialSkillsConditions)_condition}");


        GameManager.Instance.currentSocialSkillCondition = (SocialSkillsConditions) _condition;
    }

}
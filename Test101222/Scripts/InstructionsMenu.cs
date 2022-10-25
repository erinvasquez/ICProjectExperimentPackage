using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionsMenu : SimpleMenu<InstructionsMenu> {

    [SerializeField]
    private GameStateSO currentGameState;
    
    public void OnPressBack() {
        InstructionsMenu.Close();
        RA_Menu_II.Show();
    }

    public void OnPressGo() {
        InstructionsMenu.Close();

        currentGameState.CurrentState = GameState.Waiting;

    }


}

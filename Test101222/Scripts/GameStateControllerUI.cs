using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateControllerUI : MonoBehaviour {

    [SerializeField]
    private GameStateSO currentGameState;

    public void OnPressMainMenuButton() {
        currentGameState.CurrentState = GameState.MainMenu;
    }

    public void OnPressWaitingButton() {
        currentGameState.CurrentState = GameState.Waiting;

    }

    public void OnPressSpawningFriendlyButton() {
        currentGameState.CurrentState = GameState.SpawningFriendly;

    }

    public void OnPressSpawningEnemiesButton() {
        currentGameState.CurrentState = GameState.SpawningEnemies;

    }

    public void OnPressGamemodeSetup() {
        currentGameState.CurrentState = GameState.GamemodeSetup;

    }

    public void OnPressGamemodeStart() {
        currentGameState.CurrentState = GameState.GamemodeStart;

    }

    public void OnPressGamemodeWin() {
        currentGameState.CurrentState = GameState.GamemodeWin;

    }

    public void OnPressGamemodeLose() {
        currentGameState.CurrentState = GameState.GamemodeLose;

    }


}

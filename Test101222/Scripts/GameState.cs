using System;
using Unity;
using UnityEngine;

/// <summary>
/// 
/// </summary>
[Serializable]
public enum GameState {
    /// <summary>
    /// Main Menu is loaded, not gameplay
    /// </summary>
    MainMenu = 0,
    /// <summary>
    /// Game is loaded and is waiting to start loading our resources before
    /// controls are available to player
    /// </summary>
    Waiting,
    /// <summary>
    /// Spawn friendly NPCS and network players?
    /// </summary>
    SpawningFriendly,
    /// <summary>
    /// Spawn enemy NPCs and network players?
    /// </summary>
    SpawningEnemies,
    /// <summary>
    /// Game has been setup, and player resumes control
    /// </summary>
    GamemodeSetup,
    /// <summary>
    /// Gamemode starts and data tracking can
    /// commence
    /// </summary>
    GamemodeStart,
    /// <summary>
    /// Gamemode has ended and local player has won
    /// </summary>
    GamemodeWin,
    /// <summary>
    /// Gamemode has ended and local player has lost
    /// </summary>
    GamemodeLose
}

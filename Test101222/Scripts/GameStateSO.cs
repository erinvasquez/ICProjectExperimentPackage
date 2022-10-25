using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameStateSO : ScriptableObject {

    [SerializeField]
    private GameState _currentState;

    public GameState CurrentState {
        get { return _currentState; }
        set { _currentState = value; }
    }


}

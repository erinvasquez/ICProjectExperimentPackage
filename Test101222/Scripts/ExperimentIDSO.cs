using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ExperimentIDSO : ScriptableObject {

    [SerializeField]
    private int _ID;

    public int ID {
        get { return _ID; }
        set { _ID = value; }
    }

}

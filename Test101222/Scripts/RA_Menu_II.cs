using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RA_Menu_II : SimpleMenu<RA_Menu_II> {
    
    public void OnPressEdit() {
        RA_Menu_II.Close();
        RA_Menu.Show();
    }

    public void OnPressGo() {
        RA_Menu_II.Close();
        InstructionsMenu.Show();
    }

}

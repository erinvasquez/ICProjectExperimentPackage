using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebriefMenu : SimpleMenu<DebriefMenu> {

    public void OnOkayPressed() {
        DebriefMenu.Close();
        RA_Menu.Show();
    }

}

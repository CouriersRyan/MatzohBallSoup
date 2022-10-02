using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayButton : Button
{
    public void Pressed(bool isPressed)
    {
        if (isPressed)
        {
            DoStateTransition(SelectionState.Pressed, true);
        }
        else
        {
            DoStateTransition(SelectionState.Normal, true);
        }
        
    }
}

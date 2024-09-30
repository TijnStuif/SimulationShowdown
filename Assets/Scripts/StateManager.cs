using System;
using Unity.VisualScripting;
using UnityEngine;

public static class StateManager
{
    // branchless programming lol
    // explanation:
    // -(1 - 1) = -0 = 0
    // -(0 - 1) = 1
    // so I sorta inverted 1 to 0 and 0 to 1 with math that coincidentally works
    private static int Invert(int input) => -(input - 1);
    
    public static void TogglePauseState() => Time.timeScale = Invert((int)Time.timeScale);

    public static void ChangePauseState(bool state) => Time.timeScale = state ? 0 : 1f;
}

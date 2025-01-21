using UnityEngine;
using UnityEngine.Events;

//class used to reset all glitch values to phase 1 values
public class CutsceneDeactivator : MonoBehaviour
{
    public UnityEvent glitchEnd;

    private void OnEnable()
    {
        glitchEnd.Invoke();
    }
}
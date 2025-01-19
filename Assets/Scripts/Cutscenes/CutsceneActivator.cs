using UnityEngine;
using UnityEngine.Events;

//class used to set all glitch values to their maximum values
public class CutsceneActivator : MonoBehaviour
{
    public UnityEvent glitchOnCutscene;

    private void OnEnable()
    {
        glitchOnCutscene.Invoke();
    }
}
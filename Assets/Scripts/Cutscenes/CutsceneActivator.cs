using UnityEngine;
using UnityEngine.Events;

public class CutsceneActivator : MonoBehaviour
{
    public UnityEvent glitchOnCutscene;

    private void OnEnable()
    {
        glitchOnCutscene.Invoke();
    }
}
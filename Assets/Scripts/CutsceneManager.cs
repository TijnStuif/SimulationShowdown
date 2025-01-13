using UnityEngine;
using UnityEngine.Playables;

public class CutsceneManager : MonoBehaviour
{
    private Boss.Controller bossController;
    public PlayableDirector endCutscene;
    private void Start()
    {
        bossController = FindObjectOfType<Boss.Controller>();
        bossController.Death += StartEndCutscene;
    }
    private void StartEndCutscene()
    {
        endCutscene.Play();
    }
}

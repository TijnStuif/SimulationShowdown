using UnityEngine;
using UnityEngine.Playables;

public class CutsceneManager : MonoBehaviour
{
    private Boss.Controller bossController;
    public PlayableDirector winCutscene;
    private void Start()
    {
        bossController = FindObjectOfType<Boss.Controller>();
        bossController.Death += StartWinCutscene;
    }
    private void StartWinCutscene()
    {
        winCutscene.Play();
    }
}

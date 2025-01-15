using UnityEngine;
using UnityEngine.Playables;

public class CutsceneManager : MonoBehaviour
{
    private Boss.Controller bossController;
    private Player.V2.Controller playerController;
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

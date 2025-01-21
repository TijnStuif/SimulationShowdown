using GameOverScreen;
using UnityEngine;
using UnityEngine.Playables;
using Player.V2;

public class CutsceneManager : MonoBehaviour
{
    private Boss.Controller bossController;
    private Player.V2.Controller playerController;
    public PlayableDirector winCutscene;
    public PlayableDirector loseCutscene;
    
    private void OnEnable()
    {
        bossController = FindObjectOfType<Boss.Controller>();
        playerController = FindObjectOfType<Player.V2.Controller>();
        bossController.Death += StartWinCutscene;
        playerController.StateChange += StartLoseCutscene;
    }

    private void OnDisable()
    {
        bossController.Death -= StartWinCutscene;
        playerController.StateChange -= StartLoseCutscene;
    }

    private void StartWinCutscene()
    {
        winCutscene.Play();
    }

    private void StartLoseCutscene(Player.V2.State state)
    {
        if (state == Player.V2.State.Loss)
        {
            loseCutscene.Play();
        }
    }
}

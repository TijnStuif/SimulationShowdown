using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class GameControllerScript : MonoBehaviour
{
    private List<IAttack> attacks;
    private List<IAttack> environmentAttacks;
    private List<IAttack> directAttacks;
    private float timer;
    private bool gamePaused;
    private GameObject pauseMenu;
    public float minAttackInterval = 5f; 
    public float maxAttackInterval = 15f;
    [SerializeField] private GameObject pauseMenuFab;

    void Start()
    {
        pauseMenu = Instantiate(pauseMenuFab);
        pauseMenu.SetActive(false);
        // Gets all the objects in the scene that have the IAttack interface
        attacks = FindObjectsOfType<MonoBehaviour>().OfType<IAttack>().ToList();
        
        // Debug log all attacks
        foreach (var attack in attacks)
        {
            Debug.Log($"{attack.GetType().Name} - Type: {attack.Type}");
        }

        // list of attacks per type
        environmentAttacks = attacks.Where(attack => attack.Type == AttackType.Environment).ToList();
        directAttacks = attacks.Where(attack => attack.Type == AttackType.Direct).ToList();

        SetRandomInterval(); 
    }

    void Update()
    {
        timer -= Time.deltaTime; 

        if (timer <= 0f)
        {
            ExecuteRandomAttack();
            SetRandomInterval(); 
        }
    }

    private void ExecuteRandomAttack()
    {
        if (attacks.Count == 0)
        {
            Debug.LogWarning("No attacks available to execute.");
            return;
        }

        int randomIndex = UnityEngine.Random.Range(0, attacks.Count);
        attacks[randomIndex].Execute();
        Debug.Log($"Executed attack: {attacks[randomIndex].GetType().Name}");
    }

    private void OnPause()
    {
        if (gamePaused)
        {
            pauseMenu.SetActive(false);
            gamePaused = false;
            return;
        }

        pauseMenu.SetActive(true);
        gamePaused = true;
    }
    

    private void SetRandomInterval()
    {
        timer = Mathf.Round(UnityEngine.Random.Range(minAttackInterval, maxAttackInterval));
        Debug.Log($"Next attack in {timer} seconds");
    }
}
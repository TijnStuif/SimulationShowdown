using System.Collections.Generic;
using System.Linq;
using Boss;
using Boss.Attack;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

public class GameControllerScript : MonoBehaviour
{
    private List<IAttack> attacks;
    private List<IAttack> environmentAttacks;
    private List<IAttack> directAttacks;
    private float timer;
    private GameObject pauseMenu;
    [SerializeField] private GameObject pauseMenuPrefab;
    public float minAttackInterval = 5f; 
    public float maxAttackInterval = 15f;
    public static bool GamePaused;

    void Start()
    {
        pauseMenu = Instantiate(pauseMenuPrefab);
        pauseMenu.GetComponent<UIDocument>().rootVisualElement.AddToClassList("hidden");
        // Gets all the objects in the scene that have the IAttack interface
        attacks = FindObjectsOfType<MonoBehaviour>().OfType<IAttack>().ToList();
        
        // Debug log all attacks
        foreach (var attack in attacks)
        {
            Debug.Log($"{attack.GetType().Name} - Type: {attack.Type}");
        }

        // list of attacks per type
        environmentAttacks = attacks.Where(attack => attack.Type == Type.Environment).ToList();
        directAttacks = attacks.Where(attack => attack.Type == Type.Direct).ToList();

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
        Debug.Log(attacks);
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
        if (GamePaused)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
            pauseMenu.GetComponent<UIDocument>().rootVisualElement.AddToClassList("hidden");
            GamePaused = false;
            return;
        }

        pauseMenu.GetComponent<UIDocument>().rootVisualElement.RemoveFromClassList("hidden");
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
        GamePaused = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    

    private void SetRandomInterval()
    {
        timer = Mathf.Round(UnityEngine.Random.Range(minAttackInterval, maxAttackInterval));
        Debug.Log($"Next attack in {timer} seconds");
    }
}
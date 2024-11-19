using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class HealthBar : MonoBehaviour
{

    VisualElement m_uiDocument;
    public Transform player;
    VisualElement healthbar;
    ProgressBar progressbar;
    public Camera cam;
    public UIDocument healthbarDocument;
    Player currentHealth;
   
    private void Awake()
    {
        //acccess the UI document
        m_uiDocument = GetComponent<UIDocument>().rootVisualElement;

        //access the healthbar VisualElement and the progressBar
        healthbar = m_uiDocument.Q<VisualElement>("healthbar");
        progressbar = m_uiDocument.Q<ProgressBar>("healthbar");

        m_uiDocument.Add(healthbar);
    }

    private void Update()
    {
        //Access the health of the player from the player script
        int health = GameObject.Find("Player").GetComponent<Player>().currentHealth;


        //adjust the position and value of the healthbar
        healthbar.style.top = 310;
        healthbar.style.width = 200;
        healthbar.style.height = 10;
        progressbar.value = health;
    }






    public UnityEngine.UI.Slider slider;
    public UnityEngine.UI.Image fill;
    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    public void SetHealth(int health)
    {
        slider.value = health;
    }
}

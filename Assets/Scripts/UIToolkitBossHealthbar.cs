using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIToolkitBossHealthbar : MonoBehaviour
{
    VisualElement m_uiDocument;
    VisualElement healthbar;
    ProgressBar progressbar;
    public UIDocument healthbarDocument;

   
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
        int health = GameObject.Find("Boss").GetComponent<Boss.Controller>().currentHealth;

        //adjust the value of the healthbar
        progressbar.value = health;
    }




}

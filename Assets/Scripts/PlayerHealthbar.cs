using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerHealthbar : MonoBehaviour
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

        //sets the haelthbar to a screenpoint
        Vector3 screen = cam.WorldToScreenPoint(player.position);

        //adjust the position and value of the healthbar
        healthbar.style.top = player.position.y - 310;
        healthbar.style.width = 200;
        healthbar.style.height = 10;
        progressbar.value = health;
    }
}

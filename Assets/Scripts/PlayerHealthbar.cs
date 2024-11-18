using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerHealthbar : MonoBehaviour
{
    VisualElement m_uiDocument;
    public Transform player;
    VisualElement healthbar;
    public Camera cam;
    public UIDocument healthbarDocument;
   
    private void Awake()
    {
        m_uiDocument = GetComponent<UIDocument>().rootVisualElement;
        Debug.Log(GetComponent<UIDocument>().rootVisualElement);


        VisualTreeAsset healthbarAsset = Resources.Load<VisualTreeAsset>("HUD");
         healthbar = m_uiDocument.Q<VisualElement>("healthbar");

        m_uiDocument.Add(healthbar);
    }

    private void Update()
    {
        Vector3 screen = cam.WorldToScreenPoint(player.position);
        healthbar.style.left = screen.x - (healthbar.layout.width / 2);
        healthbar.style.top = Screen.height - screen.y - 100;
    }




//     public Slider slider;
//     public Image fill;
//     public void SetMaxHealth(int health)
//     {
//         slider.maxValue = health;
//         slider.value = health;
//     }

//     public void SetHealth(int health)
//     {
//         slider.value = health;
//     }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class UIToolkitBossHealthbar : MonoBehaviour
{
    VisualElement m_uiDocument;
    VisualElement healthbar;
    ProgressBar progressbar;
    public UIDocument healthbarDocument;
    private PhaseController phaseController;
    private int phaseThresholdOffsetHeight;
    private int phaseThresholdOffsetWidth = 295;

   
    private void Awake()
    {
        //acccess the UI document
        m_uiDocument = GetComponent<UIDocument>().rootVisualElement;
        phaseController = FindObjectOfType<PhaseController>();

        //access the healthbar VisualElement and the progressBar
        healthbar = m_uiDocument.Q<VisualElement>("healthbar");
        progressbar = m_uiDocument.Q<ProgressBar>("healthbar");

        m_uiDocument.Add(healthbar);

        progressbar.style.top = -450;
        phaseThresholdOffsetHeight = 537;
    }

    private void Start()
    {
        // set the width between each threshold bar based on the number of phases
        int phaseCount = phaseController.phases.Count - 1;
        int[] thresholds = new int[phaseCount];
        for (int i = 0; i < phaseCount; i++)
        {
            thresholds[i] = (int)((i + 1) * (phaseThresholdOffsetWidth / phaseCount) / (phaseCount + 1));
        }


        // stylize the threshold bars
        foreach (int threshold in thresholds)
        {
            VisualElement thresholdBar = new VisualElement();
            thresholdBar.name = "thresholdBar";
            thresholdBar.style.backgroundColor = new StyleColor(Color.black);
            thresholdBar.style.position = Position.Absolute;
            thresholdBar.style.left = new StyleLength(new Length(threshold, LengthUnit.Percent));
            thresholdBar.style.width = 20;
            thresholdBar.style.height = new StyleLength(new Length(2f, LengthUnit.Percent));
            thresholdBar.style.bottom = phaseThresholdOffsetHeight;
            healthbar.Add(thresholdBar);
        }
    }

    private void Update()
    {
        
        //Access the health of the player from the player script
        int health = GameObject.Find("Boss").GetComponent<Boss.Controller>().currentHealth;

        //adjust the value of the healthbar
        progressbar.value = health;
    }




}

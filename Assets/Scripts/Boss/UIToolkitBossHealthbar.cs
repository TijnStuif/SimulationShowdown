using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class UIToolkitBossHealthbar : AbstractUiController
{
    VisualElement healthbar;
    ProgressBar progressBar;
    public UIDocument healthbarDocument;
    private const float PROGRESSBAR_OUTLINE_WIDTH = 5;
    private PhaseController phaseController;
    private int phaseThresholdOffsetHeight;
    private int phaseThresholdOffsetWidth = 295;

   
    private void Awake()
    {
        //acccess the UI document
        Root = GetComponent<UIDocument>().rootVisualElement;
        phaseController = FindObjectOfType<PhaseController>();

        // a little confusing but this is the container for the health bar
        healthbar = Root.Q<VisualElement>("hp-container");
        // and this is the progress bar named healthbar
        progressBar = healthbar.Q<ProgressBar>("healthbar");

        // progressbar.style.top = -450;
    }

    private void Start()
    {
        StartCoroutine(SetThresholdBars());
        // set the width between each threshold bar based on the number of phases
    }

    private IEnumerator SetThresholdBars()
    {
        int phaseCount = phaseController.phases.Count;
        float[] thresholds = new float[phaseCount];
        // Wait until width is initialized
        yield return new WaitUntil(() => healthbar.resolvedStyle.width > 0);
        
        var progressbarWidth = healthbar.resolvedStyle.width;
        for (int i = 0; i < phaseCount; i++)
        {
            thresholds[i] = (i + 1) * (progressbarWidth / phaseCount) - (PROGRESSBAR_OUTLINE_WIDTH * 2);
        }


        // stylize the threshold bars
        for (int i = 0; i < phaseCount-1; i++)
        {
            VisualElement thresholdBar = new VisualElement();
            thresholdBar.name = "thresholdBar";
            thresholdBar.style.backgroundColor = new StyleColor(Color.black);
            thresholdBar.style.position = Position.Absolute;
            thresholdBar.style.width = 20f;
            thresholdBar.style.height = 20f;
            thresholdBar.style.left = thresholds[i];
            progressBar.Add(thresholdBar);
        }
    }

    private void Update()
    {
        //Access the health of the player from the player script
        float health = FindObjectOfType<Boss.Controller>().currentHealth;

        //adjust the value of the healthbar
        progressBar.value = health;
    }




}

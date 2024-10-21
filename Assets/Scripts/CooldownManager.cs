using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Net.Mime.MediaTypeNames;
using UnityEngine.UI;


public class CooldownManager : MonoBehaviour
{
    public float Cooldown = 100;
    public UnityEngine.UI.Text CooldownText;
    // Update is called once per frame
    void Update()
    {
            CooldownText.text = Cooldown.ToString();
    }
}

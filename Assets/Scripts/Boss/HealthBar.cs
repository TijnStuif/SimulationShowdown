using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Boss
{
    public class HealthBar : MonoBehaviour
    {
        public Slider slider;
        public Image fill;
        
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
}

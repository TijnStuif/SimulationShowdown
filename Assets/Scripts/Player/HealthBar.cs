using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class HealthBar : MonoBehaviour
    {
        private Slider slider;
        private Image fill;

        private void Awake()
        {
            slider = GetComponentInChildren<Slider>();
            fill = GetComponentInChildren<Image>();
        }

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

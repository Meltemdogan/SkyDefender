using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts.HealthBar
{
    public class HealthBarController : MonoBehaviour
    {
        [SerializeField] private Slider slier;
        [SerializeField] private TMP_Text healthText;
        [SerializeField] private int maxHealth = 100;
        private int currentHealth;
        public bool IsAlive { get; private set; } = true;
        
        private void OnEnable()
        {
            currentHealth = maxHealth;
            IsAlive = true;
            UpdateUI();
        }
        private void Awake()
        {
            slier.maxValue = maxHealth;
        }
        private void UpdateUI()
        {
            slier.value = currentHealth;
            healthText.text = $"{currentHealth}/{maxHealth}";
        }
        public void DecreaseCurrentHealthBy(int value)
        {
            currentHealth -= value;
            
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                IsAlive = false;
            }
            UpdateUI();
        }
        public void IncreaseCurrentHealthBy(int value)
        {
            currentHealth += value;
            if (currentHealth > maxHealth)
                currentHealth = maxHealth;
            if (currentHealth > 0)
                IsAlive = true;
            UpdateUI();
        }
    }
}
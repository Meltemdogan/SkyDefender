using _Game.Scripts.Enemies.Interfaces;
using _Game.Scripts.HealthBar;
using UnityEngine;
using UnityEngine.Events;
public class CityController : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private HealthBarController healthSystem;
    
    private int currentHealth;
    public static CityController Instance { get; private set; }
    public event UnityAction OnCityDestroyed;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    
    private void Start()
    {
        currentHealth = maxHealth;
    }
    
    public void TakeDamage(int amount)
    {
        if (!healthSystem.IsAlive) return;
        healthSystem.DecreaseCurrentHealthBy(amount);
        if (!healthSystem.IsAlive)
        {
            OnDeath();
            OnCityDestroyed?.Invoke();
        }
    }
    
    public void OnDeath()
    {
        Debug.Log("City destroyed!"); 
    }
}
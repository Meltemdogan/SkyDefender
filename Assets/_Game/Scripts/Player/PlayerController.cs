using System;
using _Game.Scripts.Enemies.Interfaces;
using _Game.Scripts.HealthBar;
using _Game.Scripts.Projectiles;
using TMPro;
using ToolBox.Pools;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour, IDamageable
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 200f;
    
    [Header("Shooting")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;
    
    [Header("Bullet Count")]
    [SerializeField] private int maxBulletCount;
    [SerializeField] private int currentBulletCount;
    [SerializeField] private int spawnedBulletCount;
    [SerializeField] private TMP_Text bulletCountText;
    
    [SerializeField] private ProjectileData projectileData;
    
    [Header("UI References")]
    [SerializeField] private Button fireButton;
    
    [Header("Health System")]
    [SerializeField] private HealthBarController healthSystem;
    
    public Joystick joystick;
    public event UnityAction OnOutOfBullets;
    public event UnityAction OnPlayerDeath;
    private bool _hasFiredOutOfBulletsEvent = false;
     
    public static PlayerController Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    private void Start()
    {
        fireButton.onClick.AddListener(Shoot);
    }
    
    void Update()
    {
        transform.position += transform.up * moveSpeed * Time.deltaTime;
        
        Vector2 direction = joystick.Direction;
        if (direction.magnitude > 0.1f)
        {
            // turning the player towards the joystick direction
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f; // -90 to adjust for the sprite's orientation
            Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        if(Input.GetKeyDown("space"))
        {
            Shoot();
        }
        GetCurrentBulletCount();
        
    }
    public void TakeDamage(int amount)
    {
        healthSystem.DecreaseCurrentHealthBy(amount);
        if (!healthSystem.IsAlive)
        {
            OnDeath();
            OnPlayerDeath?.Invoke();
        }
    }
    public void OnDeath()
    {
        Debug.Log("Player has died.");
        Destroy(gameObject);
    }
    public void Shoot()
    {
        GameObject bulletObj = bulletPrefab.Reuse(bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        spawnedBulletCount++;
        
        bulletObj.GetComponent<Bullet>().Initialize(projectileData);
    }
    public void GetCurrentBulletCount()
    {
        currentBulletCount = maxBulletCount - spawnedBulletCount;
        bulletCountText.text = currentBulletCount.ToString();
        if (currentBulletCount <= 0 && !_hasFiredOutOfBulletsEvent) 
        {
            bulletCountText.text = "0";
            fireButton.interactable = false; 
            OnOutOfBullets?.Invoke();
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (CompareTag("Healer"))
        {
            healthSystem.IncreaseCurrentHealthBy(10);
        }
    }
    private void OnDestroy()
    {
        fireButton.onClick.RemoveListener(Shoot);
    }
}


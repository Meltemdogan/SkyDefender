using System;
using _Game.Scripts.Enemies;
using TMPro;
using UnityEngine;
using UnityEngine.Events;


public class TargetManager : MonoBehaviour
{
    public static TargetManager Instance { get; private set; }
    
    [SerializeField] private int targetPlaneCount;
    [SerializeField] private TMP_Text killCounterText; 
    private int currentKills;
    public event UnityAction OnTargetCompleted;
    private bool hasCompleted = false;
    
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        currentKills = 0;
        UpdateUI();
    }
    
    private void OnEnable()
    {
        BaseEnemy.OnEnemyDied += HandleEnemyDied;
    }
    
    private void OnDisable()
    {
        BaseEnemy.OnEnemyDied -= HandleEnemyDied;
    }
    
    private void HandleEnemyDied(BaseEnemy enemy)
    {
        currentKills++;
        UpdateUI();
        TargetCompleted();
    }
    
    private void UpdateUI()
    { 
        killCounterText.text = $"Kills: {currentKills}/{targetPlaneCount}";
    }
    private void TargetCompleted()
    {
        if (currentKills >= targetPlaneCount && !hasCompleted)
        {
            hasCompleted = true;
            OnTargetCompleted?.Invoke();
            
        }
    }
    public int GetKillCount() => currentKills;
}
using System;
using System.Collections;
using _Game.Scripts.Enemies;
using ToolBox.Pools;
using UnityEngine;
using Random = UnityEngine.Random;


namespace _Game.Scripts.Spawner
{
    public class EnemySpawner : MonoBehaviour
    {
        public static EnemySpawner Instance { get; private set; }
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }
        
        [Header("Spawn Settings")]
        [SerializeField] private GameObject[] enemyPrefabs;
        [SerializeField] private float spawnInterval = 3f;
        [SerializeField] private Transform[] corners; // 0 = TopLeft, 1 = TopRight, 2 = BottomRight, 3 = BottomLeft
        [SerializeField] private int maxEnemies = 30;
        public int spawnedEnemyCount = 0;
        public int currentEnemyCount
        {
            get
            { 
                return spawnedEnemyCount - diedEnemyCount;
            }
            set
            {
                spawnedEnemyCount = value;
                if (spawnedEnemyCount < 0)
                {
                    spawnedEnemyCount = 0;
                }
            }
        }
        public int diedEnemyCount;
        private bool canSpawn = true;

        private Coroutine spawnRoutine;

        private void Start()
        {
            spawnRoutine = StartCoroutine(SpawnEnemiesRoutine());
        }
        
        private void Update()
        {
            TotalEnemies();
        }
        
        private void TotalEnemies()
        {
            diedEnemyCount = TargetManager.Instance.GetKillCount();
            
            if (currentEnemyCount >= maxEnemies)
            {
                canSpawn = false;
                if (spawnRoutine != null)
                {
                    StopCoroutine(spawnRoutine);
                    spawnRoutine = null;
                }
            }
            else if (currentEnemyCount < maxEnemies && !canSpawn)
            {
                canSpawn = true;
                spawnRoutine = StartCoroutine(SpawnEnemiesRoutine());
            }
        }
        
        private IEnumerator SpawnEnemiesRoutine()
        {
            while (true)
            {
                int enemyCount = Random.Range(1, 3);
                for (int i = 0; i < enemyCount; i++)
                {
                    SpawnEnemyFromEdge();
                    yield return new WaitForSeconds(0.2f);
                }
                yield return new WaitForSeconds(spawnInterval);
            }
        }
        private void SpawnEnemyFromEdge()
        {
            if (corners.Length != 4 || enemyPrefabs.Length == 0) return;
            GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

            //Select random edge (0-1, 1-2, 2-3, 3-0)
            int edgeIndex = Random.Range(0, 4);
            Transform startCorner = corners[edgeIndex];
            Transform endCorner = corners[(edgeIndex + 1) % 4];
            
            float t = Random.Range(0f, 1f);
            Vector3 spawnPos = Vector3.Lerp(startCorner.position, endCorner.position, t);
            
            var enemy = enemyPrefab.Reuse();
            spawnedEnemyCount++;
            enemy.transform.position = spawnPos;
        }
    }
}

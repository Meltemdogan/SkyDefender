using _Game.Scripts.Enemies.Interfaces;
using UnityEngine;
using System.Collections;
using _Game.Scripts.HealthBar;
using ToolBox.Pools;


namespace _Game.Scripts.Enemies
{
    public class EnemyKamikaze : BaseEnemy
    {
        [Header("Kamikaze Settings")]
        [SerializeField] private float detectionRange = 5f;
        [SerializeField] private float chargeSpeed = 8f;

        private bool isCharging = false;
        private bool isWandering = false;
        private Vector2 movementDirection;

        protected override void Start()
        {
            base.Start();
            SetDirectionToCity();
        }
        [SerializeField] private GameObject explosionEffect;
        protected override void OnTriggerEnter2D(Collider2D other)
        {
            base.OnTriggerEnter2D(other);
            
            if (isCharging && other.CompareTag("Player"))
            {
                if (other.TryGetComponent<IDamageable>(out var playerTarget))
                {
                    playerTarget.TakeDamage(projectileData.Damage);
                    Instantiate(explosionEffect, other.transform.position, Quaternion.identity);
                    
                    Die();
                }
            }
        }
        protected override void OnTriggerExit2D(Collider2D other)
        {
            base.OnTriggerExit2D(other);
            if (!gameObject.activeInHierarchy) return;
            
            if (other.CompareTag("Map") && !isWandering)
            {
                StartCoroutine(WanderThenReturnToCity());
            }
        }

        protected override void MoveBehavior()
        {
            if (isWandering) return;

            if (!isCharging)
            {
                Patrol();
                CheckForPlayer();
            }
            else
            {
                ChargeAtPlayer();
            }
        }

        private void Patrol()
        {
            SmoothRotateTowards(movementDirection);
            transform.position += transform.up * MoveSpeed * Time.deltaTime;
        }

        private void CheckForPlayer()
        {
            if (player == null) return;

            float distance = Vector2.Distance(transform.position, player.position);
            if (distance <= detectionRange)
            {
                isCharging = true;
            }
        }

        private void ChargeAtPlayer()
        {
            if (player == null) return;

            Vector2 dirToPlayer = (player.position - transform.position).normalized;
            SmoothRotateTowards(dirToPlayer);
            transform.position += transform.up * chargeSpeed * Time.deltaTime;
        }
        private IEnumerator WanderThenReturnToCity()
        {
            isWandering = true;

            Vector2 randomDir = Random.insideUnitCircle.normalized;
            float wanderTime = 4f;
            float timer = 0f;

            while (timer < wanderTime)
            {
                timer += Time.deltaTime;
                SmoothRotateTowards(randomDir);
                transform.position += transform.up * MoveSpeed * Time.deltaTime;
                yield return null;
            }

            SetDirectionToCity();
            isWandering = false;
        }

        private void SetDirectionToCity()
        {
            movementDirection = (CityController.Instance.transform.position - transform.position).normalized;
            SmoothRotateTowards(movementDirection);
        }
        
        public override void TakeDamage(int amount)
        {
            if (!healthSystem.IsAlive) return;
            healthSystem.DecreaseCurrentHealthBy(amount);
            if (!healthSystem.IsAlive)
            {
                Die();
            }
        }
        protected override void OnDeath()
        {
            base.OnDeath();
            
        }
    }
}

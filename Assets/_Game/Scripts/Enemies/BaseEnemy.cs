using System;
using System.Collections;
using _Game.Scripts.Enemies.Interfaces;
using _Game.Scripts.HealthBar;
using _Game.Scripts.Projectiles;
using _Game.Scripts.Spawner;
using CartoonFX;
using DG.Tweening;
using ToolBox.Pools;
using UnityEngine;


namespace _Game.Scripts.Enemies
{
    public abstract class BaseEnemy : MonoBehaviour, IDamageable, IKillable, IEnemyStats
    {
        [Header("Stats")] [SerializeField] protected float moveSpeed;
        [SerializeField] protected float rotationSpeed;
        public float MoveSpeed => moveSpeed;
        public float RotationSpeed => rotationSpeed;
        
        [Header("Movement Settings")]
        [SerializeField]
        protected float maxTurnSpeed = 60f; // degrees per second
        
        [Header("Coroutine Settings")] protected bool canFire = true;
        protected Coroutine fireCoroutine;
        
        [Header("Projectile Settings")]
        [SerializeField]
        protected GameObject projectilePrefab;
        [SerializeField] protected Transform firePoint;
        
        [SerializeField] protected ProjectileData projectileData;
        [SerializeField] protected HealthBarController healthSystem;
        
        protected Transform player;
        
        protected virtual void Start()
        {
            projectileData.OwnerTag = gameObject.tag; // Set the owner tag for the projectile
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
            Minimap.Instance.RegisterEnemy(transform);
        }
        
        [SerializeField] private Vector3 initialScale = Vector3.one * 0.5f;
        
        private void OnEnable()
        {
            transform.localScale = initialScale;
        }
        
        protected virtual void Update()
        {
            MoveBehavior();
        }
        
        public virtual void TakeDamage(int amount) 
        { }
        protected abstract void MoveBehavior();
        
        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("PlayerBullet"))
            {
                TakeDamage(1); ;
            }
        }
        
        protected virtual void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("TileMap"))
            {
                gameObject.Release();
                EnemySpawner.Instance.currentEnemyCount--;
                
            }
        }
        
        public void Die()
        {
            OnDeath();
        }
        
        public static event Action<BaseEnemy> OnEnemyDied; 
        protected virtual void OnDeath()
        {
            Debug.Log($"{gameObject.name} has been destroyed.");
            Minimap.Instance.UnregisterEnemy(transform);
            DieAnimation();
            OnEnemyDied?.Invoke(this);
            
        }
        
        [SerializeField] private ReturnPoolAfterAnimation explosionFx;
        
        [ContextMenu("Die Anim")]
        protected virtual void DieAnimation()
        {
            transform.DORotate(Vector3.forward * 180f, 0.7f);
            transform.DOScale(Vector3.one * 0.1f, 0.7f)
                .OnComplete(() =>
                {
                    PlayHitEffect();
                    gameObject.Release();
                });
        }
        
        [SerializeField] private CFXR_Effect ExplosionParticle;
        
        private void PlayHitEffect()
        {
            if (ExplosionParticle == null) return;
            var fx = Instantiate(ExplosionParticle, transform.position, Quaternion.identity);
            fx.Animate(5f);
        }
        
        protected void SmoothRotateTowards(Vector2 direction)
        {
            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            float currentAngle = transform.eulerAngles.z;
            float angle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, maxTurnSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
        
        protected virtual void Shoot() { }
        
        protected IEnumerator FireRoutine(float fireInterval)
        {
            while (canFire)
            {
                Shoot();
                yield return new WaitForSeconds(fireInterval);
            }
        }
        
        protected virtual void OnDisable()
        {
            if (fireCoroutine != null)
            {
                StopCoroutine(fireCoroutine);
                fireCoroutine = null;
            }
            
            canFire = false;
        }
    }
}
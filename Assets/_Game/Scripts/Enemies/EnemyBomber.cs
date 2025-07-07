using System.Collections;
using _Game.Scripts.Enemies.Interfaces;
using _Game.Scripts.Projectiles;
using ToolBox.Pools;
using UnityEngine;


namespace _Game.Scripts.Enemies
{
    public class EnemyBomber : BaseEnemy
    {
        [Header("Explosion Settings")] private Vector2 movementDirection;
        
        protected override void Start()
        {
            base.Start();
            
            movementDirection = (CityController.Instance.transform.position - transform.position).normalized;
            float angle = Mathf.Atan2(movementDirection.y, movementDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
        }
        
        protected override void MoveBehavior()
        {
            transform.position += (Vector3)movementDirection * MoveSpeed * Time.deltaTime;
        }
        
        protected override void OnTriggerEnter2D(Collider2D other)
        {
            base.OnTriggerEnter2D(other);
            if (other.CompareTag("CityZone"))
            {
                if (fireCoroutine == null)
                {
                    canFire = true;
                    fireCoroutine = StartCoroutine(FireRoutine(2f));
                }
            }
        }
        protected override void OnTriggerExit2D(Collider2D other)
        {
            base.OnTriggerExit2D(other);
            if (other.CompareTag("CityZone"))
            {
                canFire = false;
                if (fireCoroutine != null)
                {
                    StopCoroutine(fireCoroutine);
                    fireCoroutine = null;
                }
            }
        }
        
        protected override void Shoot()
        {
            if (canFire)
            {
                var bombObj = projectilePrefab.Reuse(transform.position, transform.rotation);
                if (bombObj.TryGetComponent<IProjectile>(out var bomb))
                {
                    bomb.Initialize(projectileData);
                }
            }
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
    }
}
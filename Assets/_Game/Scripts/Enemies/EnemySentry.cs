using _Game.Scripts.Enemies.Interfaces;
using UnityEngine;


namespace _Game.Scripts.Enemies
{
    public class EnemySentry: BaseEnemy
    {
        private Vector3 centerPos;
        
        protected override void Start()
        {
            centerPos = transform.position;
        }
        protected override void MoveBehavior()
        {
            float distance = Vector2.Distance(transform.position, player.position);
            Vector2 dirToPlayer = (player.position - transform.position).normalized;
        }
        protected override void OnTriggerEnter2D(Collider2D other)
        {
            base.OnTriggerEnter2D(other);
            if (other.CompareTag("CityZone"))
            {
                if (other.TryGetComponent<IDamageable>(out var city))
                {
                    city.TakeDamage(1);
                }
            }
        }
        private void SetDirectionToPoint(Vector3 position)
        {
            Vector3 direction = (position - transform.position).normalized;
            SmoothRotateTowards(direction);
            
        }
    }
}
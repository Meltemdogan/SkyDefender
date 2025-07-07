using System;
using _Game.Scripts.Enemies.Interfaces;
using ToolBox.Pools;
using UnityEngine;

namespace _Game.Scripts.Projectiles
{
    public class Bomb : MonoBehaviour, IProjectile
    {
       [SerializeField] private ProjectileData projectileData;
       [SerializeField] private GameObject explosionPrefab;
       
       private Vector3 startPosition;
        private float time;
        public void Initialize(ProjectileData data)
        {
            projectileData = data;
            startPosition = transform.position;
        }
        
        private void Update()
        {
            transform.Translate(Vector2.down * (projectileData.Speed * Time.deltaTime));
            
            BombBehavior();
        }
        
        public void BombBehavior()
        {
            float traveledDistance = Vector3.Distance(startPosition, transform.position);
            float t = Mathf.Clamp01(traveledDistance / projectileData.Range);
            float scale = Mathf.Lerp(0.2f, 0.005f, t);
            transform.localScale = Vector3.one * scale;
            
            if (traveledDistance >= projectileData.Range)
            {
                transform.localScale = Vector3.one * 0.005f;
                gameObject.Release();
                GameObject explosion = explosionPrefab.Reuse(transform.position, Quaternion.identity);
            }
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(projectileData.OwnerTag))
                return;
            if (other.TryGetComponent<IDamageable>(out var target))
            {
                target.TakeDamage(projectileData.Damage);
                
            }
        }
    }
}
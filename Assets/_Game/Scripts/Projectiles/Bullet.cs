using System;
using _Game.Scripts.Enemies.Interfaces;
using CartoonFX;
using ToolBox.Pools;
using Unity.VisualScripting;
using UnityEngine;
namespace _Game.Scripts.Projectiles
{
    public class Bullet : MonoBehaviour, IProjectile
    {
        [SerializeField] private ProjectileData projectileData;
        [SerializeField] private CFXR_Effect impactEffectPrefab;
        
        private Vector2 startPosition;
        
        public void Initialize(ProjectileData data)
        {
            projectileData = data;
            startPosition = transform.position;
        }
        
        private void Update()
        {
            transform.Translate(Vector2.up * (projectileData.Speed * Time.deltaTime));
            
            float traveledDistance = Vector3.Distance(startPosition, transform.position);
            if (traveledDistance >= projectileData.Range)
            {
                gameObject.Release();
            }
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(projectileData.OwnerTag)) 
                return;
            if (other.CompareTag("CityZone"))
                return;
            if (other.TryGetComponent<IDamageable>(out var target))
            {
                target.TakeDamage(projectileData.Damage);
                PlayHitEffect();
            }
        }
        
        private void PlayHitEffect()
        {
            if(impactEffectPrefab == null) return;
            var fx = Instantiate(impactEffectPrefab, transform.position, Quaternion.identity);
            fx.Animate(3f);
        }
    }
}
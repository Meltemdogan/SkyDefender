using _Game.Scripts.Projectiles;
using UnityEngine;
namespace _Game.Scripts.Enemies.Interfaces
{
    public interface IProjectile
    {
        public void Initialize(ProjectileData data);
    }
}
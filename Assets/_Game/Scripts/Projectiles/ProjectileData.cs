using System;


namespace _Game.Scripts.Projectiles
{
    [Serializable]
    public class ProjectileData
    {
        public float Speed;
        public float Range; 
        public int Damage;
        public string OwnerTag;
    }
}
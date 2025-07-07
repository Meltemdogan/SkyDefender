using UnityEngine;


namespace _Game.Scripts.Enemies.Interfaces
{
    public interface IEnemyStats
    {
        float MoveSpeed { get; }
        float RotationSpeed { get; }
    }
}
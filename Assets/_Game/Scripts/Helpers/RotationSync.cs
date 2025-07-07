using System;
using UnityEngine;


namespace _Game.Scripts.Helpers
{
    public class RotationSync: MonoBehaviour
    {
        private void Update()
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
using System;
using UnityEngine;


namespace _Game.Scripts.City
{
    public class Test: MonoBehaviour
    {
        [SerializeField] private GameObject explosionPrefab;
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            }
            
        }
    }
}
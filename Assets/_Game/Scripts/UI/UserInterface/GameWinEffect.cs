using UnityEngine;


namespace _Game.Scripts.UI.UserInterface
{
    public class GameWinEffect : MonoBehaviour
    {
        [SerializeField]  GameObject winEffectPrefab;
        [SerializeField] Transform effectSpawnPoint;
        private void Start()
        {
            GameObject effectInstance = Instantiate(winEffectPrefab, effectSpawnPoint.position, Quaternion.identity);
            Destroy(effectInstance, 3f); // Effect will be destroyed after 3 seconds
            Debug.Log("Game Win Effect Triggered");
        }
    }
}
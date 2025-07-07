using UnityEngine;
using UnityEngine.UI;


namespace _Game.Scripts.UI.UserInterface
{
    public class UIHandler
    {
        public static UIHandler Instance { get; private set; }
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
        }
        
        [Header("Menus")]
        [SerializeField] GameObject mainMenu;
        [SerializeField] GameObject gameOverMenu;
        [SerializeField] GameObject gameWinMenu;
        
        [Header("Buttons")]
        [SerializeField] Button startButton;
        [SerializeField] Button restartButton;
        [SerializeField] Button exitButton;
    }
}
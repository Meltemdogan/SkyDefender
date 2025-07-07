using System;
using _Game.Scripts.Enemies;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI Panels")]
    [SerializeField] private GameObject mainMenuUI;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject gameWinUI;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    
    private void OnEnable()
    {
      
    }
    
    private void OnDisable()
    {
        PlayerController.Instance.OnOutOfBullets -= GameOver;
        PlayerController.Instance.OnPlayerDeath -= GameOver;
        CityController.Instance.OnCityDestroyed -= GameOver;
        TargetManager.Instance.OnTargetCompleted -= GameWin;
    }
    
    private void Start()
    {
        PlayerController.Instance.OnOutOfBullets += GameOver;
        PlayerController.Instance.OnPlayerDeath += GameOver;
        CityController.Instance.OnCityDestroyed += GameOver;
        TargetManager.Instance.OnTargetCompleted += GameWin;
        ShowMainMenu();
    }
    public void ShowMainMenu()
    {
        mainMenuUI.SetActive(true);
        gameOverUI.SetActive(false);
        gameWinUI.SetActive(false);

        Time.timeScale = 0f; 
    }

    public void StartGame()
    {
        mainMenuUI.SetActive(false);
        gameOverUI.SetActive(false);
        gameWinUI.SetActive(false);
        Time.timeScale = 1f; 
    }
    public void GameOver()
    {
        gameOverUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void GameWin()
    {
        gameWinUI.SetActive(true);
        Time.timeScale = 0f;
    }
    public void OnStartButton()
    {
        StartGame();
    }

    public void OnRestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }

    public void OnQuitButton()
    {
        Application.Quit();
    }
}

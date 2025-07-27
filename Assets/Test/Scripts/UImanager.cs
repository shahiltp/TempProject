using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UImanager : MonoBehaviour
{
    

    [Header("Panels")]
    public GameObject mainMenuPanel;
    public GameObject gameUIPanel;
    public GameObject gameOverPanel;

    [Header("Main Menu")]
    public Toggle[] gridSizeToggles;
    public Button playButton;

    [Header("Game UI")]
    public TextMeshProUGUI scoreText;
    public Button restartButton;

    [Header("Game Over UI")]
    public TextMeshProUGUI finalScoreText;
    public Button mainMenuButton;
    // Start is called before the first frame update
    void Start()
    {

    }
    void Awake()
    {
        playButton.onClick.AddListener(OnPlayClicked);
        restartButton.onClick.AddListener(OnRestartClicked);
        mainMenuButton.onClick.AddListener(OnMainMenuClicked);
    }

    private void OnEnable()
    {
        GameManager.OnGameStateChanged += HandleGameStateChanged;
        GameBoard.OnMatchFound += HandleScoreUpdated;
        GameManager.OnGameRestarted += HandleGameRestarted;
    }

    private void OnDisable()
    {
        GameManager.OnGameStateChanged -= HandleGameStateChanged;
        GameBoard.OnMatchFound -= HandleScoreUpdated;
        GameManager.OnGameRestarted -= HandleGameRestarted;
    }

    private void HandleGameStateChanged(GameManager.GameState newState)
    {
        mainMenuPanel.SetActive(newState == GameManager.GameState.MainMenu);
        gameUIPanel.SetActive(newState == GameManager.GameState.InGame);
        gameOverPanel.SetActive(newState == GameManager.GameState.GameOver);

        if (newState == GameManager.GameState.GameOver)
        {
            finalScoreText.text = "Final Score: " + GameManager.Instance.Score;
        }

        if (newState == GameManager.GameState.InGame)
        {
            scoreText.text = "Score: 0";
        }
    }

    private void HandleScoreUpdated(int points)
    {
        scoreText.text = "Score: " + GameManager.Instance.Score;
    }

    private void HandleGameRestarted()
    {
        scoreText.text = "Score: 0";
    }

    private void OnPlayClicked()
    {
        int rows = 2, cols = 2;
        if (gridSizeToggles[0].isOn) { rows = 2; cols = 2; }
        else if (gridSizeToggles[1].isOn) { rows = 2; cols = 3; }
        else if (gridSizeToggles[2].isOn) { rows = 5; cols = 6; }

        GameManager.Instance.StartGame(rows, cols);
    }

  
}

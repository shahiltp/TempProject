using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonDestuction<GameManager>
{
    internal int rows;
    internal int cols;

    public enum GameState { MainMenu, InGame, GameOver }
    public GameState CurrentState { get; private set; }

    public int Score { get; private set; }
    //we have 3 actions should be there listening  TODO
    public static event Action<GameState> OnGameStateChanged;

    public static event Action<int, int> OnGameStarted; 
    public static event Action OnGameRestarted;

    public static event Action<int> OnScoreChanged;

    
    
    // Start is called before the first frame update
    void Start()
    {
        UpdateState(GameState.MainMenu);
    }

    private void OnEnable()
    {
        GameBoard.OnMatchFound += HandleMatchFound;
        GameBoard.OnAllMatchesFound += HandleAllMatchesFound;
    }

    private void OnDisable()
    {
        GameBoard.OnMatchFound -= HandleMatchFound;
        GameBoard.OnAllMatchesFound -= HandleAllMatchesFound;
    }

    public void StartGame(int row, int col)
    {
        rows = row;
        cols = col;
        UpdateScore(0);
        UpdateState(GameState.InGame);
        OnGameStarted?.Invoke(rows, cols);
    }

    public void RestartGame()
    {
        UpdateScore(0); 
        UpdateState(GameState.InGame);
        OnGameRestarted?.Invoke();
    }

    public void ReturnToMenu()
    {
        UpdateState(GameState.MainMenu);
    }

    private void HandleMatchFound(int points)
    {
        UpdateScore(Score + points);
    }
    
    private void UpdateScore(int newScore)
    {
        Score = newScore;
        OnScoreChanged?.Invoke(Score);
    }

    private void HandleAllMatchesFound()
    {
        UpdateState(GameState.GameOver);
    }

    private void UpdateState(GameState newState)
    {
        CurrentState = newState;
        OnGameStateChanged?.Invoke(newState);
    }
}

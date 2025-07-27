using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonDestuction<GameManager>
{

    public enum GameState { MainMenu, InGame, GameOver }
    public GameState CurrentState { get; private set; }

    public int Score { get; private set; }
    //we have 3 actions should be there listening  TODO
    public static event Action<GameState> OnGameStateChanged;

    public static event Action<int, int> OnGameStarted; 
    public static event Action OnGameRestarted;

    // Start is called before the first frame update
    void Start()
    {
        UpdateState(GameState.MainMenu);
    }

    private void OnEnable()
    {

    }

    private void OnDisable()
    {

    }

    public void RestartGame()
    {
        Score = 0;
        UpdateState(GameState.InGame);
        OnGameRestarted?.Invoke();
    }

    public void ReturnToMenu()
    {
        UpdateState(GameState.MainMenu);
    }

    private void HandleMatchFound(int points)
    {
        Score += points;
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

    public void StartGame(int rows, int cols)
    {
        Score = 0;
        UpdateState(GameState.InGame);
        OnGameStarted?.Invoke(rows, cols);
    }
}

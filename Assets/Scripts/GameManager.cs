using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameState State;

    public static event Action<GameState> onGameStateChanged;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        UpdateGameState(GameState.GameOver);
    }

    public void UpdateGameState(GameState newState)
    {
        State = newState;

        switch (newState)
        {
            case GameState.GameOver:
                HandleGameOver();
                break;
        }

        onGameStateChanged?.Invoke(newState);
    }

    private void HandleGameOver()
    {

    }
}

public enum GameState
{
    GameOver
}

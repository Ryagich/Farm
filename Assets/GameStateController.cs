using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameStateController : MonoBehaviour
{
    public static GameStateController Instance;
    public UnityEvent<GameStates> ChangedGameState;
    public GameStates GameState { get; private set; }
    public bool IsStanding { get; set; }

    private void Awake()
    {
        Instance = this;
    }

    public void ChangeState()
    {
        switch (GameState)
        {
            case GameStates.Game:
                GameState = GameStates.Redactor;
                ChangedGameState?.Invoke(GameStates.Redactor);
                break;
            case GameStates.Redactor:
                GameState = GameStates.Game;
                ChangedGameState?.Invoke(GameStates.Game);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(GameState));
        }
    }
}

public enum GameStates
{
    Game,
    Redactor,
}
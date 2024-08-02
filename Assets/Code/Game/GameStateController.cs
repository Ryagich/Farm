using System;
using UniRx;

namespace Code.Game
{
    public class GameStateController
    {
        public ReactiveProperty<GameStates> GameState { get; private set; } = new();
        public bool IsMoving { get; set; }

        public void ChangeState()
        {
            switch (GameState.Value)
            {
                case GameStates.Game:
                    GameState.Value = GameStates.Redactor;
                    break;
                case GameStates.Redactor:
                    GameState.Value = GameStates.Game;
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
}
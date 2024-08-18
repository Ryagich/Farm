using System;
using UniRx;

namespace Code.Game
{
    public class GameStateController
    {
        public Action UIElementClicked;
        public ReactiveProperty<GameStates> GameState { get; private set; } = new();
        public bool IsMoving { get; set; }

        public void ChangeState(GameStates newState)
        {
            GameState.Value = newState;
        }
    }

    public enum GameStates
    {
        Game,
        Redactor,
        Building,
        Expansion
    }
}
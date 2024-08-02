using Code.Game;
using UnityEngine.UIElements;

namespace Code.UI
{
    public class ToolsView
    {
        private ToolsView(UIDocument screen,GameStateController gameStateController)
        {
            var openButton = screen.rootVisualElement.Q<VisualElement>("Open_Button");
            openButton.RegisterCallback<ClickEvent>(_ => gameStateController.ChangeState());
        }
    }
}
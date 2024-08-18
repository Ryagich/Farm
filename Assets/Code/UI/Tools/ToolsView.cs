using System.Collections.Generic;
using Code.Game;
using UnityEngine.UIElements;

namespace Code.UI.Tools
{
    public class ToolsView
    {
        private ToolsSettings settings;
        private VisualElement tools;
        
        private ToolsView(UIDocument screen,ToolsSettings settings, GameStateController gameStateController)
        {
            this.settings = settings;
            tools = screen.rootVisualElement.Q<VisualElement>("Tools_Root");
            var exitButton = screen.rootVisualElement.Q<VisualElement>("Exit_Button");
            var buildingButton = screen.rootVisualElement.Q<VisualElement>("Building_Button");
            var expansionButton = screen.rootVisualElement.Q<VisualElement>("Expansion_Button");
            
            exitButton.RegisterCallback<ClickEvent>(_ =>
            {
                if (gameStateController.GameState.Value is GameStates.Game)
                {
                    gameStateController.ChangeState(GameStates.Redactor);
                    ShowPanel(settings.MaxHeight);
                }
                else
                {
                    gameStateController.ChangeState(GameStates.Game);
                    ShowPanel(settings.MinHeight);
                }
                gameStateController.UIElementClicked?.Invoke();
            });
            
            buildingButton.RegisterCallback<ClickEvent>(_ =>
                                                        {
                                                            gameStateController.ChangeState(GameStates.Building);
                                                            gameStateController.UIElementClicked?.Invoke();
                                                        });
            expansionButton.RegisterCallback<ClickEvent>(_ =>
                                                         {
                                                             gameStateController.ChangeState(GameStates.Expansion);
                                                             gameStateController.UIElementClicked?.Invoke();
                                                         });
        }
    
        private void ShowPanel(float size)
        {
            tools.style.transitionDuration = new List<TimeValue> { new(settings.Translation, TimeUnit.Second) };
            tools.style.transitionTimingFunction = new StyleList<EasingFunction>(new List<EasingFunction> { new (settings.Easing) });
            tools.style.height = size;
        }
    }
}
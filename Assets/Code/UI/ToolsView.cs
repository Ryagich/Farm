using UnityEngine;
using UnityEngine.UIElements;

public class ToolsView : MonoBehaviour
{
    [SerializeField] private UIDocument _screen;

    private void Start()
    {
        var openButton = _screen.rootVisualElement.Q<VisualElement>("Open_Button");
        openButton.RegisterCallback<ClickEvent>(_ => GameStateController.Instance.ChangeState());
    }
}
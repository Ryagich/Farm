using UnityEngine;

[RequireComponent(typeof(InterfaceHider))]
public class InterfaceController : MonoBehaviour
{
    private InterfaceHider hider;

    private void Awake()
    {
        hider ??= GetComponent<InterfaceHider>();
        GetComponent<MoneyConsumer>().Bought += Disable;
        GetComponent<PlantGrower>().GrowUp += (_) => hider.ChangeSelectedState(true);
    }

    public void Activate()
    {
        hider ??= GetComponent<InterfaceHider>();
        hider.ChangeBannerState(true);
        hider.ChangeSelectedState(true);
    }

    public void Disable()
    {
        hider.ChangeBannerState(false);
        hider.ChangeSelectedState(false);
    }
}

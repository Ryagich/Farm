using Zenject;
using UnityEngine;
using UnityEngine.UIElements;

namespace Code.UI.Installers
{
    public class UIInstaller : MonoInstaller
    {
        [SerializeField] private UIDocument _screen;

        public override void InstallBindings()
        {
            Container.Bind<UIDocument>().FromInstance(_screen).AsSingle();
        
            Container.Bind<ToolsView>().FromNew().AsSingle().NonLazy();
        }
    }
}
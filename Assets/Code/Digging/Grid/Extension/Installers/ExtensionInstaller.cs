using UnityEngine;
using Zenject;

namespace Code.Digging.Grid.Extension.Installers
{
    public class ExtensionInstaller : MonoInstaller
    {
        [SerializeField] private GridExtensionSettings _extensionSettings;

        public override void InstallBindings()
        {
            Container.Bind<GridExtensionSettings>().FromInstance(_extensionSettings).AsSingle();
            Container.Bind<GridExtensionSpawner>().FromNew().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<ExtensionFounder>()
                     .FromNew()
                     .AsSingle();
        }
    }
}
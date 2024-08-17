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
            Container.Bind<GridExtension>().FromNew().AsSingle().NonLazy();
            var founder = new GameObject("ExtensionFounder");
            Container.Bind<ExtensionFounder>()
                     .FromNewComponentOn(founder)
                     .AsSingle()
                     .NonLazy();
        }
    }
}
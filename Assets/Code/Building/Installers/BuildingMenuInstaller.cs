using UnityEngine;
using Zenject;

namespace Code.Building.Installers
{
    public class BuildingMenuInstaller : MonoInstaller
    {
        [SerializeField] private BuildingMenuConfig _config;
        
        public override void InstallBindings()
        {
            Container.Bind<BuildingMenuConfig>().FromInstance(_config).AsSingle();
            Container.Bind<BuildingMenu>().FromNew().AsSingle().NonLazy();
        }
    }
}
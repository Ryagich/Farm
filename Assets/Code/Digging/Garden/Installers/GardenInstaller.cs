using Code.Digging.Garden;
using UnityEngine;
using Zenject;

namespace Code.Garden.Installers
{
    public class GardenInstaller : MonoInstaller
    {
        [SerializeField] private GardensInfo _info;

        public override void InstallBindings()
        {
            Container.Bind<GardensInfo>().FromInstance(_info).AsSingle();
            Container.Bind<GardenSpawner>().FromNew().AsSingle();
        }
    }
}
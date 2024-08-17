using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Code.Materials.Installers
{
    public class GlobalSettingsInstaller : MonoInstaller
    {
        [SerializeField] private MaterialsConfig _materials;
        [SerializeField] private LayersConfig _layers;

        public override void InstallBindings()
        {
            Container.Bind<MaterialsConfig>().FromInstance(_materials).AsSingle();
            Container.Bind<LayersConfig>().FromInstance(_layers).AsSingle();
        }
    }
}
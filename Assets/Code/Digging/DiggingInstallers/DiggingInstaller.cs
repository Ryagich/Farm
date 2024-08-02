using Zenject;
using UnityEngine;

namespace Code.Digging.DiggingInstallers
{
    public class DiggingInstaller : MonoInstaller
    {
        [SerializeField] private DiggingConfig _config;

        public override void InstallBindings()
        {
            Container.Bind<DiggingConfig>().FromInstance(_config).AsSingle();

            var digging = new GameObject("Digging");
            Container.Bind<DiggingController>()
                     .FromNewComponentOn(digging)
                     .AsSingle()
                     .NonLazy();
        }
    }
}
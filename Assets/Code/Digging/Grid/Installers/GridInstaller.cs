using Code.Digging.Grid;
using Zenject;
using UnityEngine;

namespace Code.Grid.Installers
{
    public class GridInstaller : MonoInstaller
    {
        [SerializeField] private GridSettings _settings;

        public override void InstallBindings()
        {
            Container.Bind<GridSettings>().FromInstance(_settings).AsSingle();
            Container.Bind<VisualGridSeparation>().FromNew().AsSingle();
            Container.Bind<GridVisualizationForGarden>().FromNew().AsSingle();
            Container.Bind<GridController>().FromNew().AsSingle();
        }
    }
}
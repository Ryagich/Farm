using Zenject;

namespace Code.Game.Installers
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<GameStateController>().FromNew().AsSingle();
        }
    }
}
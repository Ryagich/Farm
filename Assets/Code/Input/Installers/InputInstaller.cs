using UnityEngine;
using Zenject;

namespace Code.Input.Installers
{
    public class InputInstaller : MonoInstaller
    {
        [SerializeField] private InputKeys _keys;

        public override void InstallBindings()
        {
            Container.Bind<InputKeys>().FromInstance(_keys).AsSingle();
            Container.Bind<InputHandler>().FromNew().AsSingle();
        }
    }
}
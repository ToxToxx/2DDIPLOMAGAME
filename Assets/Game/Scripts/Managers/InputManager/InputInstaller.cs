using UnityEngine;
using Zenject;
using UnityEngine.InputSystem;

namespace InGameInput
{
    public class InputInstaller : MonoInstaller
    {
        [SerializeField] private PlayerInput _playerInput;  // ссылка на компонент PlayerInput

        public override void InstallBindings()
        {
            Container.Bind<PlayerInput>().FromInstance(_playerInput).AsSingle();
            Container.BindInterfacesAndSelfTo<InputService>().AsSingle();
        }
    }
}
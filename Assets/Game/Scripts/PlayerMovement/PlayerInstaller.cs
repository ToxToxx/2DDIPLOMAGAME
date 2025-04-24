using UnityEngine;
using Zenject;
using PlayerMovementLogic;

public class PlayerInstaller : MonoInstaller
{
    [SerializeField] private PlayerMovementModel _model;

    public override void InstallBindings()
    {
        // Bind PlayerMovementModel as a single instance
        Container.Bind<PlayerMovementModel>().FromInstance(_model).AsSingle();

        // Bind Rigidbody2D from this GameObject
        Container.Bind<Rigidbody2D>().FromComponentOn(gameObject).AsSingle();

        // Bind Transform from this GameObject
        Container.Bind<Transform>().FromComponentOn(gameObject).AsSingle();

        // Bind PlayerMovementController as a single instance, also as ITickable and IFixedTickable
        Container.BindInterfacesAndSelfTo<PlayerMovementController>().AsSingle().NonLazy();
    }
}
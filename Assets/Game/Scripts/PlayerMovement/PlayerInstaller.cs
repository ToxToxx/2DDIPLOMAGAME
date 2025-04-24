using UnityEngine;
using Zenject;
using PlayerMovementLogic;
using PlayerAttackLogic;
using PlayerInteractionLogic;

public class PlayerInstaller : MonoInstaller
{
    [Header("Movement")]
    [SerializeField] private PlayerMovementModel _model;


    [Header("Interaction")]
    [SerializeField] private float _interactionRadius = 1.5f;
    [SerializeField] private LayerMask _interactableLayer;


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

        // Bind PlayerAttack as a single instance, also as ITickable
        Container.BindInterfacesAndSelfTo<PlayerAttack>().AsSingle().NonLazy();

        // Bind PlayerInteraction as a single instance, also as ITickable, with parameters
        Container.BindInterfacesAndSelfTo<PlayerInteraction>()
            .AsSingle()
            .WithArguments(_interactionRadius, _interactableLayer)
            .NonLazy();
    }
}
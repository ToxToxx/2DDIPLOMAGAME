using UnityEngine;
using Zenject;
using PlayerMovementLogic;
using PlayerAttackLogic;
using PlayerInteractionLogic;
using PlayerAnimationLogic;

public class PlayerInstaller : MonoInstaller
{
    [Header("Movement")]
    [SerializeField] private PlayerMovementModel _model;

    [Header("Interaction")]
    [SerializeField] private float _interactionRadius = 1.5f;
    [SerializeField] private LayerMask _interactableLayer;

    [Header("Animation")]
    [SerializeField] private PlayerAnimationController _animationController;

    public override void InstallBindings()
    {
        // Bind Event Bus
        Container.BindInterfacesAndSelfTo<PlayerEventBus>().AsSingle();

        // Bind PlayerMovementModel
        Container.Bind<PlayerMovementModel>().FromInstance(_model).AsSingle();

        // Bind Rigidbody2D and Transform
        Container.Bind<Rigidbody2D>().FromComponentOn(gameObject).AsSingle();
        Container.Bind<Transform>().FromComponentOn(gameObject).AsSingle();

        // Bind PlayerMovementController
        Container.BindInterfacesAndSelfTo<PlayerMovementController>().AsSingle().NonLazy();

        // Bind PlayerAttack
        Container.BindInterfacesAndSelfTo<PlayerAttack>().AsSingle().NonLazy();

        // Bind PlayerInteraction
        Container.BindInterfacesAndSelfTo<PlayerInteraction>()
            .AsSingle()
            .WithArguments(_interactionRadius, _interactableLayer)
            .NonLazy();

        // Bind PlayerAnimationController
        Container.Bind<PlayerAnimationController>().FromInstance(_animationController).AsSingle()
            .OnInstantiated<PlayerAnimationController>((ctx, controller) =>
            {
                var movementModel = ctx.Container.Resolve<PlayerMovementModel>();
                var attack = ctx.Container.Resolve<PlayerAttack>();
                var eventNotifier = ctx.Container.Resolve<IPlayerEventNotifier>();
                controller.Initialize(movementModel, attack, eventNotifier);
            });
    }

}

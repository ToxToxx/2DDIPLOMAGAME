using UnityEngine;
using Zenject;
using PlayerMovementLogic;
using PlayerAttackLogic;
using PlayerInteractionLogic;
using PlayerAnimation;
using PlayerEvent;
using PlayerAudio;

namespace Player
{
    public class PlayerInstaller : MonoInstaller
    {
        [Header("Movement")]
        [SerializeField] private PlayerMovementModel _model;

        [Header("Interaction")]
        [SerializeField] private float _interactionRadius = 1.5f;
        [SerializeField] private LayerMask _interactableLayer;

        [Header("Animation")]
        [SerializeField] private PlayerAnimationController _animationController;

        [Header("Attack")]
        [SerializeField] private LayerMask _enemyLayer;

        [Header("Audio")]
        [SerializeField] private PlayerAudio.PlayerAudioController _audioController;

        public override void InstallBindings()
        {
            // Bind Event Bus
            Container.BindInterfacesAndSelfTo<PlayerEventBus>().AsSingle();

            // Bind Movement Model
            Container.Bind<PlayerMovementModel>().FromInstance(_model).AsSingle();

            // Rigidbody and Transform
            Container.Bind<Rigidbody2D>().FromComponentOn(gameObject).AsSingle();
            Container.Bind<Transform>().FromComponentOn(gameObject).AsSingle();

            // Bind Systems
            Container.BindInterfacesAndSelfTo<PlayerMovementController>().AsSingle().NonLazy();

            Container.BindInterfacesAndSelfTo<PlayerAttack>()
         .AsSingle()
         .WithArguments(_enemyLayer)
         .NonLazy();

            Container.BindInterfacesAndSelfTo<PlayerInteraction>()
                .AsSingle()
                .WithArguments(_interactionRadius, _interactableLayer)
                .NonLazy();

            // Animation Controller (Inject will handle Construct)
            Container.Bind<PlayerAnimationController>().FromInstance(_animationController).AsSingle();

            Container.BindInterfacesAndSelfTo<PlayerAudioController>()
              .FromComponentOn(_audioController.gameObject)   // <- компонент из инспектора
              .AsSingle();
        }
    }
}
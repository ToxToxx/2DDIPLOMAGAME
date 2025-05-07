using UnityEngine;
using Zenject;
using PlayerMovement;
using PlayerAttack;
using PlayerInteractionLogic;
using PlayerAnimation;
using PlayerEvent;
using PlayerAudio;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerInstaller : MonoInstaller
    {

        /* ──────────── Serialized refs ──────────── */
        [Header("Input")]
        [SerializeField] private PlayerInput _playerInput;

        [Header("Movement")]
        [SerializeField] private PlayerMovementModel _movementModel;

        [Header("Interaction")]
        [SerializeField] private float _interactionRadius = 1.5f;
        [SerializeField] private LayerMask _interactableLayer;

        [Header("Attack")]
        [SerializeField] private LayerMask _enemyLayer;
        [SerializeField] private PlayerAttackStats _attackStats;

        [Header("Animation")]
        [SerializeField] private Animator _playerAnimator;

        [Header("Audio")]
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private PlayerAudioConfig _audioConfig;
        /* ───────────────────────────────────────── */


        public override void InstallBindings()
        {
            BindEventBus();
            BindAnimation();
            BindCoreComponents();
            BindMovement();
            BindAttack();
            BindInteraction();
            BindAudio();
        }

        /* ─────────────  Bind methods  ───────────── */

        private void BindEventBus()
        {
            Container.BindInterfacesAndSelfTo<PlayerEventBus>().AsSingle();
        }

        private void BindCoreComponents()
        {
            Container.Bind<PlayerMovementModel>().FromInstance(_movementModel).AsSingle();
            Container.Bind<Rigidbody2D>().FromComponentOn(gameObject).AsSingle();
            Container.Bind<Transform>().FromComponentOn(gameObject).AsSingle();
        }

        private void BindMovement()
        {
            Container.BindInterfacesAndSelfTo<PlayerMovementController>()
                     .AsSingle()
                     .NonLazy();
        }

        private void BindAttack()
        {
            Container.Bind<PlayerAttackStats>().FromInstance(_attackStats).AsSingle();

            Container.BindInterfacesAndSelfTo<PlayerAttackController>()
                     .AsSingle()
                     .WithArguments(_enemyLayer)   // _attackStats приходит через контейнер
                     .NonLazy();
        }

        private void BindInteraction()
        {
            Container.BindInterfacesAndSelfTo<PlayerInteraction>()
                     .AsSingle()
                     .WithArguments(_interactionRadius, _interactableLayer)
                     .NonLazy();
        }

        private void BindAnimation()
        {
            Container.BindInterfacesAndSelfTo<PlayerAnimationController>()
                     .AsSingle()
                     .WithArguments(_playerAnimator);   
        }

        private void BindAudio()
        {
            Container.Bind<PlayerAudioConfig>().FromInstance(_audioConfig).AsSingle();

            Container.BindInterfacesAndSelfTo<PlayerAudioController>()
                     .AsSingle()
                     .WithArguments(_audioSource);
        }

      
    }
}

using UnityEngine;
using PlayerMovementLogic;
using PlayerAttackLogic;
using UniRx;
using System;
using Zenject;

namespace PlayerAnimationLogic
{
    public class PlayerAnimationController : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private Animator _animator;

        private PlayerMovementModel _movementModel;
        private IDisposable _attackSub;
        private IDisposable _jumpSub;

        private static readonly int Run = Animator.StringToHash("Run");
        private static readonly int Jump = Animator.StringToHash("Jump");
        private static readonly int Fall = Animator.StringToHash("Fall");
        private static readonly int WallSlide = Animator.StringToHash("WallSlide");
        private static readonly int Attack = Animator.StringToHash("Attack");

        [Inject]
        public void Construct(PlayerMovementModel movementModel, PlayerAttack playerAttack, IPlayerEventNotifier eventNotifier)
        {

            _movementModel = movementModel;

            if (_animator == null)
                Debug.LogError("Animator not assigned!");

            if (_movementModel == null)
                Debug.LogError("MovementModel is null!");

            _attackSub = eventNotifier.OnAttack.Subscribe(_ =>
            {
                _animator.SetTrigger(Attack);
            });

            _jumpSub = eventNotifier.OnJump.Subscribe(_ =>
            {
            });
        }

        private void Update()
        {
            UpdateAnimationStates();
        }

        private void UpdateAnimationStates()
        {
            if (_movementModel == null || _animator == null)
                return;

            bool running = Mathf.Abs(_movementModel.HorizontalVelocity) > 0.1f && _movementModel.IsGrounded;
            bool jumping = _movementModel.IsJumping;
            bool falling = _movementModel.IsFalling && !_movementModel.IsGrounded;
            bool wallSliding = _movementModel.IsWallSliding;

            _animator.SetBool(Run, running);
            _animator.SetBool(Jump, jumping);
            _animator.SetBool(Fall, falling);
            _animator.SetBool(WallSlide, wallSliding);
        }

        private void OnDestroy()
        {
            _attackSub?.Dispose();
            _jumpSub?.Dispose();
        }
    }
}

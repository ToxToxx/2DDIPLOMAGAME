using UnityEngine;
using PlayerMovementLogic;
using PlayerAttackLogic;

namespace PlayerAnimationLogic
{
    public class PlayerAnimationController : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private Animator _animator;

        private PlayerMovementModel _movementModel;
        private PlayerAttack _playerAttack;

        // Точные названия из PNG
        private static readonly int Run = Animator.StringToHash("Run");
        private static readonly int Jump = Animator.StringToHash("Jump");
        private static readonly int Fall = Animator.StringToHash("Fall");
        private static readonly int WallSlide = Animator.StringToHash("WallSlide");
        private static readonly int Attack = Animator.StringToHash("Attack");

        public void Initialize(PlayerMovementModel movementModel, PlayerAttack playerAttack, IPlayerEventNotifier eventNotifier)
        {
            _movementModel = movementModel;
            _playerAttack = playerAttack;

            eventNotifier.OnAttack += () => _animator.SetTrigger(Attack);
            eventNotifier.OnJump += () => _animator.SetBool(Jump, true);
        }

        private void Update()
        {
            UpdateAnimationStates();
        }

        private void UpdateAnimationStates()
        {
            // движение по земле
            bool running = Mathf.Abs(_movementModel.HorizontalVelocity) > 0.1f && _movementModel.IsGrounded;
            _animator.SetBool(Run, running);

            // прыжок/падение
            _animator.SetBool(Jump, _movementModel.IsJumping);
            _animator.SetBool(Fall, _movementModel.IsFalling && !_movementModel.IsGrounded);

            // wall slide
            _animator.SetBool(WallSlide, _movementModel.IsWallSliding);
        }
    }
}

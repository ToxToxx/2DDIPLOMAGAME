using Zenject;
using UnityEngine;
using PlayerEvent;
using PlayerMovement;
using InGameInput;

namespace PlayerAttack
{
    public class PlayerAttackController : ITickable
    {
        private readonly IInputService _input;
        private readonly PlayerEventBus _eventBus;
        private readonly Transform _playerTransform;
        private readonly PlayerMovementModel _movementModel;
        private readonly PlayerAttackStats _stats;
        private readonly LayerMask _targetLayer;

        private float _attackTimer;
        public bool IsAttacking { get; private set; }

        public PlayerAttackController(PlayerEventBus eventBus,
                            Transform playerTransform,
                            PlayerMovementModel movementModel,
                            PlayerAttackStats stats,
                            LayerMask targetLayer,
                            IInputService input)
        {
            _eventBus = eventBus;
            _playerTransform = playerTransform;
            _movementModel = movementModel;
            _stats = stats;
            _targetLayer = targetLayer;
            _input = input;
        }

        public void Tick()
        {
            if (_movementModel.IsGrounded && _input.AttackWasPressed && !IsAttacking)
            {
                IsAttacking = true;
                _attackTimer = _stats.AttackDuration;

                _eventBus.RaiseAttack();
                PerformAttack();
            }

            if (IsAttacking)
            {
                _attackTimer -= Time.deltaTime;
                if (_attackTimer <= 0f) IsAttacking = false;
            }
        }

        private void PerformAttack()
        {
            Vector2 dir = _playerTransform.right.normalized;
            Vector2 origin = (Vector2)_playerTransform.position +
                             (Vector2)(_stats.BoxOffset.x * dir + Vector2.up * _stats.BoxOffset.y);

            foreach (var hit in Physics2D.OverlapBoxAll(origin, _stats.BoxSize, 0f, _targetLayer))
            {
                if (hit.TryGetComponent<IDamageable>(out var dmg))
                    dmg.TakeDamage(_stats.Damage);
            }
        }

#if UNITY_EDITOR
        public void DrawGizmos()
        {
            Vector2 dir = _playerTransform.right.normalized;
            Vector2 origin = (Vector2)_playerTransform.position +
                             (Vector2)(_stats.BoxOffset.x * dir + Vector2.up * _stats.BoxOffset.y);
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(origin, _stats.BoxSize);
        }
#endif
    }
}

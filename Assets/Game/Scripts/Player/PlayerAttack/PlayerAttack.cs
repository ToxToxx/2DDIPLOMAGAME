using Zenject;
using UnityEngine;
using PlayerEvent;
using PlayerMovementLogic;

namespace PlayerAttackLogic
{
    public class PlayerAttack : ITickable
    {
        private readonly PlayerEventBus _eventBus;
        private readonly Transform _playerTransform;
        private readonly PlayerMovementModel _movementModel;

        public bool IsAttacking { get; private set; }

        private float _attackTimer = 0f;
        private readonly float _attackDuration = 0.4f;

        // Параметры атаки
        private readonly Vector2 _boxSize = new Vector2(1.2f, 0.8f);
        private readonly Vector2 _boxOffset = new Vector2(1.0f, 0f);
        private readonly float _damage = 1f;
        private readonly LayerMask _targetLayer;

        public PlayerAttack(PlayerEventBus eventBus,
                            Transform playerTransform,
                            PlayerMovementModel movementModel,
                            LayerMask targetLayer)
        {
            _eventBus = eventBus;
            _playerTransform = playerTransform;
            _movementModel = movementModel;
            _targetLayer = targetLayer;
        }

        public void Tick()
        {
            // Можно атаковать ТОЛЬКО когда игрок стоит на земле
            if (_movementModel.IsGrounded && InputManager.AttackWasPressed && !IsAttacking)
            {
                IsAttacking = true;
                _attackTimer = _attackDuration;

                Debug.Log("Атака выполнена!");
                _eventBus.RaiseAttack();

                PerformAttack();
            }

            if (IsAttacking)
            {
                _attackTimer -= Time.deltaTime;
                if (_attackTimer <= 0f)
                {
                    IsAttacking = false;
                    Debug.Log("Атака завершена");
                }
            }
        }

        private void PerformAttack()
        {
            Vector2 rightDir = _playerTransform.right.normalized;
            Vector2 origin = (Vector2)_playerTransform.position +
                               (Vector2)(_boxOffset.x * rightDir + Vector2.up * _boxOffset.y);

            Collider2D[] hits = Physics2D.OverlapBoxAll(origin, _boxSize, 0f, _targetLayer);
            foreach (var hit in hits)
            {
                if (hit.TryGetComponent<IDamageable>(out var target))
                {
                    Debug.Log($"[Attack] Hit {hit.name}, dealing {_damage} damage.");
                    target.TakeDamage(_damage);
                }
            }
        }

#if UNITY_EDITOR
        public void DrawGizmos()
        {
            Vector2 rightDir = _playerTransform.right.normalized;
            Vector2 origin = (Vector2)_playerTransform.position +
                               (Vector2)(_boxOffset.x * rightDir + Vector2.up * _boxOffset.y);
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(origin, _boxSize);
        }
#endif
    }
}

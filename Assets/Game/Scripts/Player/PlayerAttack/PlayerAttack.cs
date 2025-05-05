using Zenject;
using UnityEngine;
using PlayerEvent;

namespace PlayerAttackLogic
{
    public class PlayerAttack : ITickable
    {
        private readonly PlayerEventBus _eventBus;
        private readonly Transform _playerTransform;

        public bool IsAttacking { get; private set; }

        private float _attackTimer = 0f;
        private readonly float _attackDuration = 0.4f;

        // Параметры атаки
        private readonly Vector2 _boxSize = new Vector2(1.2f, 0.8f);
        private readonly Vector2 _boxOffset = new Vector2(1.0f, 0f);
        private readonly float _damage = 1f;
        private readonly LayerMask _targetLayer;

        public PlayerAttack(PlayerEventBus eventBus, Transform playerTransform, LayerMask targetLayer)
        {
            _eventBus = eventBus;
            _playerTransform = playerTransform;
            _targetLayer = targetLayer;
        }

        public void Tick()
        {
            if (InputManager.AttackWasPressed && !IsAttacking)
            {
                IsAttacking = true;
                _attackTimer = _attackDuration;

                Debug.Log("Атака выполнена!");
                _eventBus.RaiseAttack();

                PerformAttack(); // вызов попадания по врагу
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
            int direction = _playerTransform.lossyScale.x > 0 ? 1 : -1;
            Vector2 origin = (Vector2)_playerTransform.position + new Vector2(_boxOffset.x * direction, _boxOffset.y);

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
        // Для отрисовки в редакторе
        public void DrawGizmos()
        {
            int direction = _playerTransform.lossyScale.x > 0 ? 1 : -1;
            Vector2 origin = (Vector2)_playerTransform.position + new Vector2(_boxOffset.x * direction, _boxOffset.y);
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(origin, _boxSize);
        }
#endif
    }
}

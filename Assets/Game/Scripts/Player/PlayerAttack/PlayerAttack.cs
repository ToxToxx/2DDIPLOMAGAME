using Zenject;
using UnityEngine;

namespace PlayerAttackLogic
{
    public class PlayerAttack : ITickable
    {
        private readonly PlayerEventBus _eventBus;

        public bool IsAttacking { get; private set; }

        private float _attackTimer = 0f;
        private readonly float _attackDuration = 0.4f; // <-- длительность атаки в секундах

        public PlayerAttack(PlayerEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public void Tick()
        {
            if (InputManager.AttackWasPressed && !IsAttacking)
            {
                IsAttacking = true;
                _attackTimer = _attackDuration;

                Debug.Log("Атака выполнена!");
                _eventBus.RaiseAttack();
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
    }
}

using Zenject;
using UnityEngine;

namespace PlayerAttackLogic
{
    public class PlayerAttack : ITickable
    {
        private readonly PlayerEventBus _eventBus;

        public bool IsAttacking { get; private set; }

        public PlayerAttack(PlayerEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public void Tick()
        {
            if (InputManager.AttackWasPressed)
            {
                Debug.Log("Атака выполнена!");
                IsAttacking = true;
                _eventBus.RaiseAttack();
            }
            else
            {
                IsAttacking = false;
            }
        }
    }
}

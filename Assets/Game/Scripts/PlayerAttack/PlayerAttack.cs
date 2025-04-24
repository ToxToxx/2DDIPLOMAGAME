using Zenject;

namespace PlayerAttackLogic
{
    public class PlayerAttack : ITickable
    {
        public void Tick()
        {
            if (InputManager.AttackWasPressed)
            {
                UnityEngine.Debug.Log("Атака выполнена!");
            }
        }
    }
}
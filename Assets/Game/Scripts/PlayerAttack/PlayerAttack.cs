using UnityEngine;

namespace PlayerAttackLogic
{
    public class PlayerAttack : MonoBehaviour
    {
        private void Update()
        {
            if (InputManager.AttackWasPressed)
            {
                Debug.Log("Атака выполнена!");
            }
        }
    }
}


using UnityEngine;

namespace PlayerAttack
{
    [CreateAssetMenu(menuName = "Game/Player/Attack Stats", fileName = "PlayerAttackStats")]
    public class PlayerAttackStats : ScriptableObject
    {
        [Header("Hit‑box")]
        public Vector2 BoxSize = new(1.2f, 0.8f);
        public Vector2 BoxOffset = new(1.0f, 0f);

        [Header("Gameplay")]
        public float Damage = 1f;
        public float AttackDuration = 0.4f;
    }

}

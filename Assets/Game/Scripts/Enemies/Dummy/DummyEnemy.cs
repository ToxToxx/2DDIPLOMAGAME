using UnityEngine;

namespace EnemyDummyLogic
{
    public class DummyEnemy : MonoBehaviour, IDamageable
    {
        [Header("Enemy Stats")]
        public float health = 3f;

        [Header("References")]
        [SerializeField] private DummyVisual _visual;

        public void TakeDamage(float damage)
        {
            health -= damage;
            Debug.Log($"{name} получил урон: {damage}. Осталось HP: {health}");

            _visual?.AnimateHit();

            if (health <= 0)
            {
                Destroy(gameObject);
                Debug.Log($"{name} уничтожен.");
            }
        }
    }
}

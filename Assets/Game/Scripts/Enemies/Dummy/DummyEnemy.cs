using UnityEngine;

namespace EnemyDummyLogic
{
    public class DummyEnemy : MonoBehaviour, IDamageable
    {
        [Header("Enemy Stats")]
        public float _health = 3f;

        [Header("References")]
        [SerializeField] private DummyVisual _visual;

        public void TakeDamage(float damage)
        {
            _health -= damage;
            Debug.Log($"{name} получил урон: {damage}. Осталось HP: {_health}");

            _visual?.AnimateHit();

            if (_health <= 0)
            {
                Destroy(gameObject);
                Debug.Log($"{name} уничтожен.");
            }
        }
    }
}

using UnityEngine;

public class DummyEnemy : MonoBehaviour, IDamageable
{
    public float health = 3f;

    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log($"{name} получил урон: {damage}. Осталось HP: {health}");

        if (health <= 0)
        {
            Destroy(gameObject);
            Debug.Log($"{name} уничтожен.");
        }
    }
}

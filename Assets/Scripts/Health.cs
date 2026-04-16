using UnityEngine;

public class Health : MonoBehaviour
{
    public int health = 100;
    public int maxHealth = 100;
    private IDamageable damageable;

    public void Awake()
    {
        damageable = GetComponent<IDamageable>();
        maxHealth = health;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            damageable.OnDead();
        }
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
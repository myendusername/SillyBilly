using UnityEngine;

public class Health : MonoBehaviour
{
    public int health = 20;
    private IDamageable damageable;

    public void Awake()
    {
        damageable = GetComponent<IDamageable>();
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


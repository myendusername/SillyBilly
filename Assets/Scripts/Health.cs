using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] public int health;
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

    public void SetHealth() {
        health = 100;
    }

    public void SetDead()
    {
        health = 0;
    }

    public Transform GetTransform()
    {
        return transform;
    }
}


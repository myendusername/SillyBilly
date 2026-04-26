using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] public int health;
    private bool invincible = false;
    private IDamageable damageable;

    public void Awake()
    {
        damageable = GetComponent<IDamageable>();
    }

    public void TakeDamage(int damage)
    {
        if (invincible == false)
        {
            health -= damage;
            if (health <= 0)
            {
                damageable.OnDead();
            }
            else
            {
                damageable.OnHurt();
            }
        }
    }

    public void SetHealth()
    {
        health = 100;
    }

    public void SetDead()
    {
        health = 0;
    }

    public void SetInvincible(bool value)
    {
        invincible = value;
    }

    public Transform GetTransform()
    {
        return transform;
    }
}


using UnityEngine;

public class Health : MonoBehaviour
{
    public int health;
    private int maxHealth;
    private bool invincible = false;
    private IDamageable damageable;

    public void Awake()
    {
        damageable = GetComponent<IDamageable>();
        maxHealth = health;
    }

    public void TakeDamage(int damage)
    {
        if (invincible == false)
        {
            health -= damage;
            health = Mathf.Clamp(health, 0, maxHealth);
            if (health <= 0)
            {
                damageable.OnDead();
            }
            else if (damage < 0)
            {
                damageable.OnHeal();
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


using UnityEngine;

public class Health : MonoBehaviour
{
    public int health;
    private int maxHealth;
    public bool invincible = false;
    private IDamageable damageable;
    private bool isDead = false;

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
            if (health <= 0 && !isDead)
            {
                isDead = true;
                damageable.OnDead();
            }
            else if (damage < 0)
            {
                damageable.OnHeal();
            }
            else if (!isDead)
            {
                damageable.OnHurt();
            }
        }
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


using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;

        if (UIManager.instance != null)
            UIManager.instance.SetHealth(currentHealth, maxHealth);
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        // dont go below 0
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (UIManager.instance != null)
            UIManager.instance.SetHealth(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;

        // dont go above max
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (UIManager.instance != null)
            UIManager.instance.SetHealth(currentHealth, maxHealth);
    }

    void Die()
    {
        // placeholder for now
        Debug.Log("Player Died");
    }
}

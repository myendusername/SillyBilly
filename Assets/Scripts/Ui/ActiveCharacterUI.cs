using UnityEngine;
using UnityEngine.UI;

public class ActiveCharacterUI : MonoBehaviour
{
    public Slider healthBar;
    public Slider staminaBar;

    private Health activeHealth;
    private PlayerMovement activeMovement;

    public static ActiveCharacterUI instance;

    void Awake()
    {
        instance = this;
    }

    public void SetActiveCharacter(Health health, PlayerMovement movement)
    {
        activeHealth = health;
        activeMovement = movement;
        UpdateBars();
    }

    void Update()
    {
        UpdateBars();
    }

    void UpdateBars()
    {
        if (activeHealth != null && healthBar != null)
        {
            healthBar.maxValue = activeHealth.maxHealth;
            healthBar.value = activeHealth.health;
        }

        if (activeMovement != null && staminaBar != null)
        {
            staminaBar.maxValue = activeMovement.maxStamina;
            staminaBar.value = activeMovement.stamina;
        }
    }
}

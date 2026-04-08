using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    // drag your Slider objects here in Inspector
    public Slider healthBar;
    public Slider staminaBar;

    public TextMeshProUGUI weaponText;
    public TextMeshProUGUI ammoText;

    public static UIManager instance;

    void Awake()
    {
        instance = this;
    }

    public void SetHealth(float current, float max)
    {
        if (healthBar != null)
        {
            healthBar.maxValue = max;
            healthBar.value = current;
        }
    }

    public void SetStamina(float current, float max)
    {
        if (staminaBar != null)
        {
            staminaBar.maxValue = max;
            staminaBar.value = current;
        }
    }

    public void SetWeapon(string name)
    {
        if (weaponText != null)
            weaponText.text = name;
    }

    public void SetAmmo(string text)
    {
        if (ammoText != null)
            ammoText.text = text;
    }
}


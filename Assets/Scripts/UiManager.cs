using TMPro;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    // We drag and drop the canvas's text objects into these fields
    // to connect them with this script
    [SerializeField] private TextMeshProUGUI healthText, staminaText;

    public GameObject player;
    private PlayerHealth playerHealth;
    private PlayerMovement playerMovement;

    private void Awake()
    {
        playerHealth = player.GetComponent<PlayerHealth>();
        playerMovement = player.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        trackPlayer();
    }

    public void trackPlayer()
    {
        // these set the health and stamina stats in the UI
        // to the player's current health and stamina stats.
        // Casting the numbers as ints so that we don't see
        // a bunch of unneeded decimal places for our stats
        healthText.text = "Health: " + playerHealth.health;
        staminaText.text = "Stamina: " + (int)playerMovement.stamina;
    }
}

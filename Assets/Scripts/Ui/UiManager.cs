using TMPro;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    public static UiManager Instance;

    // We drag and drop the canvas's text objects into these fields
    // to connect them with this script
    [SerializeField] private TextMeshProUGUI healthText, staminaText;

    public GameObject player;
    private int playerHealth;
    private PlayerMovement playerMovement;

    private void Awake()
    {
        Instance = this;
        playerMovement = player.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    // Placing the player health assignment in update allows
    // the health to change as the Health and IDamageable scripts
    // do their work(?)
    void Update()
    {
        playerHealth = player.GetComponent<Health>().health;
        TrackPlayer();
    }

    public void TrackPlayer()
    {
        // these set the health and stamina stats in the UI
        // to the player's current health and stamina stats.
        // Casting the numbers as ints so that we don't see
        // a bunch of unneeded decimal places for our stats
        healthText.text = "Health: " + playerHealth;
        staminaText.text = "Stamina: " + (int)playerMovement.stamina;
    }

    public void RespawnPlayer() {
        player.GetComponent<Health>().SetHealth();
    }
}

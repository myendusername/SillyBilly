using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public static UiManager Instance;

    // We drag and drop the canvas's slider objects into these fields
    // to connect them with this script
    [SerializeField] private Slider healthBar, staminaBar;

    public GameObject player;
    private Health playerHealth;
    private PlayerMovement playerMovement;

    private void Awake()
    {
        Instance = this;
        playerHealth = player.GetComponent<Health>();
        playerMovement = player.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    // Placing the player health assignment in update allows
    // the health to change as the Health and IDamageable scripts
    // do their work(?)
    void Update()
    {
        TrackPlayer();
    }

    public void TrackPlayer()
    {
        // these set the health and stamina stats in the UI
        // to the player's current health and stamina stats.
        if (healthBar != null && playerHealth != null)
        {
            healthBar.maxValue = 100;
            healthBar.value = playerHealth.health;
        }

        if (staminaBar != null && playerMovement != null)
        {
            staminaBar.maxValue = 50;
            staminaBar.value = playerMovement.stamina;
        }
    }

    // Resets the player's health back to its full value
    public void RespawnPlayer()
    {
        player.GetComponent<Health>().SetHealth();
    }

    // Changes the game mode to the title screen
    // And makes sure weird crap doesn't happen like the player's
    // HP dropping into the negatives
    public void GameOver()
    {
        // Debug.Log("Game over!");
        GameManager.Instance.ChangeState(GameState.TitleScreen);
        player.GetComponent<Health>().SetDead();
        GameManager.Instance.DestroyEnemies();
    }

    // updates which character the bars are tracking
    public void SetPlayer(GameObject newPlayer)
    {
        player = newPlayer;
        playerHealth = newPlayer.GetComponent<Health>();
        playerMovement = newPlayer.GetComponent<PlayerMovement>();
    }
}
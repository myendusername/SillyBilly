using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public static UiManager Instance;

    public GameObject mainMenu;
    public GameObject player;
    [SerializeField] private Slider healthBar, staminaBar;
    public GameObject hurtFlash;

    private Health playerHealth;
    private PlayerMovement playerMovement;

    // Giving the UiManager awareness of the buttons
    // for selecting each character
    public Button seriousBobert;
    public Button bobdi;
    public Button bbq;
    private Button selectedButton;

    private void Awake()
    {
        Instance = this;
        SetPlayer(player);
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
            healthBar.value = playerHealth.health;
        }

        if (staminaBar != null && playerMovement != null)
        {
            staminaBar.value = playerMovement.stamina;
        }

        if (playerHealth.health <= 0)
        {
            Debug.Log("GAME OVER!!!!!");
            GameOver();
        }
    }

    // Resets the player's health back to its full value
    public void RespawnPlayer()
    {
        player.GetComponent<Health>().SetHealth();
    }

    // These three buttons are called by each character's button's
    // On Click() method to control which character is the
    // selected character.
    public void SelectSeriousBobert() {
        selectedButton = seriousBobert;
        Debug.Log("You've selected Serious Bobert.");
    }
    public void SelectBobdi()
    {
        selectedButton = bobdi;
        Debug.Log("You've selected Bobdi.");
    }
    public void SelectBBQ()
    {
        selectedButton = bbq;
        Debug.Log("You've selected BBQ.");
    }
    // Gets which button is currently selected
    public Button getSelected() { 
        return selectedButton;
    }

    // Changes the game mode to the title screen
    // And makes sure weird crap doesn't happen like the player's
    // HP dropping into the negatives
    public void GameOver()
    {
        // Debug.Log("Game over!");
        player.GetComponent<Health>().SetDead();
        GameManager.Instance.DestroyEnemies();
        GameManager.Instance.ChangeState(GameState.TitleScreen);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // updates which character the bars are tracking
    public void SetPlayer(GameObject newPlayer)
    {
        player = newPlayer;
        playerHealth = newPlayer.GetComponent<Health>();
        playerMovement = newPlayer.GetComponent<PlayerMovement>();
    }

    public void SetPlayerUi(bool status)
    {
        healthBar.gameObject.SetActive(status);
        staminaBar.gameObject.SetActive(status);
    }

    public void SetMainMenu(bool status)
    {
        mainMenu.SetActive(status);
    }

    public void HurtFlash()
    {
        Animator hurtAnimator = hurtFlash.GetComponent<Animator>();
        hurtAnimator.Play("Restart");
    }
}
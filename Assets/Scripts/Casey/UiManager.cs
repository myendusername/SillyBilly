using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public static UiManager Instance;

    public GameObject mainMenu;
    public GameObject player;
    [SerializeField] private Slider healthBar, staminaBar;
    //new
    public Image secondaryCooldownImage;
    private PlayerSecondaryShoot activeSecondaryShooter;
    [SerializeField] private GameObject playerWeapon;
    // [SerializeField] private TextMeshProUGUI healthText, staminaText;
    public GameObject hurtFlash;

    // The UI's text fields
    public const string characterBioBegin = "Bio: ";
    public TextMeshProUGUI characterBio;
    public TextMeshProUGUI characterSelectionPrompt;

    private Health playerHealth;
    private PlayerMovement playerMovement;

    private string selectedCharacter = "";

    private void Awake()
    {
        Instance = this;
        SetPlayer(player);
        characterBio.text = characterBioBegin;
    }

    // Update is called once per frame
    // Placing the player health assignment in update allows
    // the health to change as the Health and IDamageable scripts
    // do their work(?)
    void Update()
    {
        TrackPlayer();
        //new
        if (secondaryCooldownImage != null && activeSecondaryShooter != null)
        {
            secondaryCooldownImage.fillAmount = activeSecondaryShooter.GetCooldownProgress();
        }
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
    // selected character. With each selection, the character bio
    // changes to describe the selected character.
    public void SelectSeriousBobert()
    {
        selectedCharacter = "Serious Bobert";
        characterBio.text = characterBioBegin;
        characterBio.text += "This guy can rapid fire bullets at enemies to deal some SERIOUS dps! He also has a very SMELLY secret power...";
        Debug.Log("You've selected Serious Bobert.");
    }
    public void SelectBobdi()
    {
        selectedCharacter = "Bobdi";
        characterBio.text = characterBioBegin;
        characterBio.text += "Bobdi's shotgun is devastating up close. Ride his oil slicks to rushdown enemies and deal huge bursts of damage!";
        Debug.Log("You've selected Bobdi.");
    }
    public void SelectBBQ()
    {
        selectedCharacter = "BBQ";
        characterBio.text = characterBioBegin;
        characterBio.text += "He's a cunning fire elemental who can also cast walls of light. His flames may react violently with other substances...";
        Debug.Log("You've selected BBQ.");
    }
    // Gets which button is currently selected
    public string getSelected()
    {
        return selectedCharacter;
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
        //new
        activeSecondaryShooter = newPlayer.GetComponent<PlayerSecondaryShoot>();
    }

    public void SetPlayerUi(bool status)
    {
        healthBar.gameObject.SetActive(status);
        staminaBar.gameObject.SetActive(status);
        if (playerWeapon)
        {
            playerWeapon.SetActive(status);
        }
        //new
        if (secondaryCooldownImage != null)
            secondaryCooldownImage.gameObject.SetActive(status);
        //healthText.gameObject.SetActive(status);
        //staminaText.gameObject.SetActive(status);
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
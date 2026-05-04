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

    public Image secondaryCooldownImage;
    private PlayerSecondaryShoot activeSecondaryShooter;
    // [SerializeField] private TextMeshProUGUI healthText, staminaText;
    public GameObject flash;

    // The UI's text fields
    public const string characterBioBegin = "Bio: ";
    public TextMeshProUGUI characterBio;
    public TextMeshProUGUI characterSelectionPrompt;

    private Health playerHealth;
    private PlayerMovement playerMovement;

    private string selectedCharacter = "";

    public TextMeshProUGUI waveText;

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

    // These three buttons are called by each character's button's
    // On Click() method to control which character is the
    // selected character. With each selection, the character bio
    // changes to describe the selected character.
    public void SelectSeriousBobert()
    {
        selectedCharacter = "Serious Bobert";
        characterBio.text = characterBioBegin;
        characterBio.text += "This guy can rapid fire bullets at enemies to deal some SERIOUS damage!  He also has a very SMELLY ability, enemies are hurt by the stench!  Shoot his gas cloud with Bobdi to explode it, or shoot it with BBQ to set it ablaze!";
        Debug.Log("You've selected Serious Bobert.");
    }
    public void SelectBobdi()
    {
        selectedCharacter = "Bobdi";
        characterBio.text = characterBioBegin;
        characterBio.text += "Bobdi's shotgun is devastating up close.  Ride his oil slicks to rushdown enemies!  Shoot it with Bobert to make a stunning area blast, or shoot it with BBQ to set it on fire, making the oil even faster along with hurting enemies!";
        Debug.Log("You've selected Bobdi.");
    }
    public void SelectBBQ()
    {
        selectedCharacter = "BBQ";
        characterBio.text = characterBioBegin;
        characterBio.text += "He's a cunning fire elemental who can also cast enhancing walls of light.  Shoot through his wall with Bobert to TRIPLE his damage (stacking of course), or shoot it with Bobdi to make his stones explode upon impact!";
        Debug.Log("You've selected BBQ.");
    }

    // Gets which button is currently selected
    public string GetSelected()
    {
        return selectedCharacter;
    }

    // Changes the game mode to the title screen
    // And makes sure weird crap doesn't happen like the player's
    // HP dropping into the negatives
    public void GameOver()
    {
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

        activeSecondaryShooter = newPlayer.GetComponent<PlayerSecondaryShoot>();
    }

    public void SetPlayerUi(bool status)
    {
        healthBar.gameObject.SetActive(status);
        staminaBar.gameObject.SetActive(status);
        waveText.gameObject.SetActive(status);

        if (secondaryCooldownImage != null)
        {
            secondaryCooldownImage.gameObject.SetActive(status);
        }
    }

    public void SetMainMenu(bool status)
    {
        mainMenu.SetActive(status);
    }

    public void HurtFlash()
    {
        Animator animator = flash.GetComponent<Animator>();
        animator.Play("Restart Hurt");
    }

    public void HealFlash()
    {
        Animator animator = flash.GetComponent<Animator>();
        animator.Play("Restart Heal");
    }

    public void WaveOverAnimation()
    {
        waveText.gameObject.GetComponent<Animator>().Play("Wave Over");
    }

    public void SetWaveText(int wave)
    {
        waveText.SetText("Wave " + wave);
        waveText.gameObject.GetComponent<Animator>().Play("Wave");
    }
}
using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState GameState;

    public InputManager inputManager;
    public CameraManager cameraMan;
    public UiManager uiManager;
    [SerializeField] private EnemiesList enemies;
    //public int currentEnemiesNumber;
    //private const int maxEnemiesNumber = 30;
    // private IEnumerator spawnDelay;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        ChangeState(GameState.TitleScreen);
    }

    public void ChangeState(GameState newState)
    {
        GameState = newState;
        switch (newState)
        {
            // the cursor shouldn't be locked in this state
            // The title screen should be active in this state.
            case GameState.TitleScreen:
                Cursor.lockState = CursorLockMode.None;
                inputManager.enabled = false;
                cameraMan.enabled = false;
                uiManager.SetMainMenu(true);
                uiManager.SetPlayerUi(false);
                //currentEnemiesNumber = 0;
                //Debug.Log("We're at the title screen. There are currently " + currentEnemiesNumber + " enemies in the game.");
                break;

            // lock the cursor when we activate this state.
            // spawn the enemies in.
            case GameState.GamePlay:
                SpawnPlayer();
                Cursor.lockState = CursorLockMode.Locked;
                inputManager.enabled = true;
                cameraMan.enabled = true;
                uiManager.SetMainMenu(false);
                uiManager.SetPlayerUi(true);
                // Apparently all IEnumerators have to be called with
                // StartCoroutine(), not just normally.
                StartCoroutine(SpawnEnemies());
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
    }

    // Setting the character the player starts as in the game
    // according to the character that the player has selected
    // on the title screen.
    public void SpawnPlayer() {
        // gets data for the character the player selects
        string firstCharacter = UiManager.Instance.getSelected();
        Debug.Log("Selected character: " + firstCharacter);

        // uses that data to decide who to spawn in as the player character
        switch (firstCharacter)
        {
            case "Serious Bobert":
                InputManager.Instance.SetActiveCharacter(InputManager.Instance.characters[0]);
                UiManager.Instance.SetPlayer(InputManager.Instance.characters[0]);
                Debug.Log("You're Serious Bobert!");
                break;
            case "Bobdi":
                InputManager.Instance.SetActiveCharacter(InputManager.Instance.characters[1]);
                UiManager.Instance.SetPlayer(InputManager.Instance.characters[1]);
                Debug.Log("You're Bobdi!");
                break;
            case "BBQ":
                InputManager.Instance.SetActiveCharacter(InputManager.Instance.characters[2]);
                UiManager.Instance.SetPlayer(InputManager.Instance.characters[2]);
                Debug.Log("You're BBQ!");
                break;

            // handles errors or something idk
            default:
                ChangeState(GameState.TitleScreen);
                Debug.Log("Error in spawning the player in...");
                break;
        }
    }

    // Change the gamemode to GamePlay
    public void StartGame()
    {
        string firstCharacter = UiManager.Instance.getSelected();
        if (firstCharacter == "Serious Bobert" || firstCharacter == "Bobdi" || firstCharacter == "BBQ")
        {
            ChangeState(GameState.GamePlay);
            Debug.Log("The game has BEGUN!!!");
        }
        else {
            // handles cases in which the player is a devious mf
            // and chooses not to select a character before pressing play.
            // Tells them (and us in the console) that they need to select a character
            UiManager.Instance.characterSelectionPrompt.text = "Please select a character before playing.";
            Debug.Log("Please select a character before playing.");
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    // Spawn each of the enemies in the enemies list.
    // This method spawns in enemies in waves. Currently,
    // enemies spawn in every 15 seconds.
    public IEnumerator SpawnEnemies()
    {
        int wave = 1;
        while (GameState == GameState.GamePlay)
        {
            Debug.Log("Spawn wave " + wave);
            for (int i = 0; i < enemies.enemies.Length; i++)
            {
                // randomizing the spawn position a little bit...
                float spawnX = Random.Range(-30, 30);
                float spawnZ = Random.Range(-30, 30);
                Vector3 spawnPosition = new Vector3(spawnX, 0, spawnZ);

                NavMeshHit navHit;

                // Try to get valid point on NavMesh,
                // and also make sure there aren't TOO many enemies in the game.
                if (NavMesh.SamplePosition(spawnPosition, out navHit, 5f, NavMesh.AllAreas))
                {
                    Instantiate(enemies.enemies[i], navHit.position, Quaternion.identity);
                    //currentEnemiesNumber++;
                }
                else
                {
                    Debug.Log("Error spawning " + enemies.enemies[i]);
                }
            }

            // spawns enemies again after a bit of time.
            // (20 seconds)
            wave++;
            //Debug.Log("There are currently " + currentEnemiesNumber + " enemies in the game.");
            yield return new WaitForSeconds(15.0f);
        }
    }

    // Finds the enemy by its GameObject's name in the heirarchy,
    // then destroys it. ACTUALLY WORKS NOW!!! Yippee!!!
    public void DestroyEnemies()
    {
        Debug.Log("I'm trying to kill enemies.");
        for (int i = 0; i < enemies.enemies.Length; i++)
        {
            GameObject toDestroy = GameObject.Find(enemies.enemies[i].name + "(Clone)");
            Destroy(toDestroy);
        }
    }
}

// Serious Bobert's game states
// I want to make a title screen state, where nothing happens until you press the button
// Then a gameplay state, which then allows everything to happen
public enum GameState
{
    TitleScreen = 0,
    GamePlay = 1
}

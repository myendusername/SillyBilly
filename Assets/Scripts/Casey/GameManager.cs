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
    private int currentEnemyAmount;
    public int enemiesInWave = 8;
    public int enemyAmountIncrease = 2;
    private int wave = 1;
    public float spawnDelay = 6;
    private bool waveStarting = false;

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
                SpawnEnemies();
                UiManager.Instance.SetWaveText(wave);
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
    }

    // Setting the character the player starts as in the game
    // according to the character that the player has selected
    // on the title screen.
    public void SpawnPlayer()
    {
        // gets data for the character the player selects
        string firstCharacter = UiManager.Instance.GetSelected();
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
        string firstCharacter = UiManager.Instance.GetSelected();
        if (firstCharacter == "Serious Bobert" || firstCharacter == "Bobdi" || firstCharacter == "BBQ")
        {
            ChangeState(GameState.GamePlay);
            Debug.Log("The game has BEGUN!!!");
        }
        else
        {
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
    public void SpawnEnemies()
    {
        if (GameState == GameState.GamePlay)
        {
            Debug.Log("Spawn wave " + wave);
            for (int i = 0; i < enemiesInWave; i++)
            {
                // randomizing the spawn position a little bit...
                float spawnX = Random.Range(-75, 75);
                float spawnZ = Random.Range(-75, 75);
                Vector3 spawnPosition = new Vector3(spawnX, 0, spawnZ);

                NavMeshHit navHit;

                int enemyToSpawn = Random.Range(0, enemies.enemies.Length);

                // Try to get valid point on NavMesh,
                if (NavMesh.SamplePosition(spawnPosition, out navHit, 6f, NavMesh.AllAreas))
                {
                    Instantiate(enemies.enemies[enemyToSpawn], navHit.position, Quaternion.identity);
                    currentEnemyAmount++;
                }
                else
                {
                    Debug.Log("Error spawning " + enemies.enemies[enemyToSpawn]);
                }
            }
        }
    }

    public IEnumerator WaveOver()
    {
        waveStarting = true;
        Debug.Log("All enemies are dead, starting new wave after delay.");
        UiManager.Instance.WaveOverAnimation();

        WaitForSeconds wait = new WaitForSeconds(spawnDelay);
        yield return wait;

        wave++;
        UiManager.Instance.SetWaveText(wave);
        enemiesInWave += enemyAmountIncrease;
        // Extra precaution
        currentEnemyAmount = 0;
        SpawnEnemies();

        waveStarting = false;
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

    public void ReportEnemyDeath()
    {
        currentEnemyAmount--;

        if (currentEnemyAmount <= 0 && !waveStarting)
            StartCoroutine(WaveOver());
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

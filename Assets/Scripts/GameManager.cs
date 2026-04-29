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
                break;

            // lock the cursor when we activate this state.
            // spawn the enemies in.
            case GameState.GamePlay:
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

    // Change the gamemode to GamePlay
    public void StartGame()
    {
        ChangeState(GameState.GamePlay);
        Debug.Log("The game has BEGUN!!!");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    // Spawn each of the enemies in the enemies list.
    // This method spawns in enemies in waves. Currently,
    // enemies spawn in every 5 seconds.
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

                // Try to get valid point on NavMesh
                if (NavMesh.SamplePosition(spawnPosition, out navHit, 5f, NavMesh.AllAreas))
                {
                    Instantiate(enemies.enemies[i], navHit.position, Quaternion.identity);
                }
                else
                {
                    Debug.Log("Error spawning " + enemies.enemies[i]);
                }
            }

            // spawns enemies again after a bit of time.
            // (5 seconds)
            wave++;
            yield return new WaitForSeconds(5.0f);
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

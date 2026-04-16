using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState GameState;

    public InputManager inputManager;
    public CameraSwitcher cameraSwitcher;
    [SerializeField] private EnemiesList enemies;
    [SerializeField] private GameObject titleScreen;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start game");
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
                titleScreen.SetActive(true);
                inputManager.enabled = false;
                cameraSwitcher.enabled = false;
                // DestroyEnemies();
                break;

            // lock the cursor when we activate this state.
            // spawn the enemies in.
            case GameState.GamePlay:
                Cursor.lockState = CursorLockMode.Locked;
                inputManager.enabled = true;
                cameraSwitcher.enabled = true;
                UiManager.Instance.RespawnPlayer();
                SpawnEnemies();
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
    }

    // Change the gamemode to GamePlay
    public void StartPlaying()
    {
        ChangeState(GameState.GamePlay);
        Debug.Log("The game has BEGUN!!!");
    }

    // Spawn each of the enemies in the enemies list.
    public void SpawnEnemies() {
        for (int i = 0; i < enemies.enemies.Length; i++) {
            // randomizing the spawn position a little bit...
            float spawnX = Random.Range(-30, 30);
            float spawnZ = Random.Range(-30, 30);
            Instantiate(enemies.enemies[i], new Vector3(spawnX, 0, spawnZ), Quaternion.identity);
        }
    }

    // Destroy each of the enemies in the enemies list.
    //public void DestroyEnemies()
    //{
    //    for (int i = 0; i < enemies.enemies.Length; i++)
    //    {
    //        DestroyImmediate(enemies.enemies[i]);
    //    }
    //}
}

// Serious Bobert's game states
// I want to make a title screen state, where nothing happens until you press the button
// Then a gameplay state, which then allows everything to happen
public enum GameState
{
    TitleScreen = 0,
    GamePlay = 1
}

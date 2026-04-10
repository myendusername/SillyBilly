using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState GameState;

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
            case GameState.TitleScreen:
                // doesn't do anything for right now
                break;

            // lock the cursor when we activate this state
            case GameState.GamePlay:
                Cursor.lockState = CursorLockMode.Locked;
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
}

// Serious Bobert's game states
// I want to make a title screen state, where nothing happens until you press the button
// Then a gameplay state, which then allows everything to happen
public enum GameState
{
    TitleScreen = 0,
    GamePlay = 1
}

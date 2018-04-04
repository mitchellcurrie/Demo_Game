using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager GMinstance = null;
    public enum GameState { GAME, GAMEOVER };
    public static GameState CurrentState;

    void Awake()
    {
        // Singleton structure to ensure only 1 game manager instance
        if (GMinstance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            GMinstance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        CurrentState = GameState.GAME;
    }

    void Update()
    {
        // Update based on current state of the game
        if (CurrentState == GameState.GAME)
        {
          
        }

        else if (CurrentState == GameState.GAMEOVER)
        {

        }
    }

    public static void GameOver()
    {
        CurrentState = GameState.GAMEOVER;
        Cursor.visible = true;
    }

    public static void ResetGame()
    {
        CurrentState = GameState.GAME;
    }

    public static void Quit()
    {
        Application.Quit();
    }
}

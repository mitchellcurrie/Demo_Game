using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager GMinstance = null;

    // Game state
    public enum GameState { GAME, GAMEOVER };
    public static GameState CurrentState;

    // Text
    public GameObject gameText;
    public GameObject gameOverText;

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
        Cursor.visible = false;
        gameText.SetActive(true);
        gameOverText.SetActive(false);
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

    public void GameOver()
    {
        CurrentState = GameState.GAMEOVER;
        Cursor.visible = true;
        gameText.SetActive(false);
        gameOverText.SetActive(true);
    }

    public void ResetGame()
    {
        CurrentState = GameState.GAME;
        Cursor.visible = false;
        gameText.SetActive(true);
        gameOverText.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }
}

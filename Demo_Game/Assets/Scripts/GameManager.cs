using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour {

    public static GameManager GMinstance = null;

    // Game state
    public enum GameState { INSTRUCTIONS, GAME, GAMEOVER };
    public static GameState CurrentState;

    // On Screen UI Objects containing the text for each game state
    public GameObject gameUI;
    public GameObject gameOverUI;
    public GameObject instructionsUI;


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
        // Set starting game state and display instruction UI only
        CurrentState = GameState.INSTRUCTIONS;
        gameUI.SetActive(false);
        gameOverUI.SetActive(false);
        instructionsUI.SetActive(true);
    }

    public void GameOver()
    {
        CurrentState = GameState.GAMEOVER;
        gameUI.SetActive(false);
        gameOverUI.SetActive(true);
        instructionsUI.SetActive(false);
    }

    public void ResetGame()
    {
        CurrentState = GameState.GAME;
        gameUI.SetActive(true);
        gameOverUI.SetActive(false);
        instructionsUI.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }
}

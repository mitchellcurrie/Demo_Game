using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Each line of the game is made up of a GameString struct
struct GameString
{
    public string stringText;
    public bool hasWordPlaced;
}

public class HackerGame : MonoBehaviour {

    public GameManager gameManager;

    // Player input
    public TextMeshProUGUI playerInputString;
    private int stringCharLimit;

    // Strings
    public TextMeshProUGUI gameStrings;
    private GameString[] gameStringArray = new GameString[10];
    public string Answer;
    public string[] FakeAnswers;    
    private string randomChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ~!@#$%^&*()-+={}[]|:;<>,.?/";
    private int gameStringLength;

    void Start ()
    {
        stringCharLimit = 10;
        gameStringLength = 30;
        SetRandomStrings();
        DisplayGameStrings();
    }
	
	void Update ()
    {
		if (GameManager.CurrentState == GameManager.GameState.GAME)
        {
            CheckPlayerInputAndDisplay();
        }
	}

    // Check player keyboard input and display to screen by updating on screen text
    void CheckPlayerInputAndDisplay()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
        {
            // Mouse input
        }

        else if (Input.GetKeyDown(KeyCode.Return))
        {
            playerInputString.text = "";
        }

        else if (Input.GetKeyDown(KeyCode.Backspace))
        {
            if (playerInputString.text.Length > 0)
            {
                // Remove last character of string
                playerInputString.text = playerInputString.text.Remove(playerInputString.text.Length - 1, 1);
            }
        }

        else if (Input.anyKeyDown && playerInputString.text.Length < stringCharLimit)
        {
            if (Input.GetKeyDown(KeyCode.Space))           
                playerInputString.text += "_";
            else
                playerInputString.text += Input.inputString;
        }
    }

    void GameSetup()
    {

    }

    // Display game strings to screen by updating the on screen text
    void DisplayGameStrings()
    {
        gameStrings.text = "";

        foreach (GameString gs in gameStringArray)
        {
            gameStrings.text += gs.stringText + "\n";
        }      
    }

    // Assign random characters from "randomChars" string to each of the game strings in the array
    void SetRandomStrings()
    {
        for (int i = 0; i < gameStringArray.Length; i++)
        {
            gameStringArray[i].stringText = "";

            for (int j = 0; j < gameStringLength; j++)
            {
                gameStringArray[i].stringText += randomChars[Random.Range(0, randomChars.Length - 1)];
            }
        }
    }
}


//  TestInt = TestString.IndexOf(Answer);

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Each line of the game is made up of a GameString struct
struct GameString
{
    public string str;
    public bool hasWordPlaced;
}

public class HackerGame : MonoBehaviour {

    public GameManager gameManager;

    // Player input
    public TextMeshProUGUI playerInputString;
    private int stringCharLimit;

    // Strings
    public TextMeshProUGUI gameStrings;
    private GameString[] gsArray = new GameString[10];
    public string Answer;
    [Header("Maximum of 8 Fake Answers")]
    public string[] FakeAnswers;
    //private string randomChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ~!@#$%^&*()-+={}[]|:;<>,.?/";
    private string randomChars = "-";
    private int gameStringLength;

    void Start ()
    {
        stringCharLimit = 10;
        gameStringLength = 20;
        playerInputString.text = "";

        GameSetup();
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
            if (!CheckWord(playerInputString.text))
            {
                Debug.Log("Word not found");
            }

            DisplayGameStrings();
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
        SetRandomStrings();
        AddWordsToGameStrings();
        DisplayGameStrings();
    }

    // Display game strings to screen by updating the on screen text
    void DisplayGameStrings()
    {
        gameStrings.text = "";

        foreach (GameString gs in gsArray)
        {
            gameStrings.text += gs.str + "\n";
        }      
    }

    // Assign random characters from "randomChars" string to each of the game strings in the array
    void SetRandomStrings()
    {
        for (int i = 0; i < gsArray.Length; i++)
        {
            gsArray[i].str = "";
            gsArray[i].hasWordPlaced = false;

            for (int j = 0; j < gameStringLength; j++)
            {
                gsArray[i].str += randomChars[Random.Range(0, randomChars.Length - 1)];
            }
        }
    }

    // Insert the answer and fake answers into the random strings
    void AddWordsToGameStrings()
    {
        // Add answer

        // Find random gamestring in the gsArray to place the answer and store in "i"
        int i = Random.Range(0, gsArray.Length - 1);

        // Reduce the length of the string by the length of the answer being inserted
        gsArray[i].str = gsArray[i].str.Substring(0, gsArray[i].str.Length - Answer.Length);
        // Add answer in a random position
        gsArray[i].str = gsArray[i].str.Insert(Random.Range(0, gsArray[i].str.Length - 1), Answer);
        gsArray[i].hasWordPlaced = true;

        // Add fake answers

        for (int j = 0; j < FakeAnswers.Length; j++)
        {
            // Make sure the game string hasn't had a word placed in it yet
            // If it has, get another random one
            while (gsArray[i].hasWordPlaced)
            {
                i = Random.Range(0, gsArray.Length - 1);

                if (!gsArray[i].hasWordPlaced)               
                   break;               
            }

            // Reduce the length of the string by the length of the fake answer being inserted
            gsArray[i].str = gsArray[i].str.Substring(0, gsArray[i].str.Length - FakeAnswers[j].Length);
            // Add fake answer in a random position
            gsArray[i].str = gsArray[i].str.Insert(Random.Range(0, gsArray[i].str.Length - 1), FakeAnswers[j]);
            gsArray[i].hasWordPlaced = true;
        }
    }

    bool CheckWord(string playerInputString)
    {
        Debug.Log("Checking word: " + playerInputString);

        if (playerInputString == Answer)
        {
            Debug.Log("Correct!!");
            gameManager.GameOver();
            return true;
        }

        for (int i = 0; i < FakeAnswers.Length; i++)
        {
            if (playerInputString == FakeAnswers[i])
            {
                Debug.Log("Fake answer " + i);
                ReplaceFakeAnswer(FakeAnswers[i]);
                return true;
            }
        }

        return false;
    }

    void ReplaceFakeAnswer(string fakeAnswer)
    {
        string strToReplaceFakeAnswer = "";

        for (int k = 0; k < fakeAnswer.Length; k++)
        {
            strToReplaceFakeAnswer += ".";
        }

        for (int i = 0; i < gsArray.Length; i++)
        {
            if (gsArray[i].str.Contains(fakeAnswer))
            {
                gsArray[i].str = gsArray[i].str.Replace(fakeAnswer, strToReplaceFakeAnswer);
                break;
            }
        }
    }
}


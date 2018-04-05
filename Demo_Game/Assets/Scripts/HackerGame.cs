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
    public bool wordGuessed;
}

public class HackerGame : MonoBehaviour {

    public GameManager gameManager;

    // Screen Text
    [Header("Screen Text")]
    public TextMeshProUGUI playerInputString;
    public TextMeshProUGUI gameStrings;
    public TextMeshProUGUI playerInputResponse;
    public TextMeshProUGUI remainingAttempts;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI timerText;

    // Answers
    [Header("Answers")]
    public string CorrectAnswer;
    [Header("Maximum of 8 Fake Answers, Do Not Leave Blank!")]
    public string[] FakeAnswers;

    // Player 
    private int stringCharLimit; // maximum number of typed characters allowed on screen
    private int numberOfAttempts;
    private int currentRemainingAttempts;

    // Game Strings - represent each line on the screen 
    private GameString[] gsArray = new GameString[10];

    // Characters to be randomly populated in the game strings
    private string randomChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ~!@#$%^&*()-+={}[]|:;<>,.?/";

    private int gameStringLength; // Length of each of the game strings on the screen

    // Game timer
    private static float gameTimer;
    private float currentGameTimer;

    void Start ()
    {
        // Set variable values
        stringCharLimit = 10;
        gameStringLength = 30;
        numberOfAttempts = 4;
        gameTimer = 61;
    }
	
	void Update ()
    {
		if (GameManager.CurrentState == GameManager.GameState.GAME)
        {
            CheckPlayerInputAndDisplay();           
        }
	}

    void FixedUpdate()
    {
        if (GameManager.CurrentState == GameManager.GameState.GAME)
        {
            // Reduce timer by delta time
            currentGameTimer -= Time.deltaTime;

            // Convert to integer for rounding
            int intTimer = (int)currentGameTimer;

            // Check if time has run out
            if (intTimer < 1)
            {
                GameOver("You Lose\nYou ran out of time!");
            }

            // Display timer text by updating on screen text
            timerText.text = "Time remaining: " + intTimer.ToString();
        }
    }

    // Check player keyboard input and display to screen by updating on screen text
    void CheckPlayerInputAndDisplay()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
        {
            // Mouse input
        }

        // Pressing enter checks the current word
        else if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            // Check if word matches either the correct or one of the fake answers
            if (!CheckWord(playerInputString.text))
            {
                playerInputResponse.text = "Word not found!";
            }

            DisplayGameStrings();

            // Reset the input string
            playerInputString.text = "";

            // Display remaining attempts
            remainingAttempts.text = "Remaning attempts: " + currentRemainingAttempts.ToString();

            // Check for game over - no attmepts left
            if (currentRemainingAttempts < 1)
            {
                GameOver("You Lose\nNo more attempts!");
            }
        }

        else if (Input.GetKeyDown(KeyCode.Backspace))
        {
            if (playerInputString.text.Length > 0)
            {
                // Remove last character of string
                playerInputString.text = playerInputString.text.Remove(playerInputString.text.Length - 1, 1);
            }
        }

        // Any other key input
        else if (Input.anyKeyDown && playerInputString.text.Length < stringCharLimit)
        {
            // Add underscore for a space
            if (Input.GetKeyDown(KeyCode.Space))           
                playerInputString.text += "_";
            else
                // Convert lower case letters to upper case - since all answers are in upper case
                playerInputString.text += Input.inputString.ToUpper();

            // Remove the previous clue text when the player starts typing the next word
            if (playerInputResponse.text != "")
            {
                playerInputResponse.text = "";
            }
        }
    }

    void GameSetup()
    {
        // Reset variables
        currentRemainingAttempts = numberOfAttempts;
        currentGameTimer = gameTimer;
        playerInputString.text = "";
        playerInputResponse.text = "";
        remainingAttempts.text = "Remaning attempts: " + currentRemainingAttempts;

        // Set new game strings
        SetRandomGameStrings();
        AddAnswersToGameStrings();
        DisplayGameStrings();
    }

    // Display game strings to screen by updating the on screen text
    void DisplayGameStrings()
    {
        gameStrings.text = "";

        // Add each game string to the text on screen
        foreach (GameString gs in gsArray)
        {
            gameStrings.text += gs.str + "\n";
        }      
    }

    // Assign random characters from "randomChars" string to each of the game strings in the array
    void SetRandomGameStrings()
    {
        for (int i = 0; i < gsArray.Length; i++)
        {
            // Reset game string
            gsArray[i].str = "";
            gsArray[i].hasWordPlaced = false;
            gsArray[i].wordGuessed = false;

            for (int j = 0; j < gameStringLength; j++)
            {
                // Add random character from preset string of characters
                gsArray[i].str += randomChars[Random.Range(0, randomChars.Length - 1)];
            }
        }
    }

    // Insert the answer and fake answers into the random strings
    void AddAnswersToGameStrings()
    {
        // Add answer

        // Find random gamestring in the gsArray to place the answer and store in "i"
        int i = Random.Range(0, gsArray.Length - 1);

        // Reduce the length of the string by the length of the answer being inserted
        gsArray[i].str = gsArray[i].str.Substring(0, gsArray[i].str.Length - CorrectAnswer.Length);
        // Add answer in a random position
        gsArray[i].str = gsArray[i].str.Insert(Random.Range(0, gsArray[i].str.Length - 1), CorrectAnswer);
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

    // Checks the word entered against the correct and fake answers
    bool CheckWord(string playerInputString)
    {
        if (playerInputString == CorrectAnswer)
        {
            GameOver("You Win\nYou entered the correct password!");
            return true;
        }

        // Decrement number of remaining attempts for the incorrect answer
        currentRemainingAttempts--;

        // Check player word against the fake answers
        for (int i = 0; i < FakeAnswers.Length; i++)
        {
            if (playerInputString == FakeAnswers[i])
            {
                // Player entered word that matched a fake answer
                // Check if the replacement is made - guards against giving the clue to an already entered fake answer
                if (ReplaceFakeAnswer(FakeAnswers[i]))
                {
                    // Give clue if player is entering that fake answer for the first time
                    CheckFakeAnswerAndGiveClue(FakeAnswers[i]);
                }                        
                return true;
            }
        }

        return false;
    }

    bool ReplaceFakeAnswer(string fakeAnswer)
    {
        string strToReplaceFakeAnswer = "";

        // Make a string the same length as the fake answer made from "."
        for (int k = 0; k < fakeAnswer.Length; k++)
        {
            strToReplaceFakeAnswer += ".";
        }

        // Find the game string containing the fake answer and replace it
        for (int i = 0; i < gsArray.Length; i++)
        {
            // Only replace if it hasn't already been replaced
            if (gsArray[i].str.Contains(fakeAnswer) && gsArray[i].wordGuessed == false)
            {
                gsArray[i].str = gsArray[i].str.Replace(fakeAnswer, strToReplaceFakeAnswer);
                gsArray[i].wordGuessed = true;
                return true;
            }
        }

        // Returns false if player guesses a fake word they have already guessed and is no longer showing on the terminal
        playerInputResponse.text = "Word not found!";
        return false;
    }

    void CheckFakeAnswerAndGiveClue(string fakeAnswer)
    {
        int CorrectChars = 0;
        int CorrectlyPositionedChars = 0;

        for (int i = 0; i < CorrectAnswer.Length; i++)
        {
            if (fakeAnswer.Contains(CorrectAnswer[i].ToString()))
            {
                //Fake answer entered contains same character as the answer
                CorrectChars++;

                if (CorrectAnswer.IndexOf(CorrectAnswer[i]) == fakeAnswer.IndexOf(CorrectAnswer[i]))
                {
                    //Fake answer entered contains same character as the answer AND in the correct position (indexes match)
                    CorrectlyPositionedChars++;
                }
            }
        }

        // Check for 1 correct character - remove "s" as not plural
        string characters = "characters";

        if (CorrectChars == 1)
            characters = "character";

        // Update screen text with clue
        playerInputResponse.text = ("Word entered: " + fakeAnswer + "\n" + CorrectChars + " " + characters + " correct, " + CorrectlyPositionedChars + " in the correct position");
    }

    void GameOver(string _gameOverText)
    {
        gameManager.GameOver();

        // Set game over text on screen
        gameOverText.text = _gameOverText;
    }

    public void RestartGame()
    {
        gameManager.ResetGame();
        GameSetup();
    }

}


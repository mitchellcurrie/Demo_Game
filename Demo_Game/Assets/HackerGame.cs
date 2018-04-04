using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HackerGame : MonoBehaviour {

    public GameManager gameManager;

    // Player input
    public TextMeshProUGUI playerInputString;
    private int stringCharLimit;

    void Start ()
    {
        stringCharLimit = 10;
    }
	
	void Update ()
    {
		if (GameManager.CurrentState == GameManager.GameState.GAME)
        {
            CheckPlayerInputAndDisplay();
        }
	}

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
}

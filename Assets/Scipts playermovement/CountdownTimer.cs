using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CountdownTimer : MonoBehaviour
{
    public float maxTimeAllowed; // maximum time allowed to complete the maze (in seconds)
    public float minTimeRequired; // minimum time required to complete the maze (in seconds)
    public float timeTaken; // time taken by the player to complete the maze (in seconds)
    public float scorePercent; // score as a percentage (0-100%)

    public TextMeshProUGUI countdownText;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI scoreText;
    public Button tryAgainBtn;
    
    private bool gameFinished = false; // flag to indicate if the game has finished

    void Start()
    {
        gameOverText.enabled = false;
        tryAgainBtn.gameObject.SetActive(false);
        gameFinished = false;
    }

    void Update()
    {
        if (!gameFinished)
        {
            timeTaken += Time.deltaTime; // update the time taken while the game is running

            if (timeTaken >= maxTimeAllowed)
            {
                gameFinished = true;
                timeTaken = maxTimeAllowed; // limit the time taken to the max time allowed
                CalculateScore(); // calculate the final score
                EndGame(); // end the game
            }
        }

        countdownText.text = "Time: " + timeTaken.ToString("F2"); // update the countdown text
        if(timeTaken >= minTimeRequired)
        {
            CalculateScore(); // calculate the score
        }
        else if(timeTaken < minTimeRequired)
        {
            scoreText.text = "Score: 100%"; // update the score text
        } 
    }

    private void CalculateScore()
    {
        // Calculate the score as a percentage using the formula
        scorePercent = ((maxTimeAllowed - timeTaken) / (maxTimeAllowed - minTimeRequired)) * 100f;
        scoreText.text = "Score: " + scorePercent.ToString("F2") + "%"; // update the score text
    }

    private void EndGame()
    {
        gameOverText.enabled = true;
        tryAgainBtn.gameObject.SetActive(true);  
        // Implement code to end the game here (e.g., show a game over screen)
        Debug.Log("Game Over. Score: " + scorePercent + "%");
    }

    public void resetTheGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Debug.Log("Reset the game");
    }    

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Menu");
        Debug.Log("Load Main Menu");
    }
}

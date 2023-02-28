using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CountdownTimer : MonoBehaviour
{
    float currentTime = 0f;
    float startingTime = 20f;
    float startingScore = 100f;
    float currentScore = 0f; 

    public TextMeshProUGUI countdownText;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI scoreText;
    public Button tryAgainBtn;
    
    // Start is called before the first frame update
    void Start()
    {
        currentTime = startingTime;
    
        currentScore = startingScore;
        gameOverText.enabled = false;
    
        tryAgainBtn.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        currentTime -= 1 * Time.deltaTime;
        countdownText.text = "Time: " + currentTime.ToString("0.00");

        currentScore -= 10 * Time.deltaTime;
        scoreText.text = "Score: " + currentScore.ToString("0");
        if (currentTime <= 0){
            currentTime = 0;
            currentScore = 0;
            gameOverText.enabled = true;
            tryAgainBtn.gameObject.SetActive(true);  
        }
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

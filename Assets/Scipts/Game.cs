using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public Text scoreText;
    public int score;

    public void ScoreButton()
    {
        score++;
        scoreText.text = "Score: " + score;
    }

    public void ResetButton()
    {
       score = 0;
       scoreText.text = "Score: " + score;
    }
}

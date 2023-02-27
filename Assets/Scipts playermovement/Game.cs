using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Data;
using System.Data.SqlClient;

public class Game : MonoBehaviour
{
    public Text scoreText;
    public Text PlayerText;
    public int score;

    private string firstname;
    private string lastname;

    private string conn;

    void Start()
    {
        Debug.Log("Connecting to database...");
        conn = @"Data Source=127.0.0.1; user id=SA; password=Password1234; Initial Catalog=testdb;";

        SqlConnection dbconn = new SqlConnection(conn);

        try
        {
            dbconn.Open();
            Debug.Log("Connected to database!");
        }
        catch (System.Exception)
        {
            Debug.Log("Failed to connect to database!");
            throw;
        } 
        finally
        {
            dbconn.Close();
        }

        ShowPlayer();
    }

    
    public void SaveScore()
    {
        SqlConnection dbconn = new SqlConnection(conn);
        dbconn.Open();

        using (SqlCommand command = new SqlCommand("UPDATE Player SET Score = @score WHERE PlayerId IN (SELECT TOP(1) PlayerId from Player order by PlayerId desc)", dbconn))
        //using (SqlCommand command = new SqlCommand("INSERT INTO score (score) VALUES (@score)", dbconn))
        {
            command.Parameters.AddWithValue("@score", score);
            command.ExecuteNonQuery();
            Debug.Log("Score " + score + " added to database!");
        }

        dbconn.Close();
    }

    public void ShowPlayer()
    {
        SqlConnection dbconn = new SqlConnection(conn);
        dbconn.Open();

        using (SqlCommand command = new SqlCommand("SELECT TOP 1 * FROM Player ORDER BY PlayerId DESC", dbconn))
        {
            SqlDataReader reader= command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    firstname = reader[1].ToString();
                    lastname = reader[2].ToString();
                    Debug.Log("Player: " + firstname + " " + lastname);
                    PlayerText.text = "Player: " + firstname + " " + lastname;
                }
            }
            
        }
        dbconn.Close();
    }

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

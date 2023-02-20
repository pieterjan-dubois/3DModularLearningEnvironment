using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mono.Data.Sqlite;

public class LoginMenu : MonoBehaviour
{
    public InputField username;
    public InputField password;
    public Text PlayerInfo;

    private string dbName = "URI=file:Player.db";
    
    // Start is called before the first frame update
    void Start()
    {
        CreateDB();
        //DisplayPlayer();
    }

    public void CreateDB()
    {
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();
        
            // Create a new database connection:
            using(var command = connection.CreateCommand())
            {
                command.CommandText = "CREATE TABLE IF NOT EXISTS Player (id INTEGER PRIMARY KEY AUTOINCREMENT, username VARCHAR(50), password VARCHAR(50));";
                command.ExecuteNonQuery();
            }

            connection.Close();
        }
    }

    public void AddPlayer()
    {
        using(var connection = new SqliteConnection(dbName))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO Player (username, password) VALUES ('" + username.text + "', '" + password.text + "');";
                command.ExecuteNonQuery();
            }

            connection.Close();
        }
    }
/*
    public void DisplayPlayer()
    {
        PlayerInfo.text = "";

        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM Player";

                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                        PlayerInfo.text += reader["username"] + " " + reader["password"] + "\n";
                    
                    reader.Close();
                    
                }
            }

            connection.Close(); 
        }    
    }
*/
}

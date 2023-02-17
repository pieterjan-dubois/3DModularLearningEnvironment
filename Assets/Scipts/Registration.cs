using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using Npgsql;

public class Registration : MonoBehaviour
{
    public string host = "localhost";
    public string database = "mydatabase";
    public string user = "myuser";
    public string password = "mypassword";
    public string tableName = "mytable";

    public InputField nameInput;
    public InputField emailInput;
    public InputField passwordInput;

    public Button registerButton;

/*
    public void SaveData()
    {
        // Create connection to the database
        string connectionString = $"Host={host};Database={database};Username={user};Password={password}";
        using (var connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();

            // Construct SQL command to insert data into table
            string sql = $"INSERT INTO {tableName} (name, email, password) VALUES (@name, @email, @password)";
            using (var command = new NpgsqlCommand(sql, connection))
            {
                // Set parameter values for name and email
                command.Parameters.AddWithValue("name", nameInput.text);
                command.Parameters.AddWithValue("email", emailInput.text);
                command.Parameters.AddWithValue("password", passwordInput.text);

                // Execute the SQL command
                command.ExecuteNonQuery();
            }

            connection.Close();
        }
    }
*/

    private bool ValidateInput()
    {
        if (string.IsNullOrEmpty(nameInput.text))
        {
            Debug.Log("Name field is empty.");
            return false;
        }

        if (string.IsNullOrEmpty(emailInput.text))
        {
            Debug.Log("Email field is empty.");
            return false;
        }

        if (!IsValidEmail(emailInput.text))
        {
            Debug.Log("Invalid email format.");
            return false;
        }

        if (string.IsNullOrEmpty(passwordInput.text))
        {
            Debug.Log("Password field is empty.");
            return false;
        }

        if(passwordInput.text.Length < 8)
        {
            Debug.Log("Password must be at least 8 characters long.");
            return false;
        }

        return true;
    }

    private bool IsValidEmail(string email)
    {
        try {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch {
            return false;
        }
    }
}

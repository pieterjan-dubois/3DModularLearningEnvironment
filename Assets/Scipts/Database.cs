using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Data;
using System.Data.SqlClient;

public class Database : MonoBehaviour
{
    private string conn;
    public InputField name;
    public InputField lastname;

    void Start()
    {
        Debug.Log("Connecting to database...");
        conn = @"Data Source=127.0.0.1; user id=SA; password=Password1234; Initial Catalog=testdb;";

        //SqlClient dbconn = new SqlClient(conn);
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
    }

    public void AddPlayer()
    {
        SqlConnection dbconn = new SqlConnection(conn);
        dbconn.Open();

        using (SqlCommand command = new SqlCommand("INSERT INTO Player (name, lastname) VALUES (@name, @lastname)", dbconn))
        {
            command.Parameters.AddWithValue("@name", name.text);
            command.Parameters.AddWithValue("@lastname", lastname.text);
            command.ExecuteNonQuery();
            Debug.Log("Player " + name.text + " " + lastname.text + " added to database!");
        }

        dbconn.Close();
    }
}

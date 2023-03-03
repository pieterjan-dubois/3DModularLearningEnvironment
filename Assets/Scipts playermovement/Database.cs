using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Data;
using System.Data.SqlClient;

public class Database : MonoBehaviour
{
    private string conn;
    public InputField firstname;
    public InputField lastname;

    void Start()
    {
        Debug.Log("Connecting to database...");
        conn = @"Data Source=127.0.0.1; user id=SA; password=Password1234; Initial Catalog=3D_Modular_Gaming_Tool;";

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

        using (SqlCommand command = new SqlCommand("INSERT INTO Player (firstName, lastName) VALUES (@firstName, @lastName)", dbconn))
        {
            command.Parameters.AddWithValue("@firstName", firstname.text);
            command.Parameters.AddWithValue("@lastName", lastname.text);
            command.ExecuteNonQuery();
            Debug.Log("Player " + firstname.text + " " + lastname.text + " added to database!");
        }

        dbconn.Close();
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using System.Data;
using System.Data.SqlClient;

public class EditorDatabase : MonoBehaviour
{

    private string conn;
    
    
    // Start is called before the first frame update
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}

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

    public void SaveLevel(LevelEditor level, string name)
    {
        Debug.Log("Saving level " + name + " to database...");

        SqlConnection dbconn = new SqlConnection(conn);
        dbconn.Open();

        List<CreatedObject.Data> createdObjects = level.createdObjects;
        List<FloorData.Data> floors = level.floors;

        //if level in database update, else add
        using (SqlCommand commandSelect = new SqlCommand("SELECT * FROM Level WHERE name = @name", dbconn))
        {
            commandSelect.Parameters.AddWithValue("@name", name);
            using (SqlDataReader reader = commandSelect.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    Debug.Log("Level " + name + " already exists in database, updating...");
                    //update level
                    
                }
                else
                {
                    Debug.Log("Level " + name + " does not exist in database, adding...");
                    //Save level

                    using (SqlCommand command = new SqlCommand("INSERT INTO Level (levelname) VALUES (@levelname)", dbconn))
                    {
                        command.Parameters.AddWithValue("@levelname", name);
                        command.ExecuteNonQuery();
                    }

                    //Save objects
                    foreach (CreatedObject.Data obj in createdObjects)
                    {
                        using (SqlCommand command = new SqlCommand("INSERT INTO Object (type, position, rotation, scale, level) VALUES (@type, @position, @rotation, @scale, @level)", dbconn))
                        {
                            command.Parameters.AddWithValue("@type", obj.tag);
                            command.Parameters.AddWithValue("@position", obj.position.ToString());
                            command.Parameters.AddWithValue("@rotation", obj.rotation.ToString());
                            command.Parameters.AddWithValue("@scale", obj.scale.ToString());
                            command.Parameters.AddWithValue("@level", name);
                            command.ExecuteNonQuery();
                        }
                    }

                    //Save floors

                    foreach (FloorData.Data floor in floors)
                    {

                        Debug.Log("Path : " + floor.floorPlanPath);

                        if (floor.floorPlanPath != null || floor.floorPlanPath != "")
                        {
                            using (SqlCommand command = new SqlCommand("INSERT INTO Floor (floorLevel, floorplan, level) VALUES (@floorLevel, @floorplan, @level)", dbconn))
                            {
                                command.Parameters.AddWithValue("@floorLevel", floor.floorNumber);
                                command.Parameters.AddWithValue("@floorplan", floor.floorPlanPath);
                                command.Parameters.AddWithValue("@level", name);
                                command.ExecuteNonQuery();
                            }

                        }
                        else
                        {
                            using (SqlCommand command = new SqlCommand("INSERT INTO Floor (floorLevel, level) VALUES (@floorLevel, @level)", dbconn))
                            {
                                command.Parameters.AddWithValue("@floorLevel", floor.floorNumber);
                                command.Parameters.AddWithValue("@level", name);
                                command.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }


        }



        dbconn.Close();
    }
}

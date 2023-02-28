using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

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

        //check if level table exists
        using (SqlCommand command = new SqlCommand("SELECT * FROM sys.tables WHERE name = 'Level'", dbconn))
        {
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (!reader.HasRows)
                {
                    Debug.Log("Level table does not exist, creating table...");
                    CreateLevelTables(dbconn);


                }
            }
        }

        List<CreatedObject.Data> createdObjects = level.createdObjects;
        List<FloorData.Data> floors = level.floors;

        //if level in database update, else add
        using (SqlCommand commandSelect = new SqlCommand("SELECT * FROM Level WHERE levelname = @levelname", dbconn))
        {
            commandSelect.Parameters.AddWithValue("@levelname", name);
            using (SqlDataReader reader = commandSelect.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    Debug.Log("Level " + name + " already exists in database, updating...");

                    reader.Close();

                    DeleteLevel(name, dbconn);

                }


                reader.Close();
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

                    if (!string.IsNullOrEmpty(floor.floorPlanPath))
                    {
                        Debug.Log("Path : " + floor.floorPlanPath);

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

        dbconn.Close();
    }

    void CreateLevelTables(SqlConnection dbconn)
    {
        using (SqlCommand createTable = new SqlCommand("CREATE TABLE Level(LevelName VARCHAR (50) NOT NULL, TimeLimit INT NULL, CONSTRAINT PK_Level PRIMARY KEY CLUSTERED(LevelName ASC)", dbconn))
        {
            createTable.ExecuteNonQuery();
        }

        using (SqlCommand createTable = new SqlCommand("CREATE TABLE Object(ObjectID INT IDENTITY (1, 1) NOT NULL,Type VARCHAR (50) NOT NULL, Position VARCHAR (50) NOT NULL,Rotation VARCHAR (50) NOT NULL,Scale VARCHAR (50) NOT NULL,Level    VARCHAR (50) NOT NULL,CONSTRAINT PK_Object PRIMARY KEY CLUSTERED ([ObjectID] ASC),CONSTRAINT FK_LevelName FOREIGN KEY (Level) REFERENCES Level (LevelName) ON DELETE CASCADE;", dbconn))
        {
            createTable.ExecuteNonQuery();
        }

        using (SqlCommand createTable = new SqlCommand("CREATE TABLE Floor (FloorLevel INT NOT NULL, Floorplan  VARCHAR (MAX) NULL,Level      VARCHAR (50)  NOT NULL, CONSTRAINT PK_Floor PRIMARY KEY CLUSTERED (FloorLevel ASC, Level ASC), CONSTRAINT FK_LevelNameFloor FOREIGN KEY (Level) REFERENCES Level (LevelName) ON DELETE CASCADE;", dbconn))
        {
            createTable.ExecuteNonQuery();
        }

    }

    public LevelEditor LoadLevel(string name)
    {
        Debug.Log("Loading level " + name + " from database...");

        LevelEditor level = new LevelEditor();
        level.createdObjects = new List<CreatedObject.Data>();
        level.floors = new List<FloorData.Data>();

        SqlConnection dbconn = new SqlConnection(conn);
        dbconn.Open();

        using (SqlCommand command = new SqlCommand("SELECT * FROM Level WHERE levelname = @levelname", dbconn))
        {
            command.Parameters.AddWithValue("@levelname", name);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {

                    reader.Close();

                    // Load objects
                    using (SqlCommand objCommand = new SqlCommand("SELECT * FROM Object WHERE level = @levelname", dbconn))
                    {
                        objCommand.Parameters.AddWithValue("@levelname", name);
                        using (SqlDataReader objReader = objCommand.ExecuteReader())
                        {
                            while (objReader.Read())
                            {
                                CreatedObject.Data objData = new CreatedObject.Data();
                                objData.tag = objReader.GetString(objReader.GetOrdinal("type"));
                                objData.position = StringToVector3(objReader.GetString(objReader.GetOrdinal("position")));
                                objData.rotation = StringToQuaternion(objReader.GetString(objReader.GetOrdinal("rotation")));
                                objData.scale = StringToVector3(objReader.GetString(objReader.GetOrdinal("scale")));
                                level.createdObjects.Add(objData);
                            }
                        }
                    }

                    // Load floors
                    using (SqlCommand floorCommand = new SqlCommand("SELECT * FROM Floor WHERE level = @levelname", dbconn))
                    {
                        floorCommand.Parameters.AddWithValue("@levelname", name);
                        using (SqlDataReader floorReader = floorCommand.ExecuteReader())
                        {
                            while (floorReader.Read())
                            {
                                FloorData.Data floorData = new FloorData.Data();
                                floorData.floorNumber = floorReader.GetInt32(floorReader.GetOrdinal("floorLevel"));
                                if (!floorReader.IsDBNull(floorReader.GetOrdinal("floorplan")))
                                {
                                    floorData.floorPlanPath = floorReader.GetString(floorReader.GetOrdinal("floorplan"));
                                }
                                level.floors.Add(floorData);
                            }
                        }
                    }

                    Debug.Log("Level " + name + " loaded from database.");
                }
                else
                {
                    Debug.Log("Level " + name + " does not exist in database.");
                }
            }
        }

        dbconn.Close();

        return level;
    }

    private Vector3 StringToVector3(string s)
    {
        Debug.Log("StringToVector3 input: " + s);
        string[] split = s.Trim(new char[] { '(', ')' }).Split(',');
        Debug.Log("StringToVector3 split: " + split[0] + " " + split[1] + " " + split[2]);
        return new Vector3(float.Parse(split[0], CultureInfo.InvariantCulture), float.Parse(split[1], CultureInfo.InvariantCulture), float.Parse(split[2], CultureInfo.InvariantCulture));
    }


    private Quaternion StringToQuaternion(string s)
    {
        string[] split = s.Trim(new char[] { '(', ')' }).Split(',');
        return new Quaternion(float.Parse(split[0], CultureInfo.InvariantCulture), float.Parse(split[1], CultureInfo.InvariantCulture), float.Parse(split[2], CultureInfo.InvariantCulture), float.Parse(split[3], CultureInfo.InvariantCulture));
    }

    public void DeleteLevel(string name)
    {
        SqlConnection dbconn = new SqlConnection(conn);
        dbconn.Open();

        using (SqlCommand command = new SqlCommand("DELETE FROM Object WHERE level = @levelname", dbconn))
        {
            command.Parameters.AddWithValue("@levelname", name);
            command.ExecuteNonQuery();
        }

        using (SqlCommand command = new SqlCommand("DELETE FROM Floor WHERE level = @levelname", dbconn))
        {
            command.Parameters.AddWithValue("@levelname", name);
            command.ExecuteNonQuery();
        }

        using (SqlCommand command = new SqlCommand("DELETE FROM Level WHERE levelname = @levelname", dbconn))
        {
            command.Parameters.AddWithValue("@levelname", name);
            command.ExecuteNonQuery();
        }

        dbconn.Close();
    }

    public void DeleteLevel(string name, SqlConnection dbconn)
    {
        using (SqlCommand command = new SqlCommand("DELETE FROM Level WHERE levelname = @levelname", dbconn))
        {
            command.Parameters.AddWithValue("@levelname", name);
            command.ExecuteNonQuery();
        }
    }

}
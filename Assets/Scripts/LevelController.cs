using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static System.Net.Mime.MediaTypeNames;
using System.IO;

public class LevelController : MonoBehaviour
{
    public List<CreatedObject.Data> createdObjects;
    public Button save;
    public Button load;
    private GameObject[] placeableObjectPrefabs;

    private LevelEditor level;

    // Start is called before the first frame update
    void Start()
    {
        save.onClick.AddListener(SaveLevel);
        load.onClick.AddListener(LoadLevel);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.S))
        {
            SaveLevel();
        }

        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.L))
        {
            LoadLevel();
        }
    }

    public void SaveLevel()
    {
        level = GetComponent<PlacementController>().level;

        Debug.Log("Saving.");


        string json = JsonUtility.ToJson(level);
        string folder = UnityEngine.Application.dataPath + "/Saved/";
        string levelFile = "new_level.json";

        Debug.Log("Saving..");

        //Create new directory if LevelData directory does not yet exist.
        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);

        Debug.Log("Saving...");

        string path = Path.Combine(folder, levelFile); // set filepath

        //Overwrite file with same name, if applicable
        if (File.Exists(path))
            File.Delete(path);

        Debug.Log("Saving....");

        // create and save file
        File.WriteAllText(path, json);

        Debug.Log("Saved succesfully");


    }


    // Loading a level
    public void LoadLevel()
    {
        string folder = UnityEngine.Application.dataPath + "/Saved/";
        string levelFile = "new_level.json";

        string path = Path.Combine(folder, levelFile); // set filepath

        if (File.Exists(path)) // if the file could be found in LevelData
        {
            Debug.Log("Loading level");
            
            CreatedObject[] foundObjects = FindObjectsOfType<CreatedObject>();
            foreach (CreatedObject obj in foundObjects)
                Destroy(obj.gameObject);

            Debug.Log("Loading level.");

            string json = File.ReadAllText(path); // provide text from json file
            level = JsonUtility.FromJson<LevelEditor>(json); // level information filled from json file

            Debug.Log("Loading level..");

            CreateFromFile(); // create objects from level data.

            Debug.Log("Level loaded");
        }
        else // if the file could not be found
        {
            Debug.Log("File not found");
        }
    }

    void CreateFromFile()
    {
        placeableObjectPrefabs = GetComponent<PlacementController>().placeableObjectPrefabs;

        foreach (CreatedObject.Data data in level.createdObjects)
        {
            Debug.Log("Loading object..");
            for (int i = 0; i < placeableObjectPrefabs.Length; i++)
            {
                if (placeableObjectPrefabs[i].tag == data.tag)
                {
                    Debug.Log("Creating object..");
                    GameObject obj = Instantiate(placeableObjectPrefabs[i], data.position, data.rotation);
                    obj.transform.localScale = data.scale;

                    CreatedObject newObjData = obj.AddComponent<CreatedObject>();
                    newObjData.data = data;

                }
            }
        }
    }
}

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
        
    }
}

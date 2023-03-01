using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    private Button loadLevel;
    private string activeLevel = "House";

    public GameObject[] placeableObjectPrefabs;

    private LevelEditor level;

    // Start is called before the first frame update
    void Start()
    {   
        loadLevel = GameObject.Find("LoadButton").GetComponent<Button>();
        loadLevel.onClick.AddListener(LoadLevel);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LoadLevel()
    {
        /*activeLevel = PlayerPrefs.GetString("activeLevel");*/

        Debug.Log("Loading level");

        level = GetComponent<EditorDatabase>().LoadLevel(activeLevel);

        if (level == null)
        {
            Debug.Log("Level not found");
        }
        else
        {

            CreateFromFile(); // create objects from level data.

            /*Debug.Log("Loading floors");

            List<FloorData.Data> floors = level.floors;

            Debug.Log("Loading level...");

            foreach (FloorData.Data floor in floors)
                GameObject.Find("Floors").GetComponent<FloorplanController>().LoadFloorplanFromSave(floor.floorNumber, floor.floorPlanPath);

            GameObject.Find("Floors").GetComponent<FloorplanController>().ActivateGroundFloor();

            Debug.Log("Level loaded");*/
        }

    }

    void CreateFromFile()
    {
        foreach (CreatedObject.Data data in level.createdObjects)
        {
            Debug.Log("Loading object..");
            for (int i = 0; i < placeableObjectPrefabs.Length; i++)
            {
                if (placeableObjectPrefabs[i].tag == data.tag)
                {
                    Debug.Log("Creating object " + data.tag);
                    GameObject obj = Instantiate(placeableObjectPrefabs[i], data.position, data.rotation);
                    obj.transform.localScale = data.scale;

                    CreatedObject newObjData = obj.AddComponent<CreatedObject>();
                    newObjData.data = data;

                }
            }
        }
    }
}

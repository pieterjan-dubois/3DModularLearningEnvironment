using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.IO;

public class FloorplanController : MonoBehaviour
{
    private Button uploadButton;
    private GameObject floors;

    public GameObject floorPlane;

    private GameObject currentFloorPlane;

    private string imagePath;
    private GameObject UI;
    private int clickCount = 0;

    private List<Button> uploadButtons = new List<Button>();

    private int currentFloor;

    private Dictionary<int, GameObject> floorplans;

    // Start is called before the first frame update
    void Start()
    {
        floors = GameObject.Find("Floors");
        uploadButton = GameObject.Find("UploadButton").GetComponent<Button>();
        uploadButton.onClick.AddListener(Upload);
        uploadButtons.Add(uploadButton);
        currentFloor = 0;
        currentFloorPlane = GameObject.Find("Ground");

        FloorData floorData = currentFloorPlane.AddComponent<FloorData>();
        floorData.data.floorNumber = currentFloor;

        GameObject.Find("Mouse").GetComponent<PlacementController>().level.floors.Add(floorData.data);
        Debug.Log(GameObject.Find("Mouse").GetComponent<PlacementController>().level.floors[0].floorNumber);

        floorplans = new Dictionary<int, GameObject>();
        floorplans.Add(0, currentFloorPlane);


        UI = GameObject.Find("UI");


    }

    // Update is called once per frame
    void Update()
    {

        if (UI.GetComponent<UIController>().allowInput)
        {
            Ray ray;
            RaycastHit hit;

            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == currentFloorPlane)
                {
                    //on double click
                    if (Input.GetMouseButtonDown(0))
                    {
                        clickCount++;
                        if (clickCount == 2)
                        {
                            uploadButton.gameObject.SetActive(!uploadButton.gameObject.activeSelf);
                            clickCount = 0;
                        }
                        else
                        {
                            StartCoroutine(ClickTimer());
                        }
                    }
                }

            }

            if (Input.GetKeyDown(KeyCode.PageUp))
            {
                currentFloor++;
                SwitchFloor();
            }
            if (Input.GetKeyDown(KeyCode.PageDown))
            {
                if (currentFloor > 0)
                {
                    currentFloor--;
                    SwitchFloor();
                }

            }


        }
        else
        {
            uploadButton.gameObject.SetActive(false);

        }
    }

    void Upload()
    {
        // Open file explorer to select image file
        imagePath = UnityEditor.EditorUtility.OpenFilePanel("Select Image", "", "png,jpg,jpeg");

        if (imagePath != "")
        {
            // Load image from file path
            byte[] imageData = File.ReadAllBytes(imagePath);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(imageData);


            Material floorplan = new Material(Shader.Find("Standard"));
            floorplan.mainTexture = texture;

            currentFloorPlane.GetComponent<MeshRenderer>().material = floorplan;

            uploadButton.gameObject.SetActive(false);


            currentFloorPlane.GetComponent<FloorData>().data.floorPlanPath = imagePath;

            GameObject.Find("Mouse").GetComponent<PlacementController>().level.floors[currentFloor] = currentFloorPlane.GetComponent<FloorData>().data;


            UI.GetComponent<UIController>().messagePanel.SetActive(true);
            UI.GetComponent<UIController>().message.text = "Vloerplan succesvol geupload";
            StartCoroutine(UI.GetComponent<UIController>().CloseMessagePanel());


        }
        else
        {
            UI.GetComponent<UIController>().messagePanel.SetActive(true);
            UI.GetComponent<UIController>().message.text = "Geen vloerplan geselecteerd";
            StartCoroutine(UI.GetComponent<UIController>().CloseMessagePanel());
        }

    }

    public void LoadFloorplanFromSave(int floor, string imagePath)
    {

        Debug.Log("Loading floorplan from save");

        Debug.Log("Floor " + floor + " " + imagePath);

        if (floor == 0)
        {
            Debug.Log("Adding ground floor");

            currentFloorPlane = GameObject.Find("Ground");
            floorplans.Add(0, currentFloorPlane);

            FloorData floorData = currentFloorPlane.AddComponent<FloorData>();
            floorData.data.floorNumber = floor;

            GameObject.Find("Mouse").GetComponent<PlacementController>().level.floors.Add(floorData.data);

        }
        else
        {
            Debug.Log("Adding floor " + floor);

            currentFloorPlane = Instantiate(floorPlane, floors.transform);
            currentFloorPlane.name = "Floor " + currentFloor;
            currentFloorPlane.transform.position = new Vector3(0, 3 * currentFloor, 0);
            floorplans.Add(floor, currentFloorPlane);

            uploadButton = GameObject.Find("UploadButton").GetComponent<Button>();
            uploadButton.onClick.AddListener(Upload);

            uploadButtons.Add(uploadButton);

            FloorData floorData = currentFloorPlane.AddComponent<FloorData>();
            floorData.data.floorNumber = currentFloor;

            GameObject.Find("Mouse").GetComponent<PlacementController>().level.floors.Add(floorData.data);
        }

        if (imagePath != "")
        {
            byte[] imageData = File.ReadAllBytes(imagePath);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(imageData);

            Debug.Log("Loading Image");

            Material floorplan = new Material(Shader.Find("Standard"));
            floorplan.mainTexture = texture;

            currentFloorPlane.GetComponent<MeshRenderer>().material = floorplan;

            Debug.Log("Setting Material");


            currentFloorPlane.GetComponent<FloorData>().data.floorPlanPath = imagePath;

            GameObject.Find("Mouse").GetComponent<PlacementController>().level.floors[currentFloor] = currentFloorPlane.GetComponent<FloorData>().data;


        }
    }

    IEnumerator ClickTimer()
    {
        yield return new WaitForSeconds(0.5f);
        clickCount = 0;
    }

    void SwitchFloor()
    {
        GameObject previousFloor = currentFloorPlane;


        if (!floorplans.ContainsKey(currentFloor))
        {
            currentFloorPlane = Instantiate(floorPlane, floors.transform);
            currentFloorPlane.name = "Floor " + currentFloor;
            currentFloorPlane.transform.position = new Vector3(0, 3 * currentFloor, 0);
            floorplans.Add(currentFloor, currentFloorPlane);

            uploadButton = GameObject.Find("UploadButton").GetComponent<Button>();
            uploadButton.onClick.AddListener(Upload);

            uploadButtons.Add(uploadButton);

            FloorData floorData = currentFloorPlane.AddComponent<FloorData>();
            floorData.data.floorNumber = currentFloor;

            GameObject.Find("Mouse").GetComponent<PlacementController>().level.floors.Add(floorData.data);

        }
        else
        {
            currentFloorPlane = floorplans[currentFloor];
        }
        
        previousFloor.SetActive(false);
        currentFloorPlane.SetActive(true);

        uploadButton = uploadButtons[currentFloor];

        UI.GetComponent<UIController>().messagePanel.SetActive(false);
        UI.GetComponent<UIController>().messagePanel.SetActive(true);
        UI.GetComponent<UIController>().message.text = "Niveau " + currentFloor;
        StartCoroutine(UI.GetComponent<UIController>().CloseMessagePanel());



    }



    public void ClearFloors()
    {
        foreach (KeyValuePair<int, GameObject> floor in floorplans)
        {
            if (floor.Key != 0)
            {
                Destroy(floor.Value);
            }
            else
            {
                /*floor.Value.GetComponent<MeshRenderer>().material = new Material(Shader.Find("Standard"));*/
                floor.Value.GetComponent<FloorData>().data.floorPlanPath = "";
                uploadButton = uploadButtons[0];
            }
        }

        floorplans.Clear();
        uploadButtons.Clear();

        uploadButtons.Add(uploadButton);
    }

    public void ActivateGroundFloor()
    {
        currentFloorPlane = GameObject.Find("Ground");
        currentFloorPlane.SetActive(true);
        currentFloor = 0;
        uploadButton = uploadButtons[0];
        uploadButton.gameObject.SetActive(true);
    }



}


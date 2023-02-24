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

    private int currentFloor;

    private Dictionary<int, GameObject> floorplans;

    // Start is called before the first frame update
    void Start()
    {
        floors = GameObject.Find("Floors");
        uploadButton = GameObject.Find("UploadButton").GetComponent<Button>();
        Debug.Log("Upload Button: " + uploadButton);
        uploadButton.onClick.AddListener(Upload);
        currentFloor = 0;
        currentFloorPlane = GameObject.Find("Ground");
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
                        Debug.Log("Click Count: " + clickCount);
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

        Debug.Log("Imagepath: " + imagePath);

        if (imagePath != "")
        {
            // Load image from file path
            byte[] imageData = File.ReadAllBytes(imagePath);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(imageData);


            Material floorplan = new Material(Shader.Find("Standard"));
            floorplan.mainTexture = texture;

            Debug.Log(currentFloorPlane);
            currentFloorPlane.GetComponent<MeshRenderer>().material = floorplan;

            uploadButton.gameObject.SetActive(false);

            GameObject.Find("Mouse").GetComponent<PlacementController>().level.floorPlanPath = imagePath;


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

    public void LoadFloorplanFromSave(string imagePath, int floor)
    {
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

            GameObject.Find("Mouse").GetComponent<PlacementController>().level.floorPlanPath = imagePath;


        }
    }

    IEnumerator ClickTimer()
    {
        yield return new WaitForSeconds(0.5f);
        clickCount = 0;
    }

    void SwitchFloor()
    {
        Debug.Log("Current Floor: " + currentFloorPlane);
        GameObject previousFloor = currentFloorPlane;


        if (!floorplans.ContainsKey(currentFloor))
        {
            currentFloorPlane = Instantiate(floorPlane, floors.transform);
            currentFloorPlane.name = "Floor " + currentFloor;
            currentFloorPlane.transform.position = new Vector3(0, 3 * currentFloor, 0);
            floorplans.Add(currentFloor, currentFloorPlane);

        }
        else
        {
            currentFloorPlane = floorplans[currentFloor];
        }
        currentFloorPlane.SetActive(true);
        previousFloor.SetActive(false);
        uploadButton = GameObject.Find("UploadButton").GetComponent<Button>();
        uploadButton.onClick.AddListener(Upload);
        /*uploadButton = currentFloorPlane.transform.Find("UploadButton").GetComponent<Button>();*/

    }



}


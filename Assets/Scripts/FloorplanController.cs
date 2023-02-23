using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.IO;

public class FloorplanController : MonoBehaviour
{
    private Button uploadButton;
    private GameObject ground;

    private string imagePath;
    public Material floorplan;
    private GameObject UI;
    private int clickCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        ground = GameObject.Find("Ground");
        uploadButton = GameObject.Find("UploadButton").GetComponent<Button>();
        Debug.Log("Upload Button: " + uploadButton);
        uploadButton.onClick.AddListener(Upload);

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
                if (hit.collider.gameObject.name == "Ground")
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

            floorplan.mainTexture = texture;

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

    public void LoadFloorplanFromSave(string imagePath)
    {
        if (imagePath != "")
        {
            // Load image from file path
            byte[] imageData = File.ReadAllBytes(imagePath);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(imageData);

            floorplan.mainTexture = texture;

            uploadButton.gameObject.SetActive(false);

            GameObject.Find("Mouse").GetComponent<PlacementController>().level.floorPlanPath = imagePath;


        }
    }

    IEnumerator ClickTimer()
    {
        yield return new WaitForSeconds(0.5f);
        clickCount = 0;
    }


}

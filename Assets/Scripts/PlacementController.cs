using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class PlacementController : MonoBehaviour
{
    public GameObject[] placeableObjectPrefabs;

    private GameObject currentPlaceableObject;

    public Material placing;
    public Material selectedMaterial;

    private float height;
    private float initialHeight;

    private Material objectMaterial;

    public float gridSize = 0.05f;

    public LevelEditor level;

    private Button heightPlusButton;
    private Button heightMinusButton;
    private Button smallRotateButton;
    private Button largeRotateButton;
    private Button widthPlusButton;
    private Button widthMinusButton;
    private Button lengthPlusButton;
    private Button lengthMinusButton;
    private Button wall;
    private Button floor;
    private Button wallPart;
    private Button heightButton;
    private Button deleteButton;

    private TextMeshProUGUI heightText;
    private float heightForText;

    private GameObject selected;
    private Vector3 selectedPosition;

    private GameObject selectedObject;



    void Start()
    {
        heightPlusButton = GameObject.Find("HeightPlusButton").GetComponent<Button>();
        heightMinusButton = GameObject.Find("HeightMinusButton").GetComponent<Button>();
        heightPlusButton.onClick.AddListener(IncreaseHeight);
        heightMinusButton.onClick.AddListener(DecreaseHeight);

        smallRotateButton = GameObject.Find("RotateSmall").GetComponent<Button>();
        largeRotateButton = GameObject.Find("RotateLarge").GetComponent<Button>();
        smallRotateButton.onClick.AddListener(RotateObject);
        largeRotateButton.onClick.AddListener(RotateObjectQuick);

        widthPlusButton = GameObject.Find("WidthPlusButton").GetComponent<Button>();
        widthMinusButton = GameObject.Find("WidthMinusButton").GetComponent<Button>();
        widthPlusButton.onClick.AddListener(IncreaseWidth);
        widthMinusButton.onClick.AddListener(DecreaseWidth);

        lengthPlusButton = GameObject.Find("LengthPlusButton").GetComponent<Button>();
        lengthMinusButton = GameObject.Find("LengthMinusButton").GetComponent<Button>();
        lengthPlusButton.onClick.AddListener(IncreaseLength);
        lengthMinusButton.onClick.AddListener(DecreaseLength);

        selected = GameObject.Find("Selected");
        selectedPosition = selected.transform.position;
        selected.SetActive(false);

        wall = GameObject.Find("Wall").GetComponent<Button>();
        floor = GameObject.Find("Floor").GetComponent<Button>();
        wallPart = GameObject.Find("WallPart").GetComponent<Button>();
        wall.onClick.AddListener(() => { ChangeObject(0); });
        floor.onClick.AddListener(() => { ChangeObject(1); });
        wallPart.onClick.AddListener(() => { ChangeObject(2); });

        heightButton = GameObject.Find("HeightButton").GetComponent<Button>();
        heightText = heightButton.GetComponentInChildren<TextMeshProUGUI>();

        deleteButton = GameObject.Find("Delete").GetComponent<Button>();
        deleteButton.onClick.AddListener(() => { DestroyObject(selectedObject); });

    }


    // Update is called once per frame
    void Update()
    {
        if (!Input.GetKeyDown(KeyCode.LeftControl))
        {
            HandleNewObjectHotkey();
        }


        if (selectedObject == null && currentPlaceableObject != null)
        {


            MoveCurrentObjectToMouse();

            if (Input.GetMouseButtonDown(0))
            {
                CreateObject();

            }

            if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.LeftShift))
            {
                Destroy(currentPlaceableObject);
                heightText.text = "Hoogte";
                selected.SetActive(false);
            }
        }

        if (!selected.activeSelf)
        {
            if (!EventSystem.current.IsPointerOverGameObject()) // if not over UI
            {

                if (Input.GetMouseButtonDown(0))
                {
                    ToggleSelection();
                }

            }
        }



        if (Input.GetKey(KeyCode.Delete))
        {
            DestroyObject(selectedObject);
        }

        if (Input.GetMouseButtonDown(1))
        {
            RotateObjectQuick();
        }

        if (Input.GetMouseButtonDown(2))
        {
            RotateObject();

        }

        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            IncreaseHeight();
        }

        if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            DecreaseHeight();
        }

        if (Input.GetKeyDown(KeyCode.Keypad4) || (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.Keypad4)))
        {
            DecreaseWidth();
        }

        if (Input.GetKeyDown(KeyCode.Keypad6) || (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.Keypad6)))
        {
            IncreaseWidth();
        }

        if (Input.GetKeyDown(KeyCode.Keypad2) || (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.Keypad2)))
        {
            DecreaseLength();
        }

        if (Input.GetKeyDown(KeyCode.Keypad8) || (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.Keypad8)))
        {
            IncreaseLength();
        }

        if (selectedObject != null)
        {
            heightPlusButton.interactable = false;
            heightMinusButton.interactable = false;
            heightText.text = "Hoogte";
        }


    }

    private void HandleNewObjectHotkey()
    {

        for (int i = 0; i < placeableObjectPrefabs.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0 + 1 + i))
            {
                ChangeObject(i);

            }
        }
    }

    private void MoveCurrentObjectToMouse()
    {

        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition) * 2;
        Vector3 position = new Vector3(SnapToGrid(mousePosition.x), height, SnapToGrid(mousePosition.z));
        currentPlaceableObject.transform.position = Vector3.Lerp(transform.position, position, 1f);
    }

    void CreateObject()
    {
        if (!EventSystem.current.IsPointerOverGameObject()) // if not over UI
        {
            if (currentPlaceableObject != null)
            {
                GameObject newObj = Instantiate(currentPlaceableObject);
                newObj.GetComponent<MeshRenderer>().material = objectMaterial;

                CreatedObject newObjData = newObj.AddComponent<CreatedObject>();
                newObjData.data.position = newObj.transform.position;
                newObjData.data.rotation = newObj.transform.rotation;
                newObjData.data.scale = newObj.transform.localScale;
                newObjData.data.tag = newObj.tag;

                level.createdObjects.Add(newObjData.data);
            }
        }

    }

    void RotateObject()
    {
        currentPlaceableObject.transform.Rotate(0, 15, 0);
    }

    void RotateObjectQuick()
    {
        currentPlaceableObject.transform.Rotate(0, 90, 0);
    }

    private float SnapToGrid(float n)
    {
        return (Mathf.Round(n / gridSize) * gridSize) / 2;
    }

    void DestroyObject(GameObject selectedObject)
    {
        Destroy(selectedObject);
        selectedObject = null;

    }

    void IncreaseHeight()
    {
        if (currentPlaceableObject.tag == "WallPart")
        {
            height += 0.25f;
            heightForText += 0.25f;
            heightText.text = "Hoogte: " + heightForText.ToString();
        }
        else
        {
            height += 3f;
            heightForText += 3f;
            heightText.text = "Hoogte: " + heightForText.ToString();
        }
    }

    void DecreaseHeight()
    {
        if (height > initialHeight)
        {
            if (currentPlaceableObject.tag == "WallPart")
            {
                height -= 0.5f;
                heightForText -= 0.5f;
                heightText.text = "Hoogte: " + heightForText.ToString();
            }
            else
            {
                height -= 3f;
                heightForText -= 3f;
                heightText.text = "Hoogte: " + heightForText.ToString();
            }

        }
    }

    void DecreaseLength()
    {
        if (currentPlaceableObject.transform.localScale.x > 0.5f)
        {
            currentPlaceableObject.transform.localScale -= new Vector3(0.05f, 0, 0);
        }
    }

    void IncreaseLength()
    {
        currentPlaceableObject.transform.localScale += new Vector3(0.05f, 0, 0);
    }

    void DecreaseWidth()
    {


        if (currentPlaceableObject.transform.localScale.z > 0.2f)
        {
            currentPlaceableObject.transform.localScale -= new Vector3(0, 0, 0.05f);
        }

    }

    void IncreaseWidth()
    {


        if (currentPlaceableObject.tag == "Floor" || currentPlaceableObject.transform.localScale.z < 3f)
        {
            currentPlaceableObject.transform.localScale += new Vector3(0, 0, 0.05f);
        }

    }

    void ChangeObject(int i)
    {


        if (currentPlaceableObject != null && selectedObject == null)
        {
            Destroy(currentPlaceableObject);
        }

        if (selectedObject != null)
        {
            selectedObject.GetComponent<MeshRenderer>().material = objectMaterial;
            selectedObject = null;
        }



        currentPlaceableObject = Instantiate(placeableObjectPrefabs[i]);
        objectMaterial = currentPlaceableObject.GetComponent<MeshRenderer>().material;
        currentPlaceableObject.GetComponent<MeshRenderer>().material = placing;
        placing.SetColor("_Color", new Color(0.3f, 0.8f, 1f, 0.5f));

        selected.SetActive(true);
        selected.transform.position = selectedPosition + new Vector3(100 * i, 0, 0);

        if (currentPlaceableObject.tag == "Floor")
        {
            height = 3f;
            initialHeight = 3f;
            heightForText = 3f;

        }
        else if (currentPlaceableObject.tag == "WallPart")
        {
            height = 0.25f;
            initialHeight = 0.25f;
            heightForText = 0;

        }
        else
        {
            height = 1.5f;
            initialHeight = 1.5f;
            heightForText = 0;

        }

        heightText.text = "Hoogte: " + heightForText.ToString();

    }

    void ToggleSelection()
    {

        Ray ray;
        RaycastHit hit;

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (selectedObject != null)
        {
            selectedObject.GetComponent<MeshRenderer>().material = objectMaterial;
            selectedObject = null;
            currentPlaceableObject = null;
        }

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject.tag == "Floor" || hit.transform.gameObject.tag == "WallPart" || hit.transform.gameObject.tag == "Wall")
            {
                
                selectedObject = hit.transform.gameObject;
                objectMaterial = selectedObject.GetComponent<MeshRenderer>().material;
                selectedObject.GetComponent<MeshRenderer>().material = selectedMaterial;
                currentPlaceableObject = selectedObject;
            
            }
        }





    }



}

using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class PlacementController : MonoBehaviour
{
    [SerializeField]
    private GameObject[] placeableObjectPrefabs;

    private GameObject currentPlaceableObject;

    public Material placing;

    private float height;
    private float initialHeight;

    private Material objectMaterial;

    public float gridSize = 0.5f;



    // Update is called once per frame
    void Update()
    {
        if (!Input.GetKeyDown(KeyCode.LeftControl))
        {
            HandleNewObjectHotkey();
        }


        if (currentPlaceableObject != null)
        {

            MoveCurrentObjectToMouse();
            if (Input.GetMouseButtonDown(1))
            {
                RotateObjectQuick();
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                RotateObject();

            }

            if (Input.GetKeyDown(KeyCode.KeypadPlus))
            {
                if (currentPlaceableObject.tag == "WallPart")
                {
                    height += 1f;
                }
                else
                {
                    height += 3f;
                }

            }

            if (Input.GetKeyDown(KeyCode.KeypadMinus))
            {
                if (height > initialHeight)
                {
                    if (currentPlaceableObject.tag == "WallPart")
                    {
                        height -= 1f;
                    }
                    else
                    {
                        height -= 3f;
                    }
                }
            }

            if (Input.GetKey(KeyCode.Keypad2) && currentPlaceableObject.transform.localScale.z > 0.2f)
            {
                currentPlaceableObject.transform.localScale -= new Vector3(0, 0, 0.05f);
            }

            if (Input.GetKey(KeyCode.Keypad8))
            {
                if (currentPlaceableObject.tag == "Floor" || currentPlaceableObject.transform.localScale.z < 3f)
                {
                    currentPlaceableObject.transform.localScale += new Vector3(0, 0, 0.05f);

                }
            }

            if (Input.GetKey(KeyCode.Keypad4) && currentPlaceableObject.transform.localScale.x > 0.5f)
            {
                currentPlaceableObject.transform.localScale -= new Vector3(0.05f, 0, 0);
            }

            if (Input.GetKey(KeyCode.Keypad6))
            {
                currentPlaceableObject.transform.localScale += new Vector3(0.05f, 0, 0);
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (currentPlaceableObject != null)
                {
                    CreateObject();
                }
            }

            if (Input.GetMouseButton(2) || Input.GetKey(KeyCode.LeftControl))
            {
                Destroy(currentPlaceableObject);
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                DestroyObject();
            }
        }


    }

    private void HandleNewObjectHotkey()
    {
        for (int i = 0; i < placeableObjectPrefabs.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0 + 1 + i))
            {
                if (currentPlaceableObject != null)
                {
                    Destroy(currentPlaceableObject);
                }

                currentPlaceableObject = Instantiate(placeableObjectPrefabs[i]);
                objectMaterial = currentPlaceableObject.GetComponent<MeshRenderer>().material;
                currentPlaceableObject.GetComponent<MeshRenderer>().material = placing;

                if (currentPlaceableObject.tag == "Floor")
                {
                    height = 3f;
                    initialHeight = 3f;
                }
                else if (currentPlaceableObject.tag == "WallPart")
                {
                    height = 0.5f;
                    initialHeight = 0.5f;
                }
                else
                {
                    height = 1.5f;
                    initialHeight = 1.5f;
                }

            }
        }
    }

    private void MoveCurrentObjectToMouse()
    {
        // Have the object follow the mouse cursor by getting mouse coordinates and converting them to world point.
        /* ray = Camera.main.ScreenPointToRay(Input.mousePosition);

         if (Physics.Raycast(ray, out hit))
         {
             currentPlaceableObject.transform.position = new Vector3(hit.point.x, 1.5f, hit.point.z);
         }*/

        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition) * 2;
        Vector3 position = new Vector3(SnapToGrid(mousePosition.x), height, SnapToGrid(mousePosition.z));
        currentPlaceableObject.transform.position = Vector3.Lerp(transform.position, position, 1f);
    }

    void CreateObject()
    {
        GameObject newObj = Instantiate(currentPlaceableObject);
        newObj.GetComponent<MeshRenderer>().material = objectMaterial;
        /*
                //Create object
                newObj = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                newObj.transform.position = transform.position;
                newObj.layer = 9; // set to Spawned Objects layer*/
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

    void DestroyObject()
    {
        Ray ray;
        RaycastHit hit;

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject.tag == "Floor" || hit.transform.gameObject.tag == "WallPart" || hit.transform.gameObject.tag == "Wall")
            {
                Destroy(hit.transform.gameObject);
            }
        }

    }



}

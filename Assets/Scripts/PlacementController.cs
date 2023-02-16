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



    // Update is called once per frame
    void Update()
    {
        HandleNewObjectHotkey();

        if (currentPlaceableObject != null)
        {

            MoveCurrentObjectToMouse();
            if (Input.GetMouseButtonDown(1))
            {
                RotateObject();
            }

            if (Input.GetMouseButtonDown(2))
            {
                RotateObjectQuick();
            }

            if (Input.GetKeyDown(KeyCode.KeypadPlus))
            {
                height += 3f;

            }

            if (Input.GetKeyDown(KeyCode.KeypadMinus))
            {
                if (height > initialHeight)
                {
                    height -= 3f;
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                CreateObject();
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
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector3 position = new Vector3(mousePosition.x, height, mousePosition.z);
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
}

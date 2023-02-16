using UnityEngine;
using UnityEngine.EventSystems;

public class PlacementController : MonoBehaviour
{
    [SerializeField]
    private GameObject[] placeableObjectPrefabs;

    private GameObject currentPlaceableObject;



    // Update is called once per frame
    void Update()
    {
        HandleNewObjectHotkey();

        if (currentPlaceableObject != null)
        {
            MoveCurrentObjectToMouse();
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
        Vector3 position = new Vector3(mousePosition.x, 1.5f, mousePosition.z);
        currentPlaceableObject.transform.position = Vector3.Lerp(transform.position, position, 1f);
    }

    void CreateObject()
    {
        GameObject newObj = Instantiate(currentPlaceableObject);
        /*
                //Create object
                newObj = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                newObj.transform.position = transform.position;
                newObj.layer = 9; // set to Spawned Objects layer*/
    }
}

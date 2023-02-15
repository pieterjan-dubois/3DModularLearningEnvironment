using System;
using UnityEngine;

public class PlacementController : MonoBehaviour
{
    [SerializeField]
    private GameObject[] placeableObjectPrefabs;

    private GameObject currentPlaceableObject;

    private float mouseWheelRotation;
    private int currentPrefabIndex = -1;

    private void Update()
    {
        HandleNewObjectHotkey();

        if (currentPlaceableObject != null)
        {
            MoveCurrentObjectToMouse();
            /*RotateFromMouseWheel();*/
            ReleaseIfClicked();
        }
    }

    private void HandleNewObjectHotkey()
    {
        for (int i = 0; i < placeableObjectPrefabs.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0 + 1 + i))
            {
                if (PressedKeyOfCurrentPrefab(i))
                {
                    Destroy(currentPlaceableObject);
                    currentPrefabIndex = -1;
                }
                else
                {
                    if (currentPlaceableObject != null)
                    {
                        Destroy(currentPlaceableObject);
                    }

                    currentPlaceableObject = Instantiate(placeableObjectPrefabs[i]);
                    currentPrefabIndex = i;
                }

                break;
            }
        }
    }

    private bool PressedKeyOfCurrentPrefab(int i)
    {
        return currentPlaceableObject != null && currentPrefabIndex == i;
    }

    private void MoveCurrentObjectToMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            currentPlaceableObject.transform.position = new Vector3(
            Mathf.Clamp(hitInfo.point.x, -20, 20),
            0.75f,
            Mathf.Clamp(hitInfo.point.z, -20, 20));
            /*currentPlaceableObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);*/
        }
    }

    /*private void RotateFromMouseWheel()
    {
        Debug.Log(Input.mouseScrollDelta);
        mouseWheelRotation += Input.mouseScrollDelta.y;
        currentPlaceableObject.transform.Rotate(Vector3.up, mouseWheelRotation * 10f);
    }*/

    private void ReleaseIfClicked()
    {
        if (Input.GetMouseButtonDown(0))
        {
            currentPlaceableObject = null;
        }
    }
}
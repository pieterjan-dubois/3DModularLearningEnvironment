using UnityEngine;
using UnityEngine.UI;


public class EditorCameraController : MonoBehaviour
{

    private float xAxis;
    private float yAxis;
    private float zoom;
    private Camera cam;

    private Vector3 prevPosition;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>(); // get the camera component for later use
    }

    // Update is called once per frame
    void Update()
    {
        xAxis = Input.GetAxis("Horizontal"); // get user input
        yAxis = Input.GetAxis("Vertical");

        zoom = Input.GetAxis("Mouse ScrollWheel") * 10;

        // move camera based on info from xAxis and yAxis
        transform.Translate(new Vector3(xAxis, yAxis, 0.0f));
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, -20, 20),
            Mathf.Clamp(transform.position.y, 50, 50),
            Mathf.Clamp(transform.position.z, -20, 20)); // limit camera movement to -20 min, 20 max. Y value remains 20.

        //change camera's orthographic size to create zooming in and out. Can only be between -25 and -5.
        cam.orthographicSize -= zoom;

        if (Input.GetKey(KeyCode.LeftControl))
        {
            transform.position = new Vector3(0, 60, -100);
            transform.rotation = Quaternion.Euler(30, 0, 0);
        }
        
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            transform.position = new Vector3(0, 50, 0);
            transform.rotation = Quaternion.Euler(90, 0, 0);
        }

    }
}

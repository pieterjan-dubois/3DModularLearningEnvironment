using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class UIController : MonoBehaviour
{

    private GameObject controlsText;
    private Button controls;
    private GameObject mainUI;
    // Start is called before the first frame update
    void Start()
    {
        controlsText = GameObject.Find("ControlsText");
        controlsText.SetActive(false);
        mainUI = GameObject.Find("MainUI");
        controls = GameObject.Find("Controls").GetComponent<Button>();
        controls.onClick.AddListener(ToggleControls);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            ToggleUI();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            ToggleControls();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    void ToggleControls()
    {
        controlsText.SetActive(!controlsText.activeSelf);
    }

    void ToggleUI()
    {
        mainUI.SetActive(!mainUI.activeSelf);
    }
}

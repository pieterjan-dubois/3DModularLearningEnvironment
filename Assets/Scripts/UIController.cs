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

    private GameObject saveAndLoadUI;
    private GameObject saveMenu;
    private GameObject loadMenu;
    private GameObject saveNameInputObject;
    private InputField saveNameInput;
    private Button saveFileButton;

    private Button saveButton;
    private Button loadButton;

    public GameObject mouse;

    // Start is called before the first frame update
    void Start()
    {
        controlsText = GameObject.Find("ControlsText");
        controlsText.SetActive(false);
        mainUI = GameObject.Find("MainUI");
        controls = GameObject.Find("Controls").GetComponent<Button>();
        controls.onClick.AddListener(ToggleControls);

        saveAndLoadUI = GameObject.Find("SaveAndLoadMenu");
        saveMenu = GameObject.Find("SaveMenu");
        /*loadMenu = GameObject.Find("LoadMenu");*/
        saveButton = GameObject.Find("SaveButton").GetComponent<Button>();
        loadButton = GameObject.Find("LoadButton").GetComponent<Button>();
        saveAndLoadUI.SetActive(false);
        saveMenu.SetActive(false);
        /*loadMenu.SetActive(false);*/

        saveButton.onClick.AddListener(OpenSaveMenu);
        /*loadButton.onClick.AddListener(OpenLoadMenu);*/
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

    void OpenSaveMenu()
    {
        saveAndLoadUI.SetActive(true);
        saveMenu.SetActive(true);
        /*loadMenu.SetActive(false);*/

        saveNameInputObject = GameObject.Find("SaveName");
        Debug.Log(saveNameInputObject);
        saveNameInput = saveNameInputObject.GetComponent<InputField>();
        Debug.Log(saveNameInput);

        saveFileButton = GameObject.Find("SaveFileButton").GetComponent<Button>();
        saveFileButton.onClick.AddListener(SaveLevel);

    }

    void OpenLoadMenu()
    {
        saveAndLoadUI.SetActive(true);
        saveMenu.SetActive(false);
        loadMenu.SetActive(true);
    }

    void SaveLevel()
    {
        string levelName = "Untitled";

        if (saveNameInput.text != "")
        {
            levelName = saveNameInput.text;
            Debug.Log(levelName);
        }

        mouse.GetComponent<LevelController>().SaveLevel(levelName);
    }
}

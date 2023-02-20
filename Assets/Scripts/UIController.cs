using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


    
public class UIController : MonoBehaviour
{

    private GameObject controlsText;
    private Button controls;
    // Start is called before the first frame update
    void Start()
    {
        controlsText = GameObject.Find("ControlsText");
        controlsText.SetActive(false);  
        controls = GameObject.Find("Controls").GetComponent<Button>();
        controls.onClick.AddListener(ToggleControls);
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    void ToggleControls()
    {
        controlsText.SetActive(!controlsText.activeSelf);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class OptionMenu : MonoBehaviour
{
    public InputField maxTime;
    public InputField minTime;

    public void addTimer()
    {
        Debug.Log("Max time: " + maxTime.text);
        Debug.Log("Min time: " + minTime.text);
        //GetComponent<EditorDatabase>().addTimers(int.Parse(maxTime.text), int.Parse(minTime.text));
        string t = GetComponent<DropdownHandler>().value;
        Debug.Log("Selected: " + t);
    } 
}

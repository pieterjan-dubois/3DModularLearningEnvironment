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

    EditorDatabase editor;

    public void addTimer()
    {
        Debug.Log("Max time: " + maxTime.text);
        Debug.Log("Min time: " + minTime.text);
        editor.addTimers();
    } 
}

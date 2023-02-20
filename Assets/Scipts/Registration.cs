using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Registration : MonoBehaviour
{
    public InputField nameInput;
    public InputField emailInput;
    public InputField passwordInput;

    //public Button registerButton;

    public void CallRegister()
    {
        Debug.Log("Hello " + nameInput.text);
    }
   
}

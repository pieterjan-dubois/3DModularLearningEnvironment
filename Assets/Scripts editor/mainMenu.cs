using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour
{
   public void PlayGame(){
        SceneManager.LoadScene("Game");
    }

   public void PlayWorldBuilder(){
        SceneManager.LoadScene("Editor");
    }

   public void QuitGame(){
      Debug.Log("quit");
      Application.Quit();
   }
    
}

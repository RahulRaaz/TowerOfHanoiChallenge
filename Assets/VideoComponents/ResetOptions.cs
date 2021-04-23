using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ResetOptions : MonoBehaviour
{
    public void mainMenu()
    {
        SceneManager.LoadScene(0); //stop video and go to main menu
    }
}

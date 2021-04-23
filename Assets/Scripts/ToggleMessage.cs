using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleMessage : MonoBehaviour
{
    public void toggleTextBox() //toggle pause message visibility
    {
        if (gameObject.activeSelf == true)
            gameObject.SetActive(false);
        else
            gameObject.SetActive(true);
    }        
}

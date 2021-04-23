using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UpdateTimeScore : MonoBehaviour
{
    public Text gTime;
    // Update is called once per frame
    void Update()
    {
        float timeTaken = Mathf.Round(MoveDisk.gameTime);
        gTime.text = "Time : " + timeTaken.ToString() + "s"; //Update current time to screen
    }
}

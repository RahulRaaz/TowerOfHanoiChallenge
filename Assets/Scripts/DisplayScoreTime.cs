using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayScoreTime : MonoBehaviour
{
    public Text timeT;
    // Start is called before the first frame update
    void Start()
    {
        print(MoveDisk.gameTime);
        timeT.text = MoveDisk.gameTime.ToString() + " seconds"; //Display final time taken
    }
}

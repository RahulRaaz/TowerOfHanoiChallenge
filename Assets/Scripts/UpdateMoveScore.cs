using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UpdateMoveScore : MonoBehaviour
{
    public Text gMoves;
    // Update is called once per frame
    void Update()
    {
        gMoves.text = "Moves : " + MoveDisk.moveCount.ToString(); //Update current move count to screen
    }
}

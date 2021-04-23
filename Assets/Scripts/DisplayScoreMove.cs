using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayScoreMove : MonoBehaviour
{
    public Text moves;
    // Start is called before the first frame update
    void Start()
    {
        print(MoveDisk.moveCount);
        moves.text = MoveDisk.moveCount.ToString() + " moves"; //display final move count
    }
}

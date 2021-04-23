using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideMessage : MonoBehaviour
{
    bool previousState = false;
    float ticker = 0.0f;
    float toggleTime = 0.0f;
    // Update is called once per frame
    void Update()
    {
        //hide error messages after 1s
        ticker += Time.deltaTime;
        bool currentState = gameObject.activeSelf;
        if (currentState)
        {
            if (!previousState) //check if previous state is false and start timer
            {
                toggleTime = ticker;
                previousState = true;
            }
            else if ((ticker - toggleTime) >= 1.0) //check if 1.5s has elapsed since messages enabled
            {
                previousState = false;
                gameObject.SetActive(false);
            }
        }
    }
}

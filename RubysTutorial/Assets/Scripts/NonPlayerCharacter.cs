using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonPlayerCharacter : MonoBehaviour
{
    //How long we want to display this dialog
    public float displayTime = 4.0f;

    //The dialog box that we want to appear
    public GameObject dialogBox;

    //The timer for how long it will be displayed
    float timerDisplay;

    // Start is called before the first frame update
    void Start()
    {
        //Not starting out as active
        dialogBox.SetActive(false);
        //Initialize below 0 so it is not active
        timerDisplay = -1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        //If the timer display is greater than or equal to 0 start the countdown for the display
        if (timerDisplay >= 0)
        {
            timerDisplay -= Time.deltaTime;
            //Once the time has ran out, set active to false again
            if (timerDisplay < 0)
            {
                dialogBox.SetActive(false);
            }
        }
    }

    //Function once function is called the timer is given 4 seconds, and dialog box is set active to true
    public void DisplayDialog()
    {
        timerDisplay = displayTime;
        dialogBox.SetActive(true);
    }


}

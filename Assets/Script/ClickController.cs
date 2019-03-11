using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickController : MonoBehaviour
{
    public static bool DoubleClicked = false;
    int waitcount = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(waitcount > 0)
            waitcount = waitcount - 1;
    }
    private void OnGUI()
    {
        Event e = Event.current;
        if (e.isMouse)
        {
            if(e.clickCount == 2)
            {
                DoubleClicked = true;
                waitcount = 10;
            }
            else if(waitcount == 0)
            {
                DoubleClicked = false;
            }
        }
    }
}

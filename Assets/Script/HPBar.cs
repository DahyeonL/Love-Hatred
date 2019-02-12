using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBar : MonoBehaviour
{
    void Start()
    {
        transform.GetComponent<Renderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 camPos = Camera.main.transform.position;
        camPos.x += transform.position.x - camPos.x;

        transform.LookAt(camPos);
    }

    public void visible()
    {
        transform.GetComponent<Renderer>().enabled = true;
    }

    public void invisible()
    {
        transform.GetComponent<Renderer>().enabled = false; ;
    }
}

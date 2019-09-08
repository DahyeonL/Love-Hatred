using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBar : Photon.MonoBehaviour
{

    float MaxXscale;

    void Start()
    {
        transform.GetComponent<Renderer>().enabled = false;
        MaxXscale = transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 camPos = Camera.main.transform.position;
        transform.LookAt(camPos);
        float xScale = MaxXscale * (float)transform.GetComponentInParent<Status>().GetHp() / (float)transform.GetComponentInParent<Status>().GetMaxHP();
        transform.localScale = new Vector3(xScale, transform.localScale.y, transform.localScale.z);
        
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

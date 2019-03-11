using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirMapController : MonoBehaviour
{
    public GameObject[] AirMap;
    // Start is called before the first frame update
    void Start()
    {
        foreach(GameObject obj in AirMap)
        {
            obj.GetComponent<Renderer>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

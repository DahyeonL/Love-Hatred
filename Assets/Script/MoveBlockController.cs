﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBlockController : MonoBehaviour
{
    public static List<GameObject> MoveBlock = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeMoveBlockStatus()
    {
        if(MoveBlockStatus.active == false)
        {
            foreach(GameObject moveblock in MoveBlock)
            {
                moveblock.transform.GetComponent<Renderer>().enabled = true;
                moveblock.transform.GetComponent<BoxCollider>().enabled = true;
            }
            MoveBlockStatus.active = true;
        }
        else
        {
            foreach (GameObject moveblock in MoveBlock)
            {
                moveblock.transform.GetComponent<Renderer>().enabled = false;
                moveblock.transform.GetComponent<BoxCollider>().enabled = false;
            }
            MoveBlockStatus.active = false;
        }
    }
}

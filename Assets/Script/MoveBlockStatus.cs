﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBlockStatus : MonoBehaviour
{
    public static bool active = false;

    void Start()
    {
        MoveBlockController.MoveBlock.Add(gameObject);
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
    
}

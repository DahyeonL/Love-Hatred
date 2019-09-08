using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStatus : Status
{
    private void Awake()
    {
        if (Condition == "Starting")
        {
            //transform.SetParent(PlaneManager.Map.transform, false);
        }
        else
        {
            transform.SetParent(PlaneManager.Map.transform, true);
        }
        
    }
    
    private void Update()
    {
        if (Healing && HP < MaxHP)
        {
            count = count + Time.deltaTime;
            if (count >= 0.5)
            {
                HP = HP + 1;
                count = 0;
            }
        }
    }

    private void OnDestroy()
    {
        Instantiate(DeathEffect, transform.position, Quaternion.identity);
    }
}

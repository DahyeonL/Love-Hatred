using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;
using UnityEngine.AI;

public class BuildingStatus : Status
{
    public bool builded = false;

    Collider[] colliders;

    private void Update()
    {
        if(photonView.isMine && !Visible)
            FindEnemy();
        else
        {
            if (Visible)
            {
                transform.GetComponent<ObjectVisualizer>().SetVisible();
            }
            else
            {
                transform.GetComponent<ObjectVisualizer>().SetInvisible();
            }
        }
    }

    void FindEnemy()
    {
        colliders = Physics.OverlapCapsule(transform.position - new Vector3(0, 10, 0), transform.position + new Vector3(0, 10, 0), 10);
        foreach (Collider col in colliders)
        {
            if ((col.gameObject.layer == 11 || col.gameObject.layer == 10) && !col.gameObject.GetPhotonView().isMine)
            {
                transform.GetComponent<Status>().SetVisible(true);
                return;
            }
        }
        transform.GetComponent<Status>().SetVisible(false);
    }
}

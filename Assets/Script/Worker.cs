using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worker : Photon.MonoBehaviour
{
    public bool CanBuild = true;

    private void Awake()
    {
        //transform.SetParent(PlaneManager.Map.transform, false);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (photonView.isMine)
        {
            BuildManager.Workers.Add(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(photonView.isMine && CanBuild == false && transform.GetComponent<Status>().GetCondition() != "Building")
        {
            CanBuild = true;
        }
    }
}

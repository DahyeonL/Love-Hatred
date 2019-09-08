using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(GetComponentInParent<PhotonView>().isMine)
            transform.GetComponent<MeshRenderer>().material.color = PlayerController.color[PlayerController.GetPlayerNum() - 1];
        else
        {
            transform.GetComponent<MeshRenderer>().material.color = PlayerController.color[PlayerController.GetPlayerNum() % 2];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

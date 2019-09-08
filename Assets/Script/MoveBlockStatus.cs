using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBlockStatus : MonoBehaviour
{
    public static bool active = false;

    Vector3 startPos;
    float x;
    float z;
    bool move_x = false;
    bool move_z = false;

    void Start()
    {
        startPos = transform.position;
        if(startPos.x < startPos.z)
        {
            move_x = true;
            z = 0;
            if(startPos.x < 0)
            {
                x = 1;
            }
            else
            {
                x = -1;
            }
        }
        else
        {
            move_z = true;
            x = 0;
            if(startPos.z < 0)
            {
                z = 1;
            }
            else
            {
                z = -1;
            }
        }
        MoveBlockController.MoveBlock.Add(gameObject);
        transform.GetComponent<Renderer>().enabled = false;
        transform.GetComponent<BoxCollider>().enabled = false;
        //gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (move_x) {
            transform.position = transform.position + new Vector3(x * Time.deltaTime * 0.2f, 0, 0);
            if((transform.position.x * transform.position.x) > (startPos.x * startPos.x))
            {
                move_x = false;
                move_z = true;
                x = 0;
                if(transform.position.z < 0)
                {
                    z = 1;
                }
                else
                {
                    z = -1;
                }
            }
        }
        else
        {
            transform.position = transform.position + new Vector3(0, 0, z * Time.deltaTime * 0.2f);
            if ((transform.position.z * transform.position.z) > (startPos.z * startPos.z))
            {
                move_x = true;
                move_z = false;
                z = 0;
                if (transform.position.x < 0)
                {
                    x = 1;
                }
                else
                {
                    x = -1;
                }
            }
        }
    }
    
}

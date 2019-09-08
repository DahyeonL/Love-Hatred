using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkBuilding : MonoBehaviour
{
    bool send = true;
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            if (transform.GetComponent<BuildingStatus>() != null)
            {
                stream.SendNext(transform.GetComponent<BuildingStatus>().builded);
            }
            else if(send)
            {
                stream.SendNext(true);
                send = false;
            }
            stream.SendNext(transform.GetComponent<BuildingStatus>().GetHp());
            stream.SendNext(transform.GetComponent<Status>().GetVisible());
        }
        else
        {
            if (transform.GetComponent<BuildingStatus>() != null)
            {
                transform.GetComponent<BuildingStatus>().builded = (bool)stream.ReceiveNext();
            }
            transform.GetComponent<BuildingStatus>().SetHP((float)stream.ReceiveNext());
            transform.GetComponent<Status>().SetVisible((bool)stream.ReceiveNext());
        }
    }
}

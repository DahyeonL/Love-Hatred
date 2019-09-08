using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NetworkUnit : Photon.MonoBehaviour
{
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(transform.GetComponent<Moving>().WaitCount);
            stream.SendNext(transform.GetComponent<Status>().GetCondition());
            stream.SendNext(transform.GetComponent<Status>().GetHp());
            stream.SendNext(transform.GetComponent<Status>().GetVisible());
            if (transform.GetComponent<Status>().GetCondition() == "Attacking")
                stream.SendNext(transform.GetComponent<Attacking>().Enemy.GetComponent<PhotonView>().viewID);
            else
                stream.SendNext(0);
            stream.SendNext(transform.GetComponent<Attacking>().Bonus);
        }
        else
        {
            transform.GetComponent<Moving>().WaitCount = (int)stream.ReceiveNext();
            transform.GetComponent<Status>().SetCondition((string)stream.ReceiveNext());
            transform.GetComponent<Status>().SetHP((float)stream.ReceiveNext());
            transform.GetComponent<Status>().SetVisible((bool)stream.ReceiveNext());
            transform.GetComponent<Attacking>().EnemyID = (int)stream.ReceiveNext();
            transform.GetComponent<Attacking>().Bonus = (int)stream.ReceiveNext();
        }
    }
}

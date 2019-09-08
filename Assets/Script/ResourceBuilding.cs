using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceBuilding : Photon.MonoBehaviour
{
    [SerializeField]
    GameObject Fire_Worker;
    [SerializeField]
    GameObject Water_Worker;

   

    public bool Creating = false;
    float CreatingTime;
    GameObject CreatingObject;

    private void Update()
    {
        if(photonView.isMine && transform.GetComponent<BuildingStatus>().builded && StartingPoint.IsStart)
            PlayerController.AddResource(4 * (PlayerController.GetSanctomCount() + 1) * Time.deltaTime);
        if (Creating)
        {
            CreatingTime = CreatingTime - Time.deltaTime;
            if (CreatingTime <= 0)
            {
                GameObject obj = PhotonNetwork.Instantiate(CreatingObject.name, transform.position, Quaternion.identity, 0);
                obj.GetComponent<Status>().SetCondition("Create");
                Creating = false;
            }
        }
    }

    public void CreateFireWorker()
    {
        if (PlayerController.GetResource() < Fire_Worker.GetComponent<Status>().GetCost())
        {
            PlayerController.ShowWarning("자원이 부족합니다.", 3);
            return;
        }
        Creating = true;
        CreatingTime = Fire_Worker.GetComponent<Status>().GetTime();
        CreatingObject = Fire_Worker;
        PlayerController.RemoveResource(Fire_Worker.GetComponent<Status>().GetCost());
    }

    public void CreateWaterWorker()
    {
        if (PlayerController.GetResource() < Water_Worker.GetComponent<Status>().GetCost())
        {
            PlayerController.ShowWarning("자원이 부족합니다.", 3);
            return;
        }
        Creating = true;
        CreatingTime = Water_Worker.GetComponent<Status>().GetTime();
        CreatingObject = Water_Worker;
        PlayerController.RemoveResource(Water_Worker.GetComponent<Status>().GetCost());
    }
}

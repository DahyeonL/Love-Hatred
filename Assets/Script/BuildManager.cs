using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : Photon.MonoBehaviour
{
    [SerializeField]
    private GameObject FireResourceBuilding;
    [SerializeField]
    private GameObject FireUnitBuilding;
    [SerializeField]
    private GameObject FireTeraformingBuilding;
    [SerializeField]
    private GameObject WaterResourceBuilding;
    [SerializeField]
    private GameObject WaterUnitBuilding;
    [SerializeField]
    private GameObject WaterTeraformingBuilding;

    public static List<GameObject> Workers = new List<GameObject>();
    GameObject Building;
    public GameObject BuildingPrefab;
    
    public void Restart()
    {
        Workers = new List<GameObject>();
    }

    public void BuildResourceBuilding()
    {
        if (PlayerController.GetTribe() == "Fire")
        {
            BuildingPrefab = FireResourceBuilding;
        }
        else if(PlayerController.GetTribe() == "Water")
        {
            BuildingPrefab = WaterResourceBuilding;
        }
        MakeBuilding();
    }

    public void BuildUnitBuilding()
    {
        if (PlayerController.GetTribe() == "Fire")
        {
            BuildingPrefab = FireUnitBuilding;
        }
        else if (PlayerController.GetTribe() == "Water")
        {
            BuildingPrefab = WaterUnitBuilding;
        }
        MakeBuilding();
    }

    public void BuildTeraformingBuilding()
    {
        if (PlayerController.GetTribe() == "Fire")
        {
            BuildingPrefab = FireTeraformingBuilding;
        }
        else if (PlayerController.GetTribe() == "Water")
        {
            BuildingPrefab = WaterTeraformingBuilding;
        }
        MakeBuilding();
    }

    public void MakeBuilding()
    {
        OnBuildMode();
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit[] rayInfos;
        RaycastHit plane = new RaycastHit();
        int mask = 1 << LayerMask.NameToLayer("Map");
        rayInfos = Physics.RaycastAll(ray, Mathf.Infinity , mask);
        if(rayInfos.Length == 1)
        {
            plane = rayInfos[0];
        }
        else if(rayInfos.Length == 2)
        {
            foreach(RaycastHit rayinfo in rayInfos)
            {
                if(rayinfo.transform.tag == "2Floor" || rayinfo.transform.tag == "Slope")
                {
                    plane = rayinfo;
                    break;
                }
            }
        }
        else if(rayInfos.Length == 3)
        {
            foreach (RaycastHit rayinfo in rayInfos)
            {
                if (rayinfo.transform.tag == "3Floor" || rayinfo.transform.tag == "Slope")
                {
                    plane = rayinfo;
                    break;
                }
            }
        }
        Building = PhotonNetwork.Instantiate(BuildingPrefab.name, plane.point, Quaternion.identity, 0);
        if (PlayerController.GetResource() < Building.GetComponent<BuildingStatus>().GetCost() * 10)
        {
            Debug.Log("자원이 부족합니다.");
            PhotonNetwork.Destroy(Building);
            return;
        }
        PlayerController.RemoveResource(Building.GetComponent<BuildingStatus>().GetCost() * 10);
    }

    public void ConfirmBuild()
    {
        Building.GetComponent<BuildingController>().Build();
        SetDragTrue();
        foreach (GameObject obj in PlayerController.TerraBuildings)
        {
            obj.GetComponent<TerraformingArea>().OffTerra();
        }
    }

    public void CancelBuild()
    {
        Building.GetComponent<BuildingController>().Cancel();
        SetDragTrue();
        foreach (GameObject obj in PlayerController.TerraBuildings)
        {
            obj.GetComponent<TerraformingArea>().OffTerra();
        }
    }

    public void OnBuildMode()
    {
        foreach(GameObject obj in SelectUnit.SelectedUnit)
        {
            obj.GetComponent<Selecting>().selected = false;
        }
        foreach(GameObject obj in SelectUnit.SelectedBuilding)
        {
            obj.GetComponent<Selecting>().selected = false;
        }
        foreach (GameObject obj in PlayerController.TerraBuildings)
        {
            obj.GetComponent<TerraformingArea>().OnTerra();
        }
        SetDragFalse();
    }
    public void SetDragFalse()
    {
        transform.GetComponent<SelectUnit>().enabled = false;
    }

    public void SetDragTrue()
    {
        transform.GetComponent<SelectUnit>().enabled = true;
    }
}

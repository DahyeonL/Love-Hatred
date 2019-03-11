using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    GameObject Map;
    public GameObject MapPrefab;

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
        Map = Instantiate(MapPrefab, plane.point, new Quaternion(0, 0, 0, 0));
    }

    public void ConfirmBuild()
    {
        Map.GetComponent<BuildingStatus>().Build();
        SetDragTrue();
    }

    public void CancelBuild()
    {
        Map.GetComponent<BuildingStatus>().Cancel();
        SetDragTrue();
    }

    public void OnBuildMode()
    {
        foreach(GameObject obj in SelectUnit.Selected)
        {
            obj.GetComponent<Selecting>().selected = false;
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

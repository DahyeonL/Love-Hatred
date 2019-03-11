using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;

public class BuildingStatus : MonoBehaviour
{
    [SerializeField]
    private GameObject CheckCube;

    private bool Isbuild = false;

    // Start is called before the first frame update
    void Start()
    {
        CheckCube.GetComponent<Renderer>().material.color = new Color(0, 1, 0, 0.25f);
    }

    // Update is called once per frame
    void Update()
    {
        Dragging();
    }

    public void BuildCheck()
    {

    }

    public void Dragging()
    {   if (Input.GetMouseButton(0) && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] rayInfos;
            RaycastHit plane = new RaycastHit();
            int mask = 1 << LayerMask.NameToLayer("Map");
            rayInfos = Physics.RaycastAll(ray, Mathf.Infinity, mask);
            if (rayInfos.Length == 1)
            {
                plane = rayInfos[0];
            }
            else if (rayInfos.Length == 2)
            {
                foreach (RaycastHit rayinfo in rayInfos)
                {
                    if (rayinfo.transform.tag == "2Floor" || rayinfo.transform.tag == "Slope")
                    {
                        plane = rayinfo;
                        break;
                    }
                }
            }
            else if (rayInfos.Length == 3)
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
            transform.position = plane.point;
        }
    }

    public void Build()
    {
        Destroy(CheckCube);
        Isbuild = true;
        transform.GetComponent<BuildingStatus>().enabled = false;
    }

    public void Cancel()
    {
        if(Isbuild == false)
        {
            Destroy(gameObject);
        }
    }


    public void OnTriggerStay(Collider other)
    {
        CheckCube.GetComponent<Renderer>().material.color = new Color(1, 0, 0, 0.25f);
    }

    public void OnTriggerExit(Collider other)
    {
        CheckCube.GetComponent<Renderer>().material.color = new Color(0, 1, 0, 0.25f);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;
using UnityEngine.AI;

public class BuildingController : Photon.MonoBehaviour
{
    [SerializeField]
    private GameObject CheckCube;
    [SerializeField]
    private GameObject BuildingEffect;

    public bool ColliderCheck = false;
    public bool FloorCheck = false;
    public bool LandCheck = false;
    private bool Isbuild = false;
    private GameObject Worker;
    private NavMeshObstacle nvo;
    public string Condition;

    public bool Creating = false;
    float CreatingTime;

    private void Awake()
    {
        if (Condition == "Starting")
        {
            transform.SetParent(PlaneManager.Map.transform, false);
        }
        else
        {
            transform.SetParent(PlaneManager.Map.transform, true);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        nvo = GetComponent<NavMeshObstacle>();
        nvo.enabled = false;
        CheckCube.GetComponent<Renderer>().material.color = new Color(0, 1, 0, 0.25f);
        if (Condition == "Starting")
        {
            transform.GetComponent<BuildingStatus>().builded = true;
            transform.GetComponent<NavMeshObstacle>().enabled = true;
            Destroy(CheckCube);
            transform.GetComponent<BoxCollider>().enabled = true;
            Destroy(transform.GetComponent<BuildingController>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.isMine)
        {
            if (!Isbuild)
            {
                Dragging();

            }
            else
            {
                if (Creating)
                {
                    CreatingTime = CreatingTime - Time.deltaTime;
                    transform.GetComponent<Status>().SetHP(transform.GetComponent<Status>().GetHp() + transform.GetComponent<Status>().GetMaxHP() * (Time.deltaTime / transform.GetComponent<Status>().GetTime()));
                    if (CreatingTime <= 0)
                    {
                        transform.GetComponent<BuildingStatus>().builded = true;
                        transform.GetComponent<ObjectVisualizer>().SetVisible();
                        BuildingEffect.SetActive(false);
                        Destroy(transform.GetComponent<BuildingController>());

                        //effect 숨기고 건물 보이기
                    }
                }
                else
                {
                    WorkerCheck();
                }
            }
            if (!Creating)
            {
                Collidercheck();
                Floorcheck();
                Landcheck();
                check();
            }
        }
        if (!Creating && photonView.isMine && Isbuild == true && (Worker == null || Worker.GetComponent<Status>().GetCondition() != "Building"))
        {
            PhotonNetwork.Destroy(gameObject);
        }
        else if (!photonView.isMine)
        {
            Destroy(CheckCube);
            if (transform.GetComponent<BuildingStatus>().builded)
            {
                
                transform.GetComponent<BoxCollider>().enabled = true;
                transform.GetComponent<NavMeshObstacle>().enabled = true;
                Destroy(transform.GetComponent<BuildingController>());
            }
        }
    }

    public void check()
    {
        if (FloorCheck && ColliderCheck && LandCheck)
            CheckCube.GetComponent<Renderer>().material.color = new Color(0, 1, 0, 0.25f);
        else
            CheckCube.GetComponent<Renderer>().material.color = new Color(1, 0, 0, 0.25f);
    }
    
    public void Landcheck()
    {
        Collider[] colliders = Physics.OverlapCapsule(transform.position + new Vector3(0, 10, 0), transform.position - new Vector3(0, 10, 0), 10f);
        foreach (Collider col in colliders)
        {
            if((col.gameObject.tag == "TerraformingBuilding" || col.gameObject.tag == "ResourceBuilding") && col.GetComponent<PhotonView>().isMine && col.transform.GetComponent<BuildingStatus>().builded)
            {
                LandCheck = true;
                return;
            }
        }
        LandCheck = false;
    }

    public void Floorcheck()
    {
        Collider[] colliders1 = Physics.OverlapBox(transform.position - new Vector3(CheckCube.GetComponent<Renderer>().bounds.size.x / 2f, 0.2f, 0), new Vector3(0.05f, 0.05f, 0.05f));
        Collider[] colliders2 = Physics.OverlapBox(transform.position - new Vector3(-CheckCube.GetComponent<Renderer>().bounds.size.x / 2f, 0.2f, 0), new Vector3(0.05f, 0.05f, 0.05f));
        Collider[] colliders3 = Physics.OverlapBox(transform.position - new Vector3(0, 0.2f, CheckCube.GetComponent<Renderer>().bounds.size.z / 2f), new Vector3(0.05f, 0.05f, 0.05f));
        Collider[] colliders4 = Physics.OverlapBox(transform.position - new Vector3(0, 0.2f, -CheckCube.GetComponent<Renderer>().bounds.size.z / 2f), new Vector3(0.05f, 0.05f, 0.05f));
        if(colliders1.Length > 0 && colliders2.Length > 0 && colliders3.Length > 0 && colliders4.Length > 0 && colliders1[0] == colliders2[0] == colliders3[0] == colliders4[0])
        {
            FloorCheck = true;
        }
        else
        {
            FloorCheck = false;
        }
    }

    public void Collidercheck()
    {
        Collider[] colliders = Physics.OverlapBox(transform.position + new Vector3(0, 0.5f, 0), new Vector3(CheckCube.GetComponent<Renderer>().bounds.size.x / 2f, 0.05f, CheckCube.GetComponent<Renderer>().bounds.size.z / 2f));
        foreach(Collider col in colliders)
        {
            if(col.gameObject != Worker)
            {
                ColliderCheck = false;
                return;
            }
        }
        ColliderCheck = true;
    }

    public void Dragging()
    {
        if (Input.GetMouseButton(0) && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(-1))
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
        if (ColliderCheck && FloorCheck && LandCheck)
        {
            if (PlayerController.GetResource() < transform.GetComponent<Status>().GetCost())
            {
                PlayerController.ShowWarning("자원이 부족합니다.", 3);
                PhotonNetwork.Destroy(gameObject);
                return;
            }
            float distance = 0;
            Isbuild = true;
            foreach (GameObject worker in BuildManager.Workers)
            {
                if (distance == 0 && worker.GetComponent<Worker>().CanBuild)
                {
                    distance = Vector3.Distance(worker.transform.position, transform.position);
                    Worker = worker;
                }
                else
                {
                    if (distance > Vector3.Distance(worker.transform.position, transform.position) && worker.GetComponent<Worker>().CanBuild)
                    {
                        distance = Vector3.Distance(worker.transform.position, transform.position);
                        Worker = worker;
                    }
                }
            }
            if (Worker != null)
            {
                Worker.GetComponent<Status>().SetCondition("Building");
                Worker.GetComponent<Worker>().CanBuild = false;
                Worker.GetComponent<Moving>().nv.isStopped = false;
                Worker.GetComponent<Moving>().nv.SetDestination(transform.position);
            }
            else
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
        else
        {
            PlayerController.ShowWarning("건물을 지을 수 없습니다.", 3);
            PhotonNetwork.Destroy(gameObject);
        }
    }

    public void Cancel()
    {
        if (Isbuild == false)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }

    public void WorkerCheck()
    {
        if(Vector3.Distance(transform.position, Worker.transform.position) < 1.0f)
        {
            if (ColliderCheck && FloorCheck && LandCheck)
            {
                Creating = true;
                CreatingTime = transform.GetComponent<Status>().GetTime();
                if (PlayerController.GetResource() < transform.GetComponent<Status>().GetCost())
                {
                    PlayerController.ShowWarning("자원이 부족합니다.", 3);
                    PhotonNetwork.Destroy(gameObject);
                    return;
                }
                PlayerController.RemoveResource(transform.GetComponent<Status>().GetCost());

                transform.GetComponent<ObjectVisualizer>().SetInvisible();
                BuildingEffect.SetActive(true);
                //건물 숨기고 effect 보이기
                

                transform.GetComponent<BoxCollider>().enabled = true;
                //transform.GetComponent<BuildingStatus>().builded = true; 
                ScoreController.MyBuildings = ScoreController.MyBuildings + 1;
                Destroy(CheckCube);
                BuildManager.Workers.Remove(Worker);
                SelectUnit.SelectedUnit.Remove(Worker);
                PhotonNetwork.Destroy(Worker);
                nvo.enabled = true;
                transform.GetComponent<Status>().SetHP(1);
                //Destroy(transform.GetComponent<BuildingController>());
            }
            else
            {
                PlayerController.ShowWarning("건물을 지을 수 없습니다.", 3);
                Worker.GetComponent<Worker>().CanBuild = true;
                Worker.GetComponent<Status>().SetCondition("Stopping");
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }
}

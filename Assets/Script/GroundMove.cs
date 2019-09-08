using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GroundMove : Moving
{
    private void Awake()
    {
        nv = GetComponent<NavMeshAgent>();
        nvo = GetComponent<NavMeshObstacle>();
        if (photonView.isMine)
        {
            nvo.enabled = false;
            nv.enabled = true;

            transform.GetComponent<Status>().SetCondition("Create");
        }
        else
        {
            nv.enabled = false;
            nvo.enabled = true;
        }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Condition = transform.GetComponent<Status>().GetCondition();
        if (photonView.isMine)
        {
            Moving();
        }
        else
        {
            if (transform.GetComponent<Status>().GetVisible())
            {
                transform.GetComponent<ObjectVisualizer>().SetVisible();
            }
            else
            {
                transform.GetComponent<ObjectVisualizer>().SetInvisible();
            }
        }
    }

    private void SetCondition(string Con)
    {
        Condition = Con;
        transform.GetComponent<Status>().SetCondition(Con);
    }

    private void Moving()
    {
        if (ARMoving)
        {
            if (ARMoveDest != null && ARMoveDest.GetComponent<FloorStatus>().getARMove() == false)
            {
                ARMoving = false;
                ARMoveDest = null;
                SetCondition("Stopping");
                return;
            }
            if (ARMoveStart.GetComponent<FloorStatus>().getConnectedEdgeString() == "Top" || ARMoveStart.GetComponent<FloorStatus>().getConnectedEdgeString() == "Bottom")
            {
                if (Mathf.Abs(transform.position.z - ARMoveStart.GetComponent<FloorStatus>().getConnectedEdgeVector().z) < 0.8)
                {
                    transform.position = ARMoveDest.GetComponent<FloorStatus>().getConnectedEdgeVector() + (ARMoveDest.transform.position - ARMoveDest.GetComponent<FloorStatus>().getConnectedEdgeVector()) / 10;
                    SetCondition("ARMoving");
                    ARMoving = false;
                }
            }
            else if (ARMoveStart.GetComponent<FloorStatus>().getConnectedEdgeString() == "Right" || ARMoveStart.GetComponent<FloorStatus>().getConnectedEdgeString() == "Left")
            {
                if (Mathf.Abs(transform.position.x - ARMoveStart.GetComponent<FloorStatus>().getConnectedEdgeVector().x) < 0.8)
                {
                    transform.position = ARMoveDest.GetComponent<FloorStatus>().getConnectedEdgeVector() + (ARMoveDest.transform.position - ARMoveDest.GetComponent<FloorStatus>().getConnectedEdgeVector()) / 10;
                    SetCondition("ARMoving");
                    ARMoving = false;
                }
            }
        }
        if (Condition == "Moving" || Condition == "Tracing" || Condition == "OnlyMoving" || Condition == "Building")
        {
            if (transform.GetComponent<Attacking>().Enemy != null && Condition != "Tracing")
            {
                transform.GetComponent<Attacking>().Enemy.GetComponent<Status>().AttackMe.Remove(gameObject);
                transform.GetComponent<Attacking>().Enemy = null;
            }
        }
        if (Condition == "ARMoving")
        {
            nv.enabled = false;
            SetCondition("Moving");
            Destination = ARDestination;
            nv.enabled = true;
            nv.isStopped = false;
            nv.SetDestination(Destination);
        }
        else if (Condition == "Create")
        {
            SetCondition("Stopping");
        }
        else if (Condition == "Stopping" || Condition == "Attacking")
        {
            nv.ResetPath();
        }

        if (Input.GetMouseButtonUp(0) && transform.GetComponent<Selecting>().selected == true && SelectUnit.isDragged == false && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(-1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            Ray ray2 = new Ray();
            ray2.origin = transform.position;
            ray2.direction = transform.position + new Vector3(0, -10, 0);
            RaycastHit[] hitInfos;
            RaycastHit hitInfo2 = new RaycastHit();
            if (Physics.Raycast(ray, out hitInfo))
            {
                int mask;
                mask = 1 << LayerMask.NameToLayer("Map");
                hitInfos = Physics.RaycastAll(ray2, 200f, mask);
                /*Debug.Log(hitInfos.Length);
                foreach(RaycastHit a in hitInfos)
                {
                    Debug.Log(a.collider.name);
                }*/
                /*foreach(RaycastHit hit in hitInfos)
                {
                    if (hitInfo2.collider == null)
                    {
                        hitInfo2 = hit;
                    }
                    else
                    {
                        if (hit.collider.tag == "Plane"
                    }
                }*/
                if (hitInfos.Length == 1)
                {
                    hitInfo2 = hitInfos[0];
                }
                else if (hitInfos.Length == 2)
                {
                    for (int i = 0; i < hitInfos.Length; i++)
                    {
                        if (hitInfos[i].transform.tag == "2Floor" || hitInfos[i].transform.tag == "Slope")
                        {
                            hitInfo2 = hitInfos[i];
                            break;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < hitInfos.Length; i++)
                    {
                        if (hitInfos[i].transform.tag == "3Floor" || hitInfos[i].transform.tag == "2Floor")
                        {
                            hitInfo2 = hitInfos[i];
                            break;
                        }
                    }
                }
                int l = hitInfo.transform.gameObject.layer;
                if (l == MapLayer)
                {
                    Destination = hitInfo.point;
                    if ((hitInfo2.transform.tag == "2Floor" || hitInfo2.transform.tag == "3Floor") && hitInfo2.transform.GetComponent<FloorStatus>().getARMove() == true && (hitInfo.transform.tag == "2Floor" || hitInfo.transform.tag == "3Floor") && hitInfo.transform.GetComponent<FloorStatus>().getARMove() == true)
                    {
                        if (hitInfo.transform.gameObject != hitInfo2.transform.gameObject)
                        {
                            ARMoveStart = hitInfo2.transform.gameObject;
                            ARMoveDest = hitInfo.transform.gameObject;
                            ARMoving = true;
                            ARDestination = Destination;
                            Destination = hitInfo2.transform.GetComponent<FloorStatus>().getConnectedEdgeVector();
                            nv.isStopped = false;
                            nv.SetDestination(Destination);
                        }
                        else
                        {
                            ARMoveDest = null;
                            ARMoving = false;
                        }
                    }
                    else
                    {
                        ARMoveDest = null;
                        ARMoving = false;
                    }
                    if (ClickController.DoubleClicked == false)
                    {
                        SetCondition("Moving");
                        nv.isStopped = false;
                        nv.SetDestination(Destination);
                    }
                    else
                    {
                        SetCondition("OnlyMoving");
                        nv.isStopped = false;
                        nv.SetDestination(Destination);
                    }
                }
            }
        }
    }
}
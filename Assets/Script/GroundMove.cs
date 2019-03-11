using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GroundMove : Moving
{
    // Start is called before the first frame update
    void Start()
    {
        nv = GetComponent<NavMeshAgent>();
        nv.angularSpeed = 1000f;
        nvo = GetComponent<NavMeshObstacle>();
        nv.enabled = false;
        nvo.enabled = true;
        final = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Condition = transform.GetComponent<Status>().GetCondition();
        Moving();
    }

    private void SetCondition(string Con)
    {
        Condition = Con;
        transform.GetComponent<Status>().SetCondition(Con);
    }
    public void Move(Vector3 Dest)
    {
        if (ARMoving == true)
        {
            nv.SetDestination(Dest);
            nv.stoppingDistance = 0;
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
                    final = ARMoveDest.GetComponent<FloorStatus>().getConnectedEdgeVector() + (ARMoveDest.transform.position - ARMoveDest.GetComponent<FloorStatus>().getConnectedEdgeVector()) / 10;
                    SetCondition("ARMoving");
                    ARMoving = false;
                }
            }
            else if (ARMoveStart.GetComponent<FloorStatus>().getConnectedEdgeString() == "Right" || ARMoveStart.GetComponent<FloorStatus>().getConnectedEdgeString() == "Left")
            {
                if (Mathf.Abs(transform.position.x - ARMoveStart.GetComponent<FloorStatus>().getConnectedEdgeVector().x) < 0.8)
                {
                    transform.position = ARMoveDest.GetComponent<FloorStatus>().getConnectedEdgeVector() + (ARMoveDest.transform.position - ARMoveDest.GetComponent<FloorStatus>().getConnectedEdgeVector()) / 10;
                    final = ARMoveDest.GetComponent<FloorStatus>().getConnectedEdgeVector() + (ARMoveDest.transform.position - ARMoveDest.GetComponent<FloorStatus>().getConnectedEdgeVector()) / 10;
                    SetCondition("ARMoving");
                    ARMoving = false;
                }
            }
        }
        if (WaitCount < 3)
        {
            nv.stoppingDistance = 0;
            transform.position = final;
            WaitCount = WaitCount + 1;
            MyDset = Destination;
        }
        else if (Destination == Dest)
        {
            nv.SetDestination(Dest);
        }
        if (Mathf.Abs(predistance - nv.remainingDistance) < nv.speed * 0.005)
        {
            if (Condition != "Tracing")
            {
                if (nv.stoppingDistance > 1)
                {
                    predirection = FindingPath(Destination, MyDset, predirection);
                }
                nv.stoppingDistance += 0.1f;
            }
        }
        predistance = nv.remainingDistance;
        final = transform.position;
    }

    private void Moving()
    {
        if (nv.enabled == true && Vector3.Distance(transform.position, Destination) <= nv.stoppingDistance && (Condition == "Moving" || Condition == "OnlyMoving"))
        {
            SetCondition("Stopping");
        }
        else if (Condition == "Moving" || Condition == "Tracing" || Condition == "OnlyMoving")
        {
            Move(Destination);
            if (transform.GetComponent<Attacking>().Enemy != null && Condition != "Tracing")
            {
                transform.GetComponent<Attacking>().Enemy.GetComponent<Status>().AttackMe.Remove(gameObject);
                transform.GetComponent<Attacking>().Enemy = null;
            }
        }
        else if (Condition == "Stopping" || Condition == "Attacking")
        {
            WaitCount = 0;
            transform.position = final;
            nv.enabled = false;
            nvo.enabled = true;
        }
        else if (Condition == "ARMoving")
        {
            transform.position = final;
            nv.enabled = false;
            nvo.enabled = true;
            SetCondition("Moving");
            nvo.enabled = false;
            nv.enabled = true;
            Destination = ARDestination;
            nv.SetDestination(Destination);
        }

        if (Input.GetMouseButtonUp(0) && transform.GetComponent<Selecting>().selected == true && SelectUnit.isDragged == false)
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
                hitInfos = Physics.RaycastAll(ray2, 100f, mask);
                foreach(RaycastHit col in hitInfos)
                {
                    Debug.Log(col.transform.name);
                }
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
                        if (hitInfos[i].transform.tag == "3Floor" || hitInfos[i].transform.tag == "Slope")
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
                    nvo.enabled = false;
                    nv.enabled = true;
                    nv.stoppingDistance = 0;
                    if ((hitInfo2.transform.tag == "2Floor" || hitInfo2.transform.tag == "3Floor") && hitInfo2.transform.GetComponent<FloorStatus>().getARMove() == true && (hitInfo.transform.tag == "2Floor" || hitInfo.transform.tag == "3Floor") && hitInfo.transform.GetComponent<FloorStatus>().getARMove() == true)
                    {
                        if (hitInfo.transform.gameObject != hitInfo2.transform.gameObject)
                        {
                            ARMoveStart = hitInfo2.transform.gameObject;
                            ARMoveDest = hitInfo.transform.gameObject;
                            ARMoving = true;
                            ARDestination = Destination;
                            Destination = hitInfo2.transform.GetComponent<FloorStatus>().getConnectedEdgeVector();
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
                        Move(Destination);
                    }
                    else
                    {
                        SetCondition("OnlyMoving");
                        Move(Destination);
                    }
                }
            }
        }
    }
    
    private int FindingPath(Vector3 Dest, Vector3 MyDest, int num)
    {
        int RandomNum;
        Vector3 newDset = MyDest;
        if (Destination == Dest)
        {
            RandomNum = Random.Range(1, 5);
            if (RandomNum == 1)
            {
                newDset.x += 0.1f;
            }
            else if (RandomNum == 2)
            {
                newDset.z += 0.1f;
            }
            else if (RandomNum == 3)
            {
                newDset.x -= 0.1f;
            }
            else if (RandomNum == 4)
            {
                newDset.z -= 0.1f;
            }
            nv.SetDestination(newDset);
            this.MyDset = newDset;
        }
        else
        {
            RandomNum = Random.Range(1, 5);
            while (!(RandomNum != num && RandomNum % 2 == num % 2))
            {
                RandomNum = Random.Range(1, 5);
            }
            if (RandomNum == 1)
            {
                newDset.x += 0.1f;
            }
            else if (RandomNum == 2)
            {
                newDset.z += 0.1f;
            }
            else if (RandomNum == 3)
            {
                newDset.x -= 0.1f;
            }
            else if (RandomNum == 4)
            {
                newDset.z -= 0.1f;
            }
            nv.SetDestination(newDset);
            this.MyDset = newDset;
        }
        return RandomNum;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AirMove : Moving
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

        if (Input.GetMouseButtonUp(0) && transform.GetComponent<Selecting>().selected == true && SelectUnit.isDragged == false)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo))
            {
                int l = hitInfo.transform.gameObject.layer;
                if (l == MapLayer)
                {
                    Destination = hitInfo.point;
                    nvo.enabled = false;
                    nv.enabled = true;
                    nv.stoppingDistance = 0;
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
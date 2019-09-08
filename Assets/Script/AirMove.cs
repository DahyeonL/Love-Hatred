using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AirMove : Moving
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
        if (Condition == "Moving" || Condition == "Tracing" || Condition == "OnlyMoving")
        {
            if (transform.GetComponent<Attacking>().Enemy != null && Condition != "Tracing")
            {
                transform.GetComponent<Attacking>().Enemy.GetComponent<Status>().AttackMe.Remove(gameObject);
                transform.GetComponent<Attacking>().Enemy = null;
            }
        }
        else if (Condition == "Stopping" || Condition == "Attacking")
        {
            nv.isStopped = true;
        }
        else if (Condition == "Create")
        {
            SetCondition("Stopping");
        }

        if (Input.GetMouseButtonUp(0) && transform.GetComponent<Selecting>().selected == true && SelectUnit.isDragged == false && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(-1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo))
            {
                int l = hitInfo.transform.gameObject.layer;
                if (l == MapLayer)
                {
                    Destination = hitInfo.point;
                    nv.stoppingDistance = 0;
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
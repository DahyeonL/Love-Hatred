using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public class GroundUnit : MonoBehaviour
{
    public string condition;

    //Moving
    private NavMeshAgent nv;
    private NavMeshObstacle nvo;
    private float predistance = 0;
    private Vector3 MyDset;
    int WaitCount = 0;
    int predirection;
    private bool selected = false;
    private Vector3 Destination;
    private Vector3 ClickPosition;
    private int MapLayer = 0;
    private Vector3 final;

    //Selecting
    private bool contain = false;

    //Attack
    private Collider[] colliders;
    public Collider Enemy;


    private void Start()
    {
        nv = GetComponent<NavMeshAgent>();
        nvo = GetComponent<NavMeshObstacle>();
        nv.enabled = false;
        nvo.enabled = true;
        final = transform.position;
    }

    private void Update()
    {
        Attacking();
        Moving();
        Selecting();
    }

    private void Move(Vector3 Dest)
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
            if (condition != "Tracing")
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
        if (nv.enabled == true && Vector3.Distance(transform.position, Destination) <= nv.stoppingDistance && (condition == "Moving" || condition == "OnlyMoving"))
        {
            condition = "Stopping";
        }
        else if (condition == "Moving" || condition == "Tracing" || condition == "OnlyMoving")
        {
            Move(Destination);
            if(Enemy != null && condition != "Tracing")
            {
                Enemy.GetComponent<Status>().AttackMe.Remove(gameObject);
                Enemy = null;
            }
        }
        else if (condition == "Stopping" || condition == "Attacking")
        {
            WaitCount = 0;
            transform.position = final;
            nv.enabled = false;
            nvo.enabled = true;
        }

        if (Input.GetMouseButtonUp(0) && selected == true && SelectUnit.isDragged == false)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo, 100f))
            {
                int l = hitInfo.transform.gameObject.layer;
                if (l == MapLayer)
                {
                    ClickPosition = hitInfo.point;
                    Destination = ClickPosition;
                    nvo.enabled = false;
                    nv.enabled = true;
                    if (ClickController.DoubleClicked == false)
                    {
                        condition = "Moving";
                        Move(Destination);
                    }
                    else
                    {
                        condition = "OnlyMoving";
                        Move(Destination);
                    }
                }
            }
        }
    }

    private void Selecting()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 camPos = Camera.main.WorldToScreenPoint(transform.position);
            camPos.y = SelectUnit.InvertMouseY(camPos.y);
            contain = SelectUnit.selection.Contains(camPos, true);
        }
        if (contain)
        {
            selected = true;
            transform.GetComponentInChildren<HPBar>().visible();
        }
        else if (SelectUnit.isDragged == true)
        {
            selected = false;
            transform.GetComponentInChildren<HPBar>().invisible();
        }
        else if (SelectUnit.isUnit == true)
        {
            selected = false;
            transform.GetComponentInChildren<HPBar>().invisible();
            if (SelectUnit.hitInfo.collider.transform.position == transform.position)
            {
                selected = true;
                transform.GetComponentInChildren<HPBar>().visible();
            }
        }
    }

    private void DetectEnemy()
    {
        if (condition != "Attacking" && condition != "OnlyMoving")
        {
            if (Enemy != null)
            {
                Enemy.GetComponent<Status>().AttackMe.Remove(gameObject);
            }
            colliders = Physics.OverlapCapsule(transform.position - new Vector3(0, 10, 0), transform.position + new Vector3(0, 10, 0), transform.GetComponent<Status>().GetAttackRange());
            foreach (Collider col in colliders)
            {
                if (col.tag == "Unit" && col.GetComponent<Status>().GetHp() >= 0 && col.GetComponent<Status>().GetPlayer() != transform.GetComponent<Status>().GetPlayer())
                {
                    if(Enemy == null)
                    {
                        Enemy = col;
                    }
                    else
                    {
                        if(Enemy.GetComponent<Status>().AttackMe.Count > col.GetComponent<Status>().AttackMe.Count)
                        {
                            Enemy = col;
                        }
                    }
                }
            }
            if (Enemy != null)
            {
                Destination = Enemy.transform.position;
                nvo.enabled = false;
                nv.enabled = true;
                condition = "Tracing";
                Move(Destination);
                Enemy.GetComponent<Status>().AttackMe.Add(gameObject);
                return ;
            }
            colliders = Physics.OverlapCapsule(transform.position - new Vector3(0, 10, 0), transform.position + new Vector3(0, 10, 0), 3f);
            foreach (Collider col in colliders)
            {
                if (col.tag == "Unit" && col.GetComponent<Status>().GetHp() >= 0 && col.GetComponent<Status>().GetPlayer() != transform.GetComponent<Status>().GetPlayer())
                {
                    if (Enemy == null)
                    {
                        Enemy = col;
                    }
                    else
                    {
                        if (Enemy.GetComponent<Status>().AttackMe.Count > col.GetComponent<Status>().AttackMe.Count)
                        {
                            Enemy = col;
                        }
                    }
                }
            }
            if (Enemy != null)
            {
                Destination = Enemy.transform.position;
                nvo.enabled = false;
                nv.enabled = true;
                condition = "Tracing";
                Move(Destination);
                Enemy.GetComponent<Status>().AttackMe.Add(gameObject);
                return;
            }
        }
    }

    private void Attacking()
    {
        DetectEnemy();
        if (condition == "Tracing" || condition == "Attacking")
        {
            if (Enemy.GetComponent<Status>().GetHp() <= 0)
            {
                condition = "Stopping";
            }
            if (condition == "Tracing")
            {
                if (Enemy.transform.position != MyDset)
                {
                    Destination = Enemy.transform.position;
                    Move(Destination);
                }
                
            }
            if(condition != "Stopping" && Vector3.Distance(Enemy.transform.position,transform.position) <= GetComponent<Status>().GetAttackRange())
            {
                Enemy.GetComponent<Status>().Damage(GetComponent<Status>().GetAttackDamage());
                condition = "Attacking";
                if (Enemy.GetComponent<Status>().GetHp() <= 0)
                {
                    condition = "Stopping";
                }
            }
            else if(condition == "Attacking")
            {
                condition = "Stopping";
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

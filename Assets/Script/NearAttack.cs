using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NearAttack : Attacking
{
    float AttackSpeed = 0f;
    bool Attack = false;

    // Start is called before the first frame update
    void Start()
    {
        DistanceAttack = transform.GetComponent<Status>().GetDistanceAttack();
        moving = transform.GetComponent<Moving>();
    }

    // Update is called once per frame
    void Update()
    {
        Condition = transform.GetComponent<Status>().GetCondition();
        Attacking();
    }

    void SetCondition(string Con)
    {
        Condition = Con;
        transform.GetComponent<Status>().SetCondition(Con);
    }

    private void DetectEnemy()
    {
        if (Condition != "Attacking" && Condition != "OnlyMoving" && Condition != "ARMoving")
        {
            if (Enemy != null)
            {
                Enemy.GetComponent<Status>().AttackMe.Remove(gameObject);
                Enemy = null;
            }
            colliders = Physics.OverlapCapsule(transform.position - new Vector3(0, 10, 0), transform.position + new Vector3(0, 10, 0), transform.GetComponent<Status>().GetAttackRange() - 0.1f);
            foreach (Collider col in colliders)
            {
                if (col.gameObject.layer == 11 && col.tag == "GroundUnit" && col.GetComponent<Status>().GetHp() >= 0 && col.GetComponent<Status>().GetPlayer() != transform.GetComponent<Status>().GetPlayer())
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
                if(Condition == "Stopping")
                {
                    moving.DestBeforeAttack = transform.position;
                }
                else if(Condition == "Moving" || Condition == "OnlyMoving")
                {
                    moving.DestBeforeAttack = moving.Destination;
                }
                moving.Destination = Enemy.transform.position;
                moving.nvo.enabled = false;
                moving.nv.enabled = true;
                moving.nv.stoppingDistance = 0;
                SetCondition("Attacking");
                Enemy.GetComponent<Status>().AttackMe.Add(gameObject);
                Attack = true;
                return;
            }
            colliders = Physics.OverlapCapsule(transform.position - new Vector3(0, 10, 0), transform.position + new Vector3(0, 10, 0), 10f);
            foreach (Collider col in colliders)
            {
                if (col.gameObject.layer == 11 && col.tag == "GroundUnit" && col.GetComponent<Status>().GetHp() >= 0 && col.GetComponent<Status>().GetPlayer() != transform.GetComponent<Status>().GetPlayer())
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
                if (Condition == "Stopping")
                {
                    moving.DestBeforeAttack = transform.position;
                }
                else if (Condition == "Moving" || Condition == "OnlyMoving")
                {
                    moving.DestBeforeAttack = moving.Destination;
                }
                moving.Destination = Enemy.transform.position;
                moving.nvo.enabled = false;
                moving.nv.enabled = true;
                moving.nv.stoppingDistance = 0;
                SetCondition("Tracing");
                Enemy.GetComponent<Status>().AttackMe.Add(gameObject);
                Attack = false;
                return;
            }
        }
        else if(Condition == "Attacking")
        {
            colliders = Physics.OverlapCapsule(transform.position - new Vector3(0, 10, 0), transform.position + new Vector3(0, 10, 0), transform.GetComponent<Status>().GetAttackRange());
            foreach (Collider col in colliders)
            {
                if (col == Enemy)
                {
                    Attack = true;
                    return;
                }
            }
            Attack = false;
            moving.nvo.enabled = false;
            moving.nv.enabled = true;
            moving.nv.stoppingDistance = 0;
            SetCondition("Moving");
            moving.Destination = moving.DestBeforeAttack;
        }
    }

    private void Attacking()
    {
        DetectEnemy();
        if (Condition == "Tracing" || Condition == "Attacking")
        {
            if (Enemy == null || Enemy.GetComponent<Status>().GetHp() <= 0)
            {
                moving.nvo.enabled = false;
                moving.nv.enabled = true;
                moving.nv.stoppingDistance = 0;
                SetCondition("Moving");
                moving.Destination = moving.DestBeforeAttack;
                Attack = false;
                return;
            }
            if (Condition == "Tracing")
            {
                if (Enemy.transform.position != moving.MyDset)
                {
                    moving.Destination = Enemy.transform.position;
                }
            }
            if (Attack)
            {
                if (AttackSpeed > 0)
                {
                    AttackSpeed = AttackSpeed - Time.deltaTime;
                }
                if (AttackSpeed <= 0)
                {
                    Enemy.GetComponent<Status>().Damage(GetComponent<Status>().GetAttackDamage());
                    AttackSpeed = transform.GetComponent<Status>().GetAttackSpeed();
                }
                SetCondition("Attacking");
                if (Enemy == null || Enemy.GetComponent<Status>().GetHp() <= 0)
                {
                    moving.nvo.enabled = false;
                    moving.nv.enabled = true;
                    moving.nv.stoppingDistance = 0;
                    SetCondition("Moving");
                    moving.Destination = moving.DestBeforeAttack;
                    Attack = false;
                    return;
                }
            }
        }
    }
}

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
        if(Condition == "OnlyMoving")
        {
            if(Enemy != null)
                Enemy = null;
            colliders = Physics.OverlapCapsule(transform.position - new Vector3(0, 10, 0), transform.position + new Vector3(0, 10, 0), 10);
            foreach (Collider col in colliders)
            {
                if ((col.gameObject.layer == 11 || col.gameObject.layer == 10) && !col.gameObject.GetPhotonView().isMine)
                {
                    transform.GetComponent<Status>().SetVisible(true);
                    return;
                }
            }
            transform.GetComponent<Status>().SetVisible(false);
        }
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
                if((col.gameObject.layer == 11 || col.gameObject.layer == 10) && !col.gameObject.GetPhotonView().isMine)
                {
                    transform.GetComponent<Status>().SetVisible(true);
                }
                if (((col.gameObject.layer == 11 && (col.tag == "GroundUnit" || col.tag == "Worker")) || col.gameObject.layer == 10) && col.GetComponent<Status>().GetHp() >= 0 && !col.gameObject.GetPhotonView().isMine)
                {
                    if (Enemy == null)
                    {
                        Enemy = col;
                    }
                    else
                    {
                        if (Enemy.gameObject.layer == 11)
                        {
                            if (col.gameObject.layer == 11 && Enemy.GetComponent<Status>().AttackMe.Count > col.GetComponent<Status>().AttackMe.Count)
                            {
                                Enemy = col;
                            }
                        }
                        else if (col.gameObject.layer == 11)
                        {
                            Enemy = col;
                        }
                    }
                }
            }
            if (Enemy != null && Enemy.gameObject.layer == 11)
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
                moving.nv.isStopped = false;
                moving.nv.SetDestination(moving.Destination);
                moving.nv.stoppingDistance = 0;
                SetCondition("Attacking");
                Enemy.GetComponent<Status>().AttackMe.Add(gameObject);
                Attack = true;
                return;
            }
            else if (Enemy != null && Enemy.gameObject.layer == 10)
            {
                Attack = true;
            }
            colliders = Physics.OverlapCapsule(transform.position - new Vector3(0, 10, 0), transform.position + new Vector3(0, 10, 0), 10f);
            foreach (Collider col in colliders)
            {
                if ((col.gameObject.layer == 11 || col.gameObject.layer == 10) && !col.gameObject.GetPhotonView().isMine)
                {
                    transform.GetComponent<Status>().SetVisible(true);
                }
                if ((col.gameObject.layer == 11 || col.gameObject.layer == 10) && col.GetComponent<Status>().GetHp() >= 0 && !col.gameObject.GetPhotonView().isMine)
                {
                    if (Enemy == null)
                    {
                        Enemy = col;
                    }
                    else
                    {
                        if (Enemy.gameObject.layer == 11)
                        {
                            if (col.gameObject.layer == 11 && Enemy.GetComponent<Status>().AttackMe.Count > col.GetComponent<Status>().AttackMe.Count)
                            {
                                Enemy = col;
                            }
                        }
                        else if (col.gameObject.layer == 11)
                        {
                            Enemy = col;
                        }
                    }
                }
            }
            if (Enemy != null)
            {
                if (Enemy.gameObject.tag != "AirUnit")
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
                    moving.nv.isStopped = false;
                    moving.nv.SetDestination(moving.Destination);
                    moving.nv.stoppingDistance = 0;
                    SetCondition("Tracing");
                    Enemy.GetComponent<Status>().AttackMe.Add(gameObject);
                    if (!Attack)
                    {
                        Attack = false;
                    }
                }
                return;
            }
            transform.GetComponent<Status>().SetVisible(false);
        }
        else if (Condition == "Attacking")
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
            moving.nv.stoppingDistance = 0;
            SetCondition("Moving");
            moving.Destination = moving.DestBeforeAttack;
            moving.nv.isStopped = false;
            moving.nv.SetDestination(moving.Destination);
        }
    }

    private void FindEnemy()
    {
        if (EnemyID != 0)
        {
            FindedEnemy = PhotonView.Find(EnemyID).gameObject;
            if (AttackSpeed > 0)
            {
                AttackSpeed = AttackSpeed - Time.deltaTime;
            }
            if (AttackSpeed <= 0)
            {
                FindedEnemy.GetComponent<Status>().Damage(GetComponent<Status>().GetAttackDamage() + Bonus);
                if (Splash)
                {
                    colliders = Physics.OverlapCapsule(transform.position - new Vector3(0, 10, 0), transform.position + new Vector3(0, 10, 0), 3);
                    foreach (Collider col in colliders)
                    {
                        if (col.gameObject.layer == 11 && col != FindedEnemy)
                        {
                            col.GetComponent<Status>().Damage(GetComponent<Status>().GetAttackDamage() + Bonus);
                        }
                    }
                    transform.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.player.ID);
                    PhotonNetwork.Destroy(gameObject);
                }
                AttackSpeed = transform.GetComponent<Status>().GetAttackSpeed();
            }
        }
        else
        {
            Attack = false;
        }
    }

    private void Attacking()
    {
        if (photonView.isMine)
        {
            if (Condition == "Building")
            {
                colliders = Physics.OverlapCapsule(transform.position - new Vector3(0, 10, 0), transform.position + new Vector3(0, 10, 0), 10);
                foreach (Collider col in colliders)
                {
                    if ((col.gameObject.layer == 11 || col.gameObject.layer == 10) && !col.gameObject.GetPhotonView().isMine)
                    {
                        Debug.Log(col.name);
                        transform.GetComponent<Status>().SetVisible(true);
                        return;
                    }
                }
                transform.GetComponent<Status>().SetVisible(false);
                return;
            }
            DetectEnemy();
        }
        else
        {
            FindEnemy();
            return;
        }
        if (Condition == "Tracing" || Condition == "Attacking")
        {
            if (Enemy == null || Enemy.GetComponent<Status>().GetHp() <= 0)
            {
                moving.nv.stoppingDistance = 0;
                SetCondition("Moving");
                moving.Destination = moving.DestBeforeAttack;
                moving.nv.isStopped = false;
                moving.nv.SetDestination(moving.Destination);
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
                    Enemy.GetComponent<Status>().Damage(GetComponent<Status>().GetAttackDamage() + Bonus);
                    foreach (GameObject obj in transform.GetComponent<Status>().AttackMe)
                    {
                        obj.GetComponent<Attacking>().Enemy = null;
                    }
                    if (gameObject.GetComponent<Attacking>().Enemy != null)
                    {
                        gameObject.GetComponent<Attacking>().Enemy.GetComponent<Status>().AttackMe.Remove(gameObject);
                    }
                    SelectUnit.SelectedUnit.Remove(gameObject);
                    AttackSpeed = transform.GetComponent<Status>().GetAttackSpeed();
                }
                SetCondition("Attacking");
                if (Enemy == null || Enemy.GetComponent<Status>().GetHp() <= 0)
                {
                    moving.nv.stoppingDistance = 0;
                    SetCondition("Moving");
                    moving.Destination = moving.DestBeforeAttack;
                    moving.nv.isStopped = false;
                    moving.nv.SetDestination(moving.Destination);
                    Attack = false;
                    return;
                }
            }
        }
    }
}

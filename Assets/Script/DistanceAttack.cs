using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceAttack : Attacking
{
    [SerializeField]
    private GameObject shot;

    float AttackSpeed = 0f;
    bool Attack = false;
    private GameObject shotprefab;

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
        if (Condition == "OnlyMoving")
        {
            if (Enemy != null)
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
        else if (Condition != "Attacking" && Condition != "OnlyMoving" && Condition != "ARMoving")
        {
            if (Enemy != null)
            {
                Enemy.GetComponent<Status>().AttackMe.Remove(gameObject);
                Enemy = null;
            }
            colliders = Physics.OverlapCapsule(transform.position - new Vector3(0, 10, 0), transform.position + new Vector3(0, 10, 0), transform.GetComponent<Status>().GetAttackRange());
            foreach (Collider col in colliders)
            {
                if ((col.gameObject.layer == 11 || col.gameObject.layer == 10) && col.GetComponent<Status>().GetHp() >= 0 && !col.gameObject.GetPhotonView().isMine)
                {
                    transform.GetComponent<Status>().SetVisible(true);
                    if (Enemy == null)
                    {
                        Enemy = col;
                    }
                    else
                    {
                        if (Enemy.gameObject.layer == 11)
                        {
                            if(col.gameObject.layer == 11 && Enemy.GetComponent<Status>().AttackMe.Count > col.GetComponent<Status>().AttackMe.Count)
                            {
                                Enemy = col;
                            }
                        }
                        else if(col.gameObject.layer == 11)
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
                SetCondition("Tracing");
                Enemy.GetComponent<Status>().AttackMe.Add(gameObject);
                Attack = true;
                return;
            }
            else if(Enemy != null && Enemy.gameObject.layer == 10)
            {
                Attack = true;
            }
            colliders = Physics.OverlapCapsule(transform.position - new Vector3(0, 10, 0), transform.position + new Vector3(0, 10, 0), 10f);
            foreach (Collider col in colliders)
            {
                if ((col.gameObject.layer == 11 || col.gameObject.layer == 10) && col.GetComponent<Status>().GetHp() >= 0 && !col.gameObject.GetPhotonView().isMine)
                {
                    transform.GetComponent<Status>().SetVisible(true);
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
                return;
            }
            transform.GetComponent<Status>().SetVisible(false);
        }
        else if(Condition == "Attacking")
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
            colliders = Physics.OverlapCapsule(transform.position - new Vector3(0, 10, 0), transform.position + new Vector3(0, 10, 0), transform.GetComponent<Status>().GetAttackRange()+2f);
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
            moving.Destination = Enemy.transform.position;
            moving.nv.isStopped = false;
            moving.nv.SetDestination(moving.Destination);
        }
    }

    private void FindEnemy()
    {
        if(EnemyID != 0)
        {
            if (!(FindedEnemy = PhotonView.Find(EnemyID).gameObject))
            {
                return;
            }
            if (AttackSpeed > 0)
            {
                AttackSpeed = AttackSpeed - Time.deltaTime;
            }
            if (AttackSpeed <= 0)
            {
                FindedEnemy.GetComponent<Status>().Damage(GetComponent<Status>().GetAttackDamage() + Bonus);
                if (Splash)
                {
                    colliders = Physics.OverlapCapsule(FindedEnemy.transform.position - new Vector3(0, 10, 0), FindedEnemy.transform.position + new Vector3(0, 10, 0), 3);
                    foreach (Collider col in colliders)
                    {
                        if (col.gameObject.layer == 11 && col != FindedEnemy)
                        {
                            col.GetComponent<Status>().Damage((GetComponent<Status>().GetAttackDamage() / 2) + Bonus);
                        }
                    }
                }
                AttackSpeed = transform.GetComponent<Status>().GetAttackSpeed();
                shotprefab = Instantiate(shot, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
                shotprefab.GetComponent<Throw>().FindedEnemy = FindedEnemy;
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
                    moving.nv.isStopped = false;
                    moving.nv.SetDestination(moving.Destination);
                }
            }
            if (Attack) //뒷부분
            {
                if (AttackSpeed > 0)
                {
                    AttackSpeed = AttackSpeed - Time.deltaTime;
                }
                if (AttackSpeed <= 0)
                {
                    Enemy.GetComponent<Status>().Damage(GetComponent<Status>().GetAttackDamage() + Bonus);
                    AttackSpeed = transform.GetComponent<Status>().GetAttackSpeed();
                    shotprefab = Instantiate(shot, transform.position + new Vector3(0,1,0), Quaternion.identity);
                    shotprefab.GetComponent<Throw>().Enemy = Enemy;
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
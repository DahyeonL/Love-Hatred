using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacking : Photon.MonoBehaviour
{

    [SerializeField]
    protected bool Splash = false;

    //Attacking
    protected Collider[] colliders;
    public Collider Enemy;
    protected string Condition;
    protected Moving moving;
    protected float Distance;
    public int EnemyID;
    public GameObject FindedEnemy;
    public int Bonus = 0;
}

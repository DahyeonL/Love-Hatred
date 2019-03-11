using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Moving : MonoBehaviour
{
    public string Condition;
    
    //Moving
    public NavMeshAgent nv;
    public NavMeshObstacle nvo;
    protected float predistance = 0;
    public Vector3 MyDset;
    protected int WaitCount = 0;
    protected int predirection;
    public Vector3 Destination;
    protected Vector3 ClickPosition;
    protected int MapLayer = 9;
    protected Vector3 final;
    protected bool ARMoving = false;
    protected GameObject ARMoveDest;
    protected GameObject ARMoveStart;
    protected Vector3 ARDestination;
    public Vector3 DestBeforeAttack;
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AniController : MonoBehaviour
{
    public Animator Check;

    private void Start()
    {
        Check = gameObject.GetComponent<Animator>();
    }
    public void ConditionCheck()
    {
        if (GetComponent<Status>().GetCondition() == "Moving" || GetComponent<Status>().GetCondition() == "OnlyMoving" || GetComponent<Status>().GetCondition() == "Tracing")
        {
            Check.SetBool("Attacking", false);
            Check.SetBool("Moving",true);
        }
        else if(GetComponent<Status>().GetCondition() == "Attacking")
        {
            Check.SetBool("Moving", false);
            Check.SetBool("Attacking", true);
        }
        else
        {
            Check.SetBool("Moving", false);
            Check.SetBool("Attacking", false);
        }
    }
    private void Update()
    {
        ConditionCheck();
    }
}

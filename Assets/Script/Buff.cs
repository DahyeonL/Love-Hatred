using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : Attacking
{
    [SerializeField]
    bool Damage;
    [SerializeField]
    bool Healing;
    
    List<GameObject> BuffedUnit = new List<GameObject>();
    List<GameObject> pre_BuffedUnit = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.isMine)
        {
            FindEnemy();
            FindBuffedUnit();
            if (Damage)
            {
                Buff_Damage();
            }
            else if (Healing)
            {
                Buff_Healing();
            }
            pre_BuffedUnit = new List<GameObject>(BuffedUnit);
        }
    }

    void FindEnemy()
    {
        colliders = Physics.OverlapCapsule(transform.position - new Vector3(0, 10, 0), transform.position + new Vector3(0, 10, 0), 10);
        foreach(Collider col in colliders)
        {
            if ((col.gameObject.layer == 11 || col.gameObject.layer == 10) && !col.gameObject.GetPhotonView().isMine)
            {
                transform.GetComponent<Status>().SetVisible(true);
                return;
            }
        }
        transform.GetComponent<Status>().SetVisible(false);
    }

    void FindBuffedUnit()
    {
        colliders = Physics.OverlapCapsule(transform.position - new Vector3(0, 10, 0), transform.position + new Vector3(0, 10, 0), 4);
        BuffedUnit.Clear();
        foreach (Collider col in colliders)
        {
            if (col.gameObject.layer == 11 && col.gameObject.GetPhotonView().isMine)
            {
                BuffedUnit.Add(col.gameObject);
            }
        }
    }

    void Buff_Damage()
    {
        foreach(GameObject obj in BuffedUnit)
        {
            if (!pre_BuffedUnit.Contains(obj))
            {
                obj.GetComponent<Attacking>().Bonus = 2;
            }
        }
        foreach(GameObject obj in pre_BuffedUnit)
        {
            if (obj == null)
            {
                pre_BuffedUnit.Remove(obj);
            }
            else if (!BuffedUnit.Contains(obj))
            {
                obj.GetComponent<Attacking>().Bonus = 0;
            }
        }
    }

    void Buff_Healing()
    {
        foreach (GameObject obj in BuffedUnit)
        {
            if (!pre_BuffedUnit.Contains(obj))
            {
                obj.GetComponent<Status>().SetHealing(true);
            }
        }
        foreach (GameObject obj in pre_BuffedUnit)
        {
            if (obj == null)
            {
                pre_BuffedUnit.Remove(obj);
            }
            else if (!BuffedUnit.Contains(obj))
            {
                obj.GetComponent<Status>().SetHealing(false);
            }
        }
    }
}

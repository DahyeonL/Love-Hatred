using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : Photon.MonoBehaviour
{
    [SerializeField]
    protected GameObject DeathEffect;

    protected string Condition;
    public float MaxHP;
    public float HP;
    public int AttackDamage;
    protected int Armor;
    public float AttackRange;
    protected int MoveSpeed;
    public float AttackSpeed;
    public bool Visible = false;
    protected bool Healing = false;
    protected float count = 0f;
    public int Cost;
    public float CreatingTime;
    public List<GameObject> AttackMe = new List<GameObject>();

    public void Damage(int HitDamage)
    {
        if (photonView.isMine)
        {
            HP = HP - HitDamage;
            if (HP <= 0)
            {
                foreach (GameObject obj in AttackMe)
                {
                    obj.GetComponent<Attacking>().Enemy = null;
                }
                if (gameObject.layer == 11)
                {
                    if (gameObject.GetComponent<Attacking>().Enemy != null)
                    {
                        gameObject.GetComponent<Attacking>().Enemy.GetComponent<Status>().AttackMe.Remove(gameObject);
                    }
                    SelectUnit.SelectedUnit.Remove(gameObject);
                    if (transform.tag == "Worker")
                    {
                        BuildManager.Workers.Remove(gameObject);
                    }
                }
                else if (gameObject.layer == 10)
                {
                    SelectUnit.SelectedBuilding.Remove(gameObject);
                    ScoreController.MyBuildings = ScoreController.MyBuildings - 1;
                }
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }

    public string GetCondition()
    {
        return Condition;
    }

    public void SetCondition(string Con)
    {
        Condition = Con;
    }

    public float GetMaxHP()
    {
        return MaxHP;
    }

    public void SetMaxHP(float MaxHP)
    {
        this.MaxHP = MaxHP;
    }

    public float GetHp()
    {
        return HP;
    }

    public void SetHP(float HP)
    {
        this.HP = HP;
    }

    public int GetAttackDamage()
    {
        return AttackDamage;
    }
    public void SetAttackDamage(int AttackDamage)
    {
        this.AttackDamage = AttackDamage;
    }

    public int GetArmor()
    {
        return Armor;
    }
    public void SetArmor(int Armor)
    {
        this.Armor = Armor;
    }
    public float GetAttackRange()
    {
        return AttackRange;
    }
    public void SetAttackRange(float AttackRange)
    {
        this.AttackRange = AttackRange;
    }

    public float GetAttackSpeed()
    {
        return AttackSpeed;
    }
    public void SetAttackSpeed(float AttackSpeed)
    {
        this.AttackSpeed = AttackSpeed;
    }
    public bool GetVisible()
    {
        return Visible;
    }
    public void SetVisible(bool b)
    {
        Visible = b;
    }
    public bool GetHealing()
    {
        return Healing;
    }
    public void SetHealing(bool b)
    {
        Healing = b;
    }
    public int GetCost()
    {
        return Cost;
    }
    public float GetTime()
    {
        return CreatingTime;
    }
}

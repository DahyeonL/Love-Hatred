using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    [SerializeField]
    private GameObject DeathEffect;

    private string Condition;
    public bool DistanceAttack;
    public bool isAir;
    public int HP;
    public int AttackDamage;
    private int Armor;
    public float AttackRange;
    private int MoveSpeed;
    public float AttackSpeed;
    public int Player;
    public List<GameObject> AttackMe = new List<GameObject>();
    

    public void Damage(int HitDamage)
    {
        HP = HP - HitDamage;
        if (HP <= 0)
        {
            foreach(GameObject obj in AttackMe)
            {
                obj.GetComponent<Attacking>().Enemy = null;
            }
            if (gameObject.GetComponent<Attacking>().Enemy != null)
            {
                gameObject.GetComponent<Attacking>().Enemy.GetComponent<Status>().AttackMe.Remove(gameObject);
            }
            //Instantiate(DeathEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    private void die(int count)
    {
        if(count == 0)
        {
            Destroy(gameObject);
        }
        else
        {
            count = count - 1;
            die(count);
        }
    }

    public bool GetisAir()
    {
        return isAir;
    }

    public void SetisAir(bool b)
    {
        isAir = b;
    }

    public bool GetDistanceAttack()
    {
        return DistanceAttack;
    }

    public void SetDistanceAttack(bool b)
    {
        DistanceAttack = b;
    }

    public string GetCondition()
    {
        return Condition;
    }

    public void SetCondition(string Con)
    {
        Condition = Con;
    }

    public int GetHp()
    {
        return HP;
    }

    public void SetHP(int HP)
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
    public int GetPlayer()
    {
        return Player;
    }
    public void SetPlayer(int Player)
    {
        this.Player = Player;
    }
}

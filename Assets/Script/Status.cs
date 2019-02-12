using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    public int HP;
    public int AttackDamage;
    private int Armor;
    public float AttackRange;
    private int MoveSpeed;
    private int AttackSpeed;
    public int Player;
    public bool fullyAttacked = false;
    public List<GameObject> AttackMe;
    

    public void Damage(int HitDamage)
    {
        HP = HP - HitDamage;
        if (HP <= 0)
        {
            foreach(GameObject obj in AttackMe)
            {
                obj.GetComponent<GroundUnit>().condition = "Stopping";
            }
            gameObject.GetComponent<GroundUnit>().Enemy.GetComponent<Status>().AttackMe.Remove(gameObject);
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

    public int GetAttackSpeed()
    {
        return AttackSpeed;
    }
    public void SetAttackSpeed(int AttackSpeed)
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Attacker : Damageable
{
    public StatsWrapper Wrapper;
    public Stats Stats 
    { 
        get { return Wrapper.Stats; } 
        set { Wrapper.Stats = value; } 
    }
    [Space]
    public bool InRangeToAttack;
    public AttackCollider AttackColl;
    public PoolableGameObject Poolable;

    public override float GetMaxHealth()
    {
        return Stats.maxHealth;
    }

    public virtual float GetStrength()
    {
        return Stats.strength;
    }

    protected override void Awake()
    {
        base.Awake();
        Poolable = GetComponent<PoolableGameObject>();
        if (AttackColl == null)
            AttackColl = GetComponent<AttackCollider>();
        
        AttackColl?.SetAttacker(this);
    }

    public void Attack(Damageable otherUnit)
    {
        otherUnit.GetAttacked(Stats.strength);
    }

    public void Attack(Damageable[] otherUnits)
    {
        foreach(Damageable otherUnit in otherUnits)
        {
            Attack(otherUnit);
        }
    }

    public void PerformAttack()
    {
        Damageable[] attackersInRange = AttackColl.GetEnemiesInRange();
        Attack(attackersInRange);
    }

    public override void Die()
    {
        if (ArmyID != 0) // from enemy army
        {
            BankManager.Instance.EarnMoney(Stats.coinValue);
        }
        base.Die();
        Poolable.ReturnToPool();
    }
}

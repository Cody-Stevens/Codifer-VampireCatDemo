using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    public bool EnemiesInRange => m_inRange.Count > 0;

    private Attacker m_attacker;
    private List<Damageable> m_inRange = new();

    public void SetAttacker(Attacker attacker)
    {
        m_attacker = attacker;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger) return;

        Damageable otherAttacker = other.GetComponent<Damageable>();
        if (otherAttacker != null && otherAttacker.ArmyID != m_attacker.ArmyID && !m_inRange.Contains(otherAttacker))
        {
            if (m_inRange.Count == 0)
            {
                m_attacker.InRangeToAttack = true;
            }
            m_inRange.Add(otherAttacker);
            otherAttacker.OnDeath += OnOpponentDead;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.isTrigger) return;

        Damageable otherAttacker = other.GetComponent<Damageable>();
        if (otherAttacker != null && m_inRange.Contains(otherAttacker))
        {
            RemoveOpponent(otherAttacker);
        }
    }

    private void OnOpponentDead(Damageable attacker)
    {
        if (m_inRange.Contains(attacker))
        {
            RemoveOpponent(attacker);
        }
    }

    private void RemoveOpponent(Damageable opponent)
    {
        m_inRange.Remove(opponent);
            opponent.OnDeath -= OnOpponentDead;
            if (m_inRange.Count == 0)
            {
                m_attacker.InRangeToAttack = false;
            }
    }

    public Damageable[] GetEnemiesInRange()
    {
        return m_inRange.ToArray();
    }
}

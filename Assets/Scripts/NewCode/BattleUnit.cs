using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BattleUnit : Attacker
{
    public enum BattleState
    {
        Idle,
        Moving,
        Fighting
    }

    [Space]
    public bool Selectable;
    public GameObject SelectedVisuals;

    private BattleState m_state;
    private NavMeshAgent m_agent;
    private Vector3 m_destination;
    private HashSet<Damageable> m_opponents = new();
    private float m_attackCooldown;

    public string GetState()
    {
        return m_state.ToString();
    }

    protected override void Awake()
    {
        base.Awake();
        m_agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        switch(m_state)
        {
            case BattleState.Idle:
            case BattleState.Moving:
                if (InRangeToAttack)
                {
                    ToFighting();
                }
                else if (m_opponents.Count > 0)
                {
                    m_agent.SetDestination(GetClosestOpponentPosition());
                }

                else if (Vector3.Distance(transform.position, m_agent.destination) < m_agent.stoppingDistance)
                {
                    ToIdle();
                }
                break;
            case BattleState.Fighting:
                if (!InRangeToAttack)
                {
                    ToMoving();
                }
                if (!AttackColl.EnemiesInRange)
                {
                    ToIdle();
                }
                else
                {
                    m_attackCooldown -= Time.deltaTime;
                    if (m_attackCooldown <= 0f)
                    {
                        Visuals.PlayAttackAnimation();
                        m_attackCooldown += Stats.timebetweenattacks;
                    }
                }
                break;
        }
    }

    public void Select()
    {
        if (!Selectable) return;

        SelectedVisuals?.SetActive(true);
    }

    public void Deselect()
    {
        SelectedVisuals?.SetActive(false);
    }

    public void PerformAttack() // used by animator at point of attack contact
    {
        Damageable[] attackersInRange = AttackColl.GetEnemiesInRange();
        Attack(attackersInRange);
    }

    public void ToIdle()
    {
        StopMoving();
        m_state = BattleState.Idle;
        // nothing
    }

    public void ToMoving()
    {
        SetDestination(m_destination);
        m_state = BattleState.Moving;
    }

    public void ToFighting()
    {
        m_state = BattleState.Fighting;
    }

    public void StopMoving()
    {
        m_agent.SetDestination(transform.position);
    }

    public void SetDestination(Vector3 destination)
    {
        m_destination = destination;
        m_agent.SetDestination(m_destination);
        m_state = BattleState.Moving;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger) return;

        Attacker attacker = other.GetComponent<Attacker>();
        if (attacker != null && attacker.ArmyID != ArmyID)
        {
            AddOpponent(attacker);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.isTrigger) return;

        Attacker attacker = other.GetComponent<Attacker>();
        if (attacker != null && attacker.ArmyID != ArmyID && m_opponents.Contains(attacker))
        {
            LoseOpponent(attacker);
        }
    }

    private void OnOpponentDeath(Damageable opponent)
    {
        if (m_opponents.Contains(opponent))
        {
            LoseOpponent(opponent);
        }
    }

    public void AddOpponent(Damageable opponent)
    {
        m_opponents.Add(opponent);
        opponent.OnDeath += OnOpponentDeath;

        if (m_state != BattleState.Fighting)
        {
            float distanceToOpponent = Vector3.Distance(transform.position, opponent.transform.position);
            if (distanceToOpponent < Vector3.Distance(transform.position, m_agent.destination))
            {
                m_agent.SetDestination(opponent.transform.position);
                ToMoving();
            }
        }
    }

    public void LoseOpponent(Damageable opponent)
    {
        opponent.OnDeath -= OnOpponentDeath;
        m_opponents.Remove(opponent);
        if (m_opponents.Count > 0)
        {
            Vector3 positionToClosestEnemy = GetClosestOpponentPosition();
            m_agent.SetDestination(positionToClosestEnemy);
        }
        else
        {
            ToIdle();
        }
    }

    private Vector3 GetClosestOpponentPosition()
    {
        Vector3 closestPosition = transform.position;
        float closestDistance = float.MaxValue;
        foreach(Attacker opponent in m_opponents)
        {
            Vector3 position = opponent.transform.position;
            float distance = Vector3.Distance(transform.position, position);
            if (distance < closestDistance)
            {
                closestPosition = position;
                closestDistance = distance;
            }
        }

        return closestPosition;
    }
}

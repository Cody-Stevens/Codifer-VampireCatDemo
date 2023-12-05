using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackerVisuals : DamageableVisuals
{
    private Attacker m_attacker;

    protected override void Awake()
    {
        base.Awake();
        m_attacker = Damageable as Attacker;
    }

    protected override void PerformAttack()
    {
        base.PerformAttack();
        m_attacker.PerformAttack();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageableVisuals : MonoBehaviour
{
    private const float HitFlashTime = 0.2f;

    public Damageable Damageable;
    public GameObject NormalVisuals;
    public GameObject HitVisuals;

    private Animator m_animator;
    private Coroutine m_hitCoroutine;

    protected virtual void Awake()
    {
        Damageable.OnHit += OnHit;
        Damageable.OnDeath += OnDeath;

        m_animator = GetComponent<Animator>();
    }

    private void OnDestroy()
    {
        Damageable.OnHit -= OnHit;
        Damageable.OnDeath -= OnDeath;
    }

    private void OnHit()
    {
        if (m_hitCoroutine != null)
        {
            StopCoroutine(m_hitCoroutine);
        }
        
        m_hitCoroutine = StartCoroutine(HitCoroutine());
    }

    private void OnDeath(Damageable attacker)
    {
        
    }

    public void PlayAttackAnimation()
    {
        m_animator.Play("Attack");
    }

    protected virtual void PerformAttack()
    {
    }

    private IEnumerator HitCoroutine()
    {
        NormalVisuals.SetActive(false);
        HitVisuals.SetActive(true);

        yield return new WaitForSeconds(HitFlashTime);

        NormalVisuals.SetActive(true);
        HitVisuals.SetActive(false);
    }
}

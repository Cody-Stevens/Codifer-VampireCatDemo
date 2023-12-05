using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour, IDamageable
{
    public float maxHealth;
    public float currentHealth;

    public PlayerType PlayerType { get; set; } = PlayerType.Enemy;

    public int damageValue = 1;
    public int coinValue = 3;

    public Transform target;

    [SerializeField] GameObject enemyHitVFX;
    [SerializeField] GameObject enemyDeathVFX;

    public Action<Enemy> OnEnemyDeath;
    public StatsWrapper Wrapper;
    private PlayerType targetType = PlayerType.Player;
    private Transform playerTransform;
    private Transform portalTransform;
    private NavMeshAgent navAgent;
    public Stats stats
    {
        get { return Wrapper.Stats; }
        set { Wrapper.Stats = value; }
    }

    private void Awake()
    {
        maxHealth = UnityEngine.Random.Range(40, 60);
        currentHealth = maxHealth;
        navAgent = GetComponent<NavMeshAgent>();
        navAgent.updateRotation = false;
        navAgent.updateUpAxis = false;
    }
    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        portalTransform = GameObject.FindGameObjectWithTag("Portal").transform;
        
        int targetIndex = UnityEngine.Random.Range(0, 4);
        StartCoroutine(LazyTargeting(targetIndex));

        stats.coinValue = coinValue;
    }

    private IEnumerator LazyTargeting(int targetIndex)
    {
        do
        {
            //if ((transform.position - playerTransform.position).sqrMagnitude < (transform.position - portalTransform.position).sqrMagnitude)
            //{
            //    target = playerTransform;
            //    targetType = PlayerType.Player;
            //}
            //else
            //{
            //    target = portalTransform;
            //    targetType = PlayerType.Player;
            //}
            if (targetIndex == 0)
            {
                target = portalTransform;
            }
            else
            {
                target = playerTransform;
            }
            navAgent.SetDestination(target.position);
            yield return new WaitForSeconds(.1f);
        } while (true);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        DealDamage(collision);
    }

    private void DealDamage(Collider2D collision)
    {
        var damageable = collision.gameObject.GetComponent<IDamageable>();
        if (damageable != null)
        {
            if (damageable.PlayerType == targetType)
            {
                damageable.TakeDamage(damageValue * Time.deltaTime);
            }
        }
    }

    public void TakeDamage(float damage)
    {
        if (currentHealth <= 0f) return;
        currentHealth -= damage;
        //GameObject vfxObject = Instantiate(enemyHitVFX, transform.position, Quaternion.identity);
        if (currentHealth <= 0)
        {
            Death();
        }
    }

    public void Death()
    {
        GameManager.Instance.UnregisterEnemy(this);

        GameObject vfx = Instantiate(enemyDeathVFX, transform.position, Quaternion.identity);
        vfx.transform.parent = null;
        MonoBehaviour.Destroy(vfx, 2f);
        OnEnemyDeath?.Invoke(this);
        bool spawnUpgrade = WeaponUpgradeLootTable.Instance.ShouldSpawnUpgrade();
        if (spawnUpgrade)
        {
            Debug.Log("call Drop upgrade");
            WeaponUpgradeLootTable.Instance.SpawnUpgrade(transform.position);
        }
        MonoBehaviour.Destroy(this.gameObject);
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }
}

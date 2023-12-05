using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int maxEnmeies = 200;
    public int enemiesKilled = 0;
    public int soulsToChargePortal = 200;
    public int mercyUpgradeTime = 40;
    public float FXCooldown = 2f;

    [Header("References")]
    [SerializeField] TMPro.TextMeshProUGUI SoulChargeStatus;
    [SerializeField] GameObject PortalDamageFX;
    [SerializeField] GameObject PlayerDamageFX;
    [SerializeField] GameObject LooseScreen;
    [SerializeField] GameObject WinScreen;
    [SerializeField] PortalManager ExitPortal;

    //Collections
    private List<Spawner> Spawners { get; set; }
    private List<GameObject> Enemies { get; set; }
    
    private bool canSpawnPlayerFX = true;
    private bool canSpawnPortalFX = true;
    private bool gameIsOver = false;

    private void Awake()
    {
        Spawners = new List<Spawner>();
        Enemies = new List<GameObject>();

        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    void Start()
    {
        ExitPortal.OnPortalDamaged += ExitPortal_OnPortalDamaged;
        ExitPortal.OnPortalDead += ExitPortal_OnPortalDead;
        ExitPortal.OnPortalEnter += OnPortalEnter;
        PlayerHealth.OnPlayerDeath += PlayerHealth_OnPlayerDeath;
        PlayerHealth.OnPlayerDamageTaken += PlayerHealth_OnPlayerDamageTaken;
    }
    private void Update()
    {
        var percentage2 = Mathf.Round(Mathf.Clamp01((float)enemiesKilled / soulsToChargePortal) * 100);
        SoulChargeStatus.text = $"{percentage2}%";
    }
    private void PlayerHealth_OnPlayerDeath(Vector3 vector)
    {
        GameLost();
    }

    private void PlayerHealth_OnPlayerDamageTaken(Vector3 vector)
    {
        if (!canSpawnPlayerFX) return;
        vector.z = -1;
        GameObject fx = Instantiate(PlayerDamageFX, vector, Quaternion.identity);
        Destroy(fx, 2);
        canSpawnPlayerFX = false;
        StartCoroutine(FXTimer(FXCooldown, () => canSpawnPlayerFX = true));
    }
    private void ExitPortal_OnPortalDamaged(Vector3 vector)
    {
        if (!canSpawnPortalFX) return;
        vector.z = -1;
        GameObject fx = Instantiate(PortalDamageFX, vector, Quaternion.identity);
        Destroy(fx, 2);
        canSpawnPortalFX = false;
        StartCoroutine(FXTimer(FXCooldown, () => canSpawnPortalFX = true));
    }

    private void ExitPortal_OnPortalDead(Vector3 vector)
    {
        GameLost();
    }
    IEnumerator FXTimer (float timeToWait, Action enableFX)
    {
        yield return new WaitForSeconds(timeToWait);
        enableFX?.Invoke();
    }

    #region Spawner Management
    public void RegisterSpawner(Spawner spawner)
    {
        Spawners.Add(spawner);
    }
    #endregion

    #region Enemy Management
    public void RegisterEnemy(GameObject enemy)
    {
        Enemies.Add(enemy);
        if (Enemies.Count >= maxEnmeies)
        {
            foreach (var spawner in Spawners)
            {
                spawner.SetSpawnState(false);
            }
        }
        if (gameIsOver)
        {
            var enemyHealth = enemy.GetComponent<Enemy>();
            enemyHealth.TakeDamage(enemyHealth.maxHealth + 1f);
        }
        //enemy.GetComponent<Enemy>().OnEnemyDeath += UnregisterEnemy;
    }
    public void UnregisterEnemy(Enemy enemy)
    {
        enemiesKilled++;

        if (Enemies == null)
        {
            Debug.LogError("Enemy is null");
            return;
        }
        Enemies.Remove(enemy.gameObject);
        if (Enemies.Count < maxEnmeies)
        {
            foreach (var spawner in Spawners)
            {
                spawner.SetSpawnState(true);
            }
        }
        //EnemyKilled
        PortalManager.Instance.SetChargePercentage( Mathf.Clamp01(enemiesKilled/soulsToChargePortal));
    }
    #endregion

    private void OnPortalEnter(GameObject entered)
    {
        if (gameIsOver) return;
        if (entered.CompareTag("Player"))
        {
            if (enemiesKilled >= soulsToChargePortal)
            {
                GameWon();
            }
        }
    }

    private void GameWon()
    {
        gameIsOver = true;
        ExitPortal.OnPortalEnter -= OnPortalEnter;
        WinScreen.SetActive(true);
        for (int i = 0; i < Spawners.Count; i++)
        {
            Spawners[i].SetSpawnState(false);
            Debug.Log($"Spawner {Spawners[i].name} stopped");
        }
        for (int i = 0; i < Enemies.Count; i++)
        {
            if (Enemies[i] == null) continue;
            var enemy = Enemies[i].GetComponent<Enemy>();
            Debug.Log($"Enemy {Enemies[i].name} killed");
            Destroy(Enemies[i]);
            //enemy.TakeDamage(Mathf.Round(enemy.GetCurrentHealth()) + 1f);
        }
    }
    private void GameLost()
    {
        if (gameIsOver) return;
        LooseScreen.SetActive(true);
        gameIsOver = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class EnemyManager : MonoBehaviour
{
    [System.Serializable]
    public struct WaveEnemyData
    {
        public PoolableGameObject Enemy;
        public int Amount;
    }

    [System.Serializable]
    public struct WaveData
    {
        public List<WaveEnemyData> Enemies;

        public int TotalEnemies
        {
            get
            {
                int count = 0;
                foreach(WaveEnemyData data in Enemies)
                {
                    count += data.Amount;
                }

                return count;
            }
        }

        public int GetEnemy(int index)
        {
            int count = 0;
            foreach(WaveEnemyData data in Enemies)
            {
                if (index < count + data.Amount)
                {
                    return data.Enemy.ID;
                }
                else
                {
                    count += data.Amount;
                }
            }

            return 0;
        }
    }

    public float SpawnRadius = 50;
    public float SpawnCircleRadius = 5;
    public WaveData[] Waves;
    public GameObject PlayerBaseTarget;
    public PoolableGameObject SpawnCircle;
    public int MaxEnemiesPerCircle = 5;

    [Space, Header("Timings")]
    public float SpawnCircleWarmUpTime = 1f;
    public float TimeBetweenSpawnCircles = 5f;

    [SerializeField] AudioSource enemySpawnSFX;

    private Coroutine m_waveSpawnCoroutine;
    private List<BattleUnit> m_enemiesAlive = new();

    private void Awake()
    {
        GameEvents.WaveStarted += OnWaveStarted;
        GameEvents.GameOver += OnGameOver;
    }

    private void OnDestroy()
    {
        GameEvents.WaveStarted -= OnWaveStarted;
        GameEvents.GameOver -= OnGameOver;
    }

    private void Start()
    {
        //CursorManager.instance.UseCustomCursor("");
    }

    IEnumerator SpawnNewWave(int day)
    {
        WaveData selectedWave = Waves[day];

        int totalEnemies = selectedWave.TotalEnemies;
        List<int> enemiesLeftToSpawn = new();
        for(int n=0; n<totalEnemies; ++n)
        {
            enemiesLeftToSpawn.Add(n);
        }

        while (enemiesLeftToSpawn.Count > 0)
        {
            int numberPerCircle = Mathf.Min(enemiesLeftToSpawn.Count, Random.Range(1, MaxEnemiesPerCircle+1));
            Vector3 spawnCircle = transform.position + Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.up) * new Vector3(SpawnRadius, 0f, 0f);
            float rotPerEnemy = 360f / (float)numberPerCircle;

            PoolableGameObject circle = GameObjectPooler.Instance.FetchFromPool(SpawnCircle.ID);
            circle.transform.position = spawnCircle;
            yield return new WaitForSeconds(SpawnCircleWarmUpTime);

            for(int e=0; e<numberPerCircle; ++e)
            {
                Vector3 spawnPosition = spawnCircle + Quaternion.AngleAxis(rotPerEnemy * e, Vector3.up) * new Vector3(SpawnCircleRadius, 0f, 0f);
                int enemyIndex = Random.Range(0, enemiesLeftToSpawn.Count);
                int enemyID = (int)selectedWave.GetEnemy(enemiesLeftToSpawn[enemyIndex]);
                PoolableGameObject pgo = GameObjectPooler.Instance.FetchFromPool(enemyID);
                pgo.transform.position = spawnPosition;
                BattleUnit bu = pgo.GetComponent<BattleUnit>();
                bu.SetDestination(PlayerBaseTarget.transform.position);
                bu.OnDeath += OnEnemyDefeated;
                m_enemiesAlive.Add(bu);

                enemiesLeftToSpawn.RemoveAt(enemyIndex);
            }

            yield return new WaitForSeconds(TimeBetweenSpawnCircles);
        }

        while(m_enemiesAlive.Count > 0)
        {
            yield return null;
        }

        GameEvents.SendWaveFinished(DayManager.Day);
    }

    private void OnWaveStarted(int day)
    {
        m_waveSpawnCoroutine = StartCoroutine(SpawnNewWave(day));
    }

    private void OnGameOver(int day)
    {
        if (m_waveSpawnCoroutine != null)
        {
            StopCoroutine(m_waveSpawnCoroutine);
        }
    }

    private void OnEnemyDefeated(Damageable attackable)
    {
        BattleUnit bu = attackable as BattleUnit;
        bu.OnDeath -= OnEnemyDefeated;
        m_enemiesAlive.Remove(bu);
    }

    //private void OnDrawGizmos()
    //{
    //    Handles.color = Color.red;
    //    Handles.DrawWireDisc(transform.position, Vector3.up, SpawnRadius);
    //}
}

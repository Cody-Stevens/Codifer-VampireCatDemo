using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public bool IsSpawning { get; set; }
    public GameObject[] PrefabsToSpawn { get; set; }
    public int SpawnTimeInterval { get; set; }
    private float spawnTimer = 0;
    [Header("References")]
    [SerializeField] GameObject[] defaultPrefabToSpawn;

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => GameManager.Instance != null);
        GameManager.Instance.RegisterSpawner(this);
        SetPrefabArray();
        IsSpawning = true;
        SpawnTimeInterval = UnityEngine.Random.Range(5, 10);
        do
        {
            SpawnTimeInterval = (int)(SpawnTimeInterval * .75);
            yield return new WaitForSeconds(30);
        } while (true);
    }

    private void FixedUpdate()
    {
        if (IsSpawning)
        {
            spawnTimer += Time.fixedDeltaTime;
            if (spawnTimer >= SpawnTimeInterval)
            {
                spawnTimer = 0;
                Spawn();
            }
        }
    }

    private void Spawn() // add object pooler
    {
        GameObject e = Instantiate(PrefabsToSpawn[UnityEngine.Random.Range(0, PrefabsToSpawn.Length)], transform.position, Quaternion.identity);
        GameManager.Instance.RegisterEnemy(e);
    }

    private void SetPrefabArray()
    {
        if (PrefabsToSpawn == null)
        {
            PrefabsToSpawn = defaultPrefabToSpawn;
        }
        else if (PrefabsToSpawn.Length == 0)
        {
            PrefabsToSpawn = defaultPrefabToSpawn;
        }
    }

    public void SetSpawnState(bool spawning, int spawnTimeInterval = -1, GameObject[] newPrefabArray = null)
    {
        IsSpawning = spawning;
        if (spawnTimeInterval == -1) return;
        SpawnTimeInterval = spawnTimeInterval;
    }

    public void SpawnOverride(int amountToSpawn, GameObject[] newPrefabArray = null)
    {
        if (newPrefabArray != null) PrefabsToSpawn = newPrefabArray;
        SetPrefabArray();
        for (int i = 0; i < amountToSpawn; i++)
        {
            Spawn();
        }
    }
}

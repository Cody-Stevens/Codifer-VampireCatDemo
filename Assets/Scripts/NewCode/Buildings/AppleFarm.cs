using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class AppleFarm : ServiceBuilding
{
    public int ApplesToSpawn = 3;
    public PoolableGameObject Apple;
    public Transform SpawnPositionBase;
    public float SpawnRadius;

    public override void Upgrade()
    {
        base.Upgrade();
        ApplesToSpawn++;
    }

    public override void Generate()
    {
        base.Generate();

        for(int n=0; n<ApplesToSpawn; ++n)
        {
            PoolableGameObject a = GameObjectPooler.Instance.FetchFromPool(Apple.ID);
            a.transform.position = SpawnPositionBase.position + Quaternion.AngleAxis(Random.Range(0,360), Vector3.up) * new Vector3(SpawnRadius, 0f, 0f);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.DrawWireDisc(SpawnPositionBase.position, Vector3.up, SpawnRadius);
    }
    #endif
}

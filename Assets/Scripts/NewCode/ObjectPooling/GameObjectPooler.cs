using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPooler : MonoBehaviour
{
    public static GameObjectPooler Instance;

    public class ObjectPool
    {
        public PoolableGameObject Prefab;
        public Transform PoolHolder;
        public Queue<PoolableGameObject> Pool;
        public List<PoolableGameObject> Active = new();

        public ObjectPool(PoolData data, Transform root)
        {
            Prefab = data.Prefab;
            PoolHolder = new GameObject(Prefab.name).transform;
            PoolHolder.SetParent(root);

            Pool = new();
            for(int n=0; n<data.StartAmount; ++n)
            {
                PoolableGameObject newPGO = Instantiate(Prefab, PoolHolder);
                Pool.Enqueue(newPGO);
                newPGO.gameObject.SetActive(false);
            }
        }

        public PoolableGameObject FetchFromPool()
        {
            if (Pool.Count == 0)
            {
                PoolableGameObject newPGO = Instantiate(Prefab);
                Active.Add(newPGO);
                newPGO.Active = false;
                return newPGO;
            }

            PoolableGameObject pgo = Pool.Dequeue();
            pgo.transform.parent = null;
            pgo.gameObject.SetActive(true);
            pgo.Active = true;
            return pgo;
        }

        public void ReturnToPool(PoolableGameObject pgo)
        {
            Active.Remove(pgo);
            Pool.Enqueue(pgo);
            pgo.Active = false;
            pgo.transform.parent = PoolHolder;
            pgo.gameObject.SetActive(false);
        }
    }

    [System.Serializable]
    public struct PoolData
    {
        public PoolableGameObject Prefab;
        public int StartAmount;

        public PoolData(PoolableGameObject pgo, int amount=10)
        {
            Prefab = pgo;
            StartAmount = amount;
        }

        public int ID => Prefab.ID;
    }

    [SerializeField] public PoolData[] Pools;
    private Dictionary<int, ObjectPool> m_pools = new();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        for(int n=0; n<Pools.Length; ++n)
        {
            if (m_pools.ContainsKey(Pools[n].ID))
            {
                Debug.LogWarning($"Clashing PoolableGameObject IDs {Pools[n].ID}");
            }

            m_pools[Pools[n].ID] = new ObjectPool(Pools[n], transform);
        }
    }

    public PoolableGameObject FetchFromPool(int id)
    {
        if (!m_pools.ContainsKey(id))
        {
            Debug.LogWarning($"Pool doesn't exist for ID {id}");
            return null;
        }

        return m_pools[id].FetchFromPool();
    }

    public void ReturnToPool(PoolableGameObject pgo)
    {
        ObjectPool pool = m_pools[pgo.ID];
        pool.ReturnToPool(pgo);
    }
}

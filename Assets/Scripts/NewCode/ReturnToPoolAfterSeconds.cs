using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToPoolAfterSeconds : MonoBehaviour
{
    public float SecondsUntilDisabled = 1f;

    private float m_time;
    private PoolableGameObject m_poolable;

    private void Awake()
    {
        m_poolable = GetComponent<PoolableGameObject>();
    }

    private void OnEnabled()
    {
        m_time = 0f;
    }

    private void Update()
    {
        m_time += Time.deltaTime;
        if (m_time >= SecondsUntilDisabled)
        {
            m_poolable.ReturnToPool();
        }
    }
}

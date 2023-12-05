using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolableGameObject : MonoBehaviour
{
    [ObjectID]
    public int ID;
    public bool Active;

    public void Retrieve(Vector3 position)
    {

    }

    public void ReturnToPool()
    {
        GameObjectPooler.Instance.ReturnToPool(this);
    }
}

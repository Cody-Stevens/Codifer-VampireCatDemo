using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public int numberOfSpawns;
    public float spawnAreaSize;
    public float groundCheckRadius;

    void Start()
    {
        SpawnObjects();
    }

    private void SpawnObjects()
    {
        for (int i = 0; i < numberOfSpawns; i++)
        {
            Vector3 spawnPosition = GetRandomPosition();
            //Instantiate(GameObject.CreatePrimitive(PrimitiveType.Sphere), spawnPosition, Quaternion.identity);
            
            Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity, transform);
            
        }
    }

    private Vector3 GetRandomPosition()
    {
        float x = Random.Range(-spawnAreaSize / 2, spawnAreaSize / 2);
        float y = Random.Range(-spawnAreaSize / 2, spawnAreaSize / 2);
        float z = 0; // Assuming y is constant; adjust if necessary

        return transform.position + new Vector3(x, y, z);
    }

    //private bool IsNearGround(Vector3 position)
    //{
    //    // Assuming '4' is the layer mask for the "Ground" layer
    //    int groundLayerMask = 1 << 4;

    //    // Get all colliders that overlap with the circle at 'position' within 'groundCheckRadius'
    //    Collider2D[] colliders = Physics2D.OverlapCircleAll((Vector2)position, groundCheckRadius, groundLayerMask);

    //    foreach (Collider2D collider in colliders)
    //    {
    //        if (collider != null && collider.gameObject.CompareTag("Ground"))
    //        {
    //            return true;
    //        }
    //    }
    //    return false;
    //}
}

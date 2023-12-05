using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSource : MonoBehaviour
{
    public float timeToSpawnFood = 1f;
    public int numFoodsDropped = 3;
    public bool active = false;
    public GameObject food;

    private float foodDropTimer = 1f;
    
    private void Start()
    {
      
    }

    IEnumerator SpawnFood()
    {
        while (gameObject.activeSelf)
        {

            for (int i = 0; i < numFoodsDropped; i++)
            {

                Instantiate(food, CityManager.Instance.foodObjects.transform);

                food.transform.position = GetRandomNearbyPosition();

            }
            yield return new WaitForSeconds(timeToSpawnFood * CityManager.timeMultiplier);
        }

    }

    private void Update()
    {
        foodDropTimer -= Time.deltaTime;

        if (foodDropTimer <= 0)
        {
            for (int i = 0; i < numFoodsDropped; i++)
            {

                Instantiate(food, CityManager.Instance.foodObjects.transform);
                food.transform.position = GetRandomNearbyPosition();

            }
            foodDropTimer = timeToSpawnFood;
        }
    }



    private Vector3 GetRandomNearbyPosition()
    {

        int randomRangeX = Random.Range(-3, 3);
        int randomRangeY = Random.Range(-3, 3);

        return new Vector3(transform.position.x+ randomRangeX, transform.position.y+ randomRangeY);
    }

}

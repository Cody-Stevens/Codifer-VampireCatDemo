using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrol : MonoBehaviour
{
    [SerializeField] private float minExploreRadius = 5f;
    [SerializeField] private float maxExploreRadius = 10f;
    [SerializeField] private float minWaitTime = 1f;
    [SerializeField] private float maxWaitTime = 5f;

    [SerializeField] private PatrolState patrolState = PatrolState.PickingTarget;
    public Vector2 nextPosition;
    private float reachedTargetTime = 0;
    private float randomWaitTime;

    private NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.isActiveAndEnabled)
        {
            switch (patrolState)
            {
                case PatrolState.PickingTarget:
                    // find next target position in radius
                    nextPosition = GenerateRandomPointInCircle(transform.position, minExploreRadius, maxExploreRadius);
                    // set nav mesh agent to move to position
                    agent.SetDestination(nextPosition);
                    patrolState = PatrolState.MovingToTarget;
                    break;

                case PatrolState.MovingToTarget:
                    // check if have reached target
                    if (agent.remainingDistance == 0) patrolState = PatrolState.ReachedTarget;
                    break;


                case PatrolState.ReachedTarget:
                    // record time
                    reachedTargetTime = Time.time;
                    // pick random time to wait
                    randomWaitTime = Random.Range(minWaitTime, maxWaitTime);
                    // move to StandinAround 
                    patrolState = PatrolState.StandinAround;
                    break;


                default:
                    // check if time elapsed greater than random wait time
                  //  if (Time.time - reachedTargetTime > randomWaitTime) patrolState = PatrolState.PickingTarget;
                    break;

            }
        }

    }

    /// <summary>
    /// go to point in circle near a position
    /// </summary>
    /// <param name="pos"></param>
    public void GoToSpecificPoint(Vector3 pos)
    {
     
        nextPosition = GenerateRandomPointInCircle(pos,minExploreRadius,maxExploreRadius);
        patrolState = PatrolState.MovingToTarget;
        agent.SetDestination(nextPosition);

    }

    public void BeginPatrolling()
    {
        patrolState = PatrolState.PickingTarget;
    }
    public void StopPatrolling()
    {
        patrolState = PatrolState.PickingTarget;
    }

    private Vector2 GenerateRandomPointInCircle(Vector2 centerPoint, float minRadius, float maxRadius)
    {
        float randomRadius = Random.Range(minRadius, maxRadius);
        float randomAngle = Random.Range(0f, 360f) * Mathf.Deg2Rad;

        float x = centerPoint.x + randomRadius * Mathf.Cos(randomAngle);
        float y = centerPoint.y + randomRadius * Mathf.Sin(randomAngle);

        return new Vector2(x, y);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, minExploreRadius);
        Gizmos.DrawWireSphere(transform.position, maxExploreRadius);
        Gizmos.DrawSphere(nextPosition, 0.5f);
    }

    private enum PatrolState
    {
        MovingToTarget,
        ReachedTarget,
        PickingTarget,
        StandinAround
    }
}

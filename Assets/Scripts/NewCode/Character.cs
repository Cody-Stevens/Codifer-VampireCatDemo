using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class Character : MonoBehaviour
{
    public StatsWrapper Wrapper;
    public Stats stats
    {
        get { return Wrapper.Stats; }
        set { Wrapper.Stats = value; }
    }
    public bool isBattling = false;
    public HashSet<Character> opponents;
    const float attackTimeMultiplier = 3;

    [SerializeField] ParticleSystem enemyHitVFX;

    // public bool isAlive = true;
    private void Awake()
    {
        //GetStats();
        opponents = new HashSet<Character>();
    }

    private void Start()
    {

    }

    /*
    protected void GetStats()
    {
        if (stats == null)
        {
            stats = GetComponentInChildren<Stats>();
        }
    }

    public IEnumerator DealDamageOverTime()
    {
        if (opponents == null)
            opponents = new HashSet<Character>();

        while (true)
        {
            foreach(Character op in opponents.ToList())
            {
                DealDamage(op, (int)stats.strength);
            }
            
            yield return new WaitForSeconds((attackTimeMultiplier* CityManager.timeMultiplier)/stats.speed);
        }
    }

    public virtual void GetDead()
    {
        StopAllCoroutines();
        Destroy(gameObject);
        
        CityManager.Instance.HandleStats();
    }
    */
}

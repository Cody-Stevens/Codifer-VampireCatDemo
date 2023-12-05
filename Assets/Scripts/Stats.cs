using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "AttackerStats", fileName = "Character.STATS.asset")]
public class Stats : ScriptableObject
{
    public int id;
    public bool isAdult = false;
    [FormerlySerializedAs("endurance")]
    public float timebetweenattacks = 5;
    /// <summary>
    ///  the rate of each tick
    /// </summary>
    public float speed = 5;
    /// <summary>
    ///  Represents how much damage is done per tick
    /// </summary>
    public float strength = 5;
    public int coinValue = 10;
    public int maxHealth = 10;
}

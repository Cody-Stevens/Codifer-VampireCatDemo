using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    public static int damageValue = 1;
    public static float timeForDamageDealt = 2;

    public List<BuffTypes> buffs;



}

public enum BuffTypes
{
    circleFlame=0,
    heatseeking=1,


}

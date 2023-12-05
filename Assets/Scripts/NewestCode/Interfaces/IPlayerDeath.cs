using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerDeath
{
    static bool IsAlive { get; set; } = true;
    static Action OnPlayerDeath { get; set; }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panos : RangedUnit
{
	// Start is called before the first frame update
    void Awake()
    {
        base.Start();
        _name = "Vurius";
        SetAnimBools(IDLE);
        SetStats();
        isEnemy = true;
    }

    void SetStats()
    {
        speed = 22;
        reaction = 21;
    }
}

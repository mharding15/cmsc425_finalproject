using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vurius : RangedUnit
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
        speed = 9;
        reaction = 20;
        hp = 17;
        ac = 2;

        meleeDamage = 2;
        rangedDamage = 3;
    }
}

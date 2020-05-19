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
        speed = 13;
        reaction = 20;
        hp = 11;
        ac = 6;

        meleeDamage = 3;
        rangedDamage = 2;
    }
}

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
        isEnemy = false;
    }

    void SetStats()
    {
        speed = 12;
        reaction = 21;
        hp = 13;
        ac = 10;

        meleeDamage = 3;
        rangedDamage = 2;
    }
}

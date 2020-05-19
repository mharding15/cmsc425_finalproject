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
        speed = 14;
        reaction = 21;
        hp = 18;
        ac = 12;

        meleeDamage = 3;
        rangedDamage = 2;
    }
}

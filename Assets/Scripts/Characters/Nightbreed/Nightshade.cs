using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nightshade : RangedUnit
{
	// Start is called before the first frame update
    void Awake()
    {
        base.Start();
        _name = "Nightshade";
        SetAnimBools(IDLE);
        SetStats();
        isEnemy = true;
    }

    void SetStats()
    {
        speed = 17;
        reaction = 22;
        hp = 16;
        ac = 11;

        meleeDamage = 4;
        rangedDamage = 2;
    }
}

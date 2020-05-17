using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ganfaul : Unit
{
	// Start is called before the first frame update
    void Awake()
    {
        base.Start();
        _name = "Ganfaul";
        SetAnimBools(IDLE);
        SetStats();
        isEnemy = false;
    }

    void SetStats()
    {
        speed = 8;
        reaction = 18;
        hp = 22;
        ac = 2;

        meleeDamage = 4;
        // cunning;
        // perception;
        // reaction;
        // speed;
        // strength;
        // will;
    }

}

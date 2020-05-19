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
        isEnemy = true;
    }

    void SetStats()
    {
        speed = 22;
        reaction = 18;
        hp = 18;
        ac = 7;

        meleeDamage = 5;
    }

}

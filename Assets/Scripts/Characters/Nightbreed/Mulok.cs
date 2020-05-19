using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mulok : Unit
{
	// Start is called before the first frame update
    void Awake()
    {
        base.Start();
        _name = "Mulok";
        SetAnimBools(IDLE);
        SetStats();
        isEnemy = true;
    }

    void SetStats()
    {
        speed = 19;
        reaction = 18;
        hp = 18;
        ac = 5;

        meleeDamage = 6;
    }
    
}

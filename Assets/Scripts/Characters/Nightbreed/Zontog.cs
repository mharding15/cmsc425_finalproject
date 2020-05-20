﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zontog : Unit
{
	// Start is called before the first frame update
    void Awake()
    {
        base.Start();
        _name = "Zontog";
        SetAnimBools(IDLE);
        SetStats();
        isEnemy = true;
    }

    void SetStats()
    {
        speed = 21;
        reaction = 18;

        hp = 14;
        ac = 5;

        meleeDamage = 6;
    }
}

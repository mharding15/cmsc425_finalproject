using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bruno : Unit
{
    // Start is called before the first frame update
    void Awake()
    {
        base.Start();
        _name = "Bruno";
        SetAnimBools(IDLE);
        SetStats();
        isEnemy = false;
    }

    void SetStats()
    {
        speed = 9;
        reaction = 18;
        hp = 25;
        ac = 8;

        meleeDamage = 6;
    }
   
}

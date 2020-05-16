using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrok : Unit
{
	// Start is called before the first frame update
    void Awake()
    {
        base.Start();
        _name = "Warrok";
        SetAnimBools(IDLE);
        SetStats();
        isEnemy = true;
    }

    void SetStats()
    {
        speed = 10;
        reaction = 18;
    }

}

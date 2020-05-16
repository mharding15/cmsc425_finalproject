using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maria : Unit
{
	// Start is called before the first frame update
    void Awake()
    {
        base.Start();
        _name = "Maria";
        SetAnimBools(IDLE);
        SetStats();
        isEnemy = false;
    }

    void SetStats()
    {
        speed = 25;
        reaction = 18;
    }
    
}

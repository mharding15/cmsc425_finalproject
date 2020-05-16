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
        print("setting speeed to 20 for Bruno");
        speed = 20;
        reaction = 18;
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}

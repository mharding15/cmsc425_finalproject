using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// these stats can be moved into another script if we start having two many scripts. I just like keeping everything separated.
public class ErikaStats : MonoBehaviour
{
	// I'm assuming these will be modified based on what class the user pics for this character
	public static int hp;
	public static int ac;

	public static int cunning;
	public static int perception;
	public static int reaction = 15;
	public static int speed = 30;
	public static int strength;
	public static int will;

	public static bool isEnemy = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

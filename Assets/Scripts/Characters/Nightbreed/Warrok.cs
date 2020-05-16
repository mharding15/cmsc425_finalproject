using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrok : MonoBehaviour
{
	// *** STATS *** //

	// I'm assuming these will be modified based on what class the user pics for this character
	public static int hp;
	public static int ac;

	public static int cunning;
	public static int perception;
	public static int reaction = 10;
	public static int speed = 18;
	public static int strength;
	public static int will;

	public static bool isEnemy = true;

	// *** OTHER VARIABLES *** //

    private string _name = "Warrok";
    private Animator _animator;
    private bool _isIdle, _isWalking, _isRunning, _isAttacking, _isDying, _isHit;
    private int IDLE = 0, 
                WALK = 1,
                RUN = 2, 
                ATTACK = 3,
                DIE = 4,
                HIT = 5;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        SetAnimBools(IDLE);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // *** ACTIONS *** //

    public void Move()
    {
    	// to be filled in with movement code
    	print(_name + " is moving...");
    }

    public void MeleeAttack()
    {
    	// basically just call the attack animation
    	SetAnimBools(ATTACK);
    	// will probably need to set it back to IDLE here, but will test that later
    		// might have to have a set delay time or something, although probably not since the animation has exit time
    }

    // *** ANIMATIONS *** //

    void SetAnimBools(int state)
    {
        SetAllToFalse();

        switch(state){
            case 0:
                _isIdle = true;
                break;
            case 1:
                _isWalking = true;
                break;
            case 2:
                _isRunning = true;
                break;
            case 3:
                _isAttacking = true;
                break;
            case 4:
                _isDying = true;
                break;
            case 5:
                _isHit = true;
                break;
        }

        _animator.SetBool("isIdle", _isIdle);
        _animator.SetBool("isWalking", _isWalking);
        _animator.SetBool("isRunning", _isRunning);
        _animator.SetBool("isAttacking", _isAttacking);
        _animator.SetBool("isDying", _isDying);
        _animator.SetBool("isHit", _isHit);
    }

    void SetAllToFalse()
    {
        _isIdle = false;
        _isWalking = false;
        _isRunning = false;
        _isAttacking = false;
        _isDying = false;
        _isHit = false;
    }

}

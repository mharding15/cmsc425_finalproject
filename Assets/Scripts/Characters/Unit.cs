using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    // *** STATS *** //

	// I'm assuming these stats will be modified based on what class the user pics for this character
	public int hp;
	public int ac;

	public int cunning;
	public int perception;
	public int reaction;
	public int speed;
	public int strength;
	public int will;

	public bool isEnemy;

	// *** OTHER VARIABLES *** //

	protected string _name;
	protected Animator _animator;
	protected bool _isIdle, _isWalking, _isRunning, _isMelee, _isDying, _isHit;
    protected int IDLE = 0, 
                WALK = 1,
                RUN = 2, 
                MELEE = 3,
                DIE = 4,
                HIT = 5;

    // Start is called before the first frame update
    protected void Start()
    {
    	_animator = GetComponent<Animator>();
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
    	transform.Translate(new Vector3(1f, 0f, 2f));
    }

    public void MeleeAttack()
    {
    	// basically just call the attack animation
    	SetAnimBools(MELEE);
    	// will probably need to set it back to IDLE here, but will test that later
    		// might have to have a set delay time or something, although probably not since the animation has exit time
    }

    // *** ANIMATIONS *** //

     // this method can be called from outside of this script to set the animation for the character
    public void SetAnimBools(int state)
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
                _isMelee = true;
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
        _animator.SetBool("isMelee", _isMelee);
        _animator.SetBool("isDying", _isDying);
        _animator.SetBool("isHit", _isHit);
    }

    protected void SetAllToFalse()
    {
        _isIdle = false;
        _isWalking = false;
        _isRunning = false;
        _isMelee = false;
        _isDying = false;
        _isHit = false;
    }
}

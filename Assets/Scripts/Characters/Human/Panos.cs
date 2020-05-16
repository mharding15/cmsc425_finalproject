using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panos : MonoBehaviour
{
	// *** STATS *** //

	// I'm assuming these will be modified based on what class the user pics for this character
	public static int hp;
	public static int ac;

	public static int cunning;
	public static int perception;
	public static int reaction = 13;
	public static int speed = 20;
	public static int strength;
	public static int will;
	
	public static bool isEnemy = false;

	// *** OTHER VARIABLES *** //

	public GameObject flames;

    private string _name = "Panos";
	private Animator _animator;
    private bool _isIdle, _isWalking, _isRunning, _isAttacking, _isDying, _isHit, _isMelee;
    private int IDLE = 0, 
                WALK = 1,
                RUN = 2, 
                ATTACK = 3,
                DIE = 4,
                HIT = 5,
                MELEE = 6;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        SetAnimBools(IDLE, new Vector3(0f,0f,0f));
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

    public void RangedAttack(Vector3 pos)
    {
    	SetAnimBools(ATTACK, pos);
    }

    public void MeleeAttack()
    {
    	// basically just call the attack animation
    	SetAnimBools(MELEE, new Vector3(0f,0f,0f));
    	// will probably need to set it back to IDLE here, but will test that later
    		// might have to have a set delay time or something, although probably not since the animation has exit time
    }

    // *** ANIMATIONS *** //

    // this method can be called from outside of this script to set the animation for the character
    public void SetAnimBools(int state, Vector3 pos)
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
                CreateFlames(pos);
                break;
            case 4:
                _isDying = true;
                break;
            case 5:
                _isHit = true;
                break;
            case 6:
                _isMelee = true;
                break;
        }

        _animator.SetBool("isIdle", _isIdle);
        _animator.SetBool("isWalking", _isWalking);
        _animator.SetBool("isRunning", _isRunning);
        _animator.SetBool("isAttacking", _isAttacking);
        _animator.SetBool("isDying", _isDying);
        _animator.SetBool("isHit", _isHit);
        _animator.SetBool("isMelee", _isMelee);
    }

    void SetAllToFalse()
    {
        _isIdle = false;
        _isWalking = false;
        _isRunning = false;
        _isAttacking = false;
        _isDying = false;
        _isHit = false;
        _isMelee = false;
    }

    public void CreateFlames(Vector3 pos)
    {
        Instantiate(flames, pos, Quaternion.identity);
    }
}

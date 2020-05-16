using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Erika : RangedUnit
{
	public GameObject arrow_prefab;

    private bool _isDelaying;

    void Awake()
    {
        base.Start();
        SetAnimBools(IDLE);
        _isDelaying = false;
        SetStats();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetStats()
    {
        speed = 22;
        reaction = 21;
    }

    // *** ACTIONS *** //

    public void Move()
    {
    	// to be filled in with movement code
    	print(_name + " is moving...");
    }

    new public void RangedAttack()
    {
    	SetAnimBools(RANGED_ATTACK);
    }

    // *** ANIMATIONS *** //

    // this method can be called from outside of this script to set the animation for the character
    new public void SetAnimBools(int state)
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
            case 6:
                _isRanged = true;
                ShootArrow();
                break;
        }

        _animator.SetBool("isIdle", _isIdle);
        _animator.SetBool("isWalking", _isWalking);
        _animator.SetBool("isRunning", _isRunning);
        _animator.SetBool("isMelee", _isMelee);
        _animator.SetBool("isDying", _isDying);
        _animator.SetBool("isHit", _isHit);
        _animator.SetBool("isRanged", _isRanged);
    }

    void ShootArrow()
    {
        if (!_isDelaying){
            StartCoroutine(Delay());
        }
    }

    IEnumerator Delay()
    {
        _isDelaying = true;
        //yield on a new YieldInstruction that waits for 2.5 seconds.
        yield return new WaitForSeconds(2.5f);
        GameObject arrow = Instantiate(arrow_prefab, new Vector3(transform.position.x + .25f, transform.position.y + 1.475f, transform.position.z), transform.rotation);
        Rigidbody arrow_rb = arrow.GetComponent<Rigidbody>();
        //yield again for .5 seconds because the archer pauses before actually shooting the arrow
        yield return new WaitForSeconds(.5f);
        arrow_rb.velocity = transform.forward * 10f;
        _isDelaying = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedUnit : Unit
{
	protected bool _isRanged;
	protected int RANGED_ATTACK = 6;

	public GameObject spellEffect;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        longRange = 12f;
        SetAnimBools(IDLE, new Vector3(0f,0f,0f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    new public void MeleeAttack()
    {
    	SetAnimBools(MELEE, new Vector3(0f,0f,0f));
    }

    public void RangedAttack(Vector3 pos)
    {
    	SetAnimBools(RANGED_ATTACK, pos);
    }

    new void SetAnimBools(int state, Vector3 pos)
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
                CreateEffect(pos);
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

    new void SetAllToFalse()
    {
    	base.SetAllToFalse();
    	_isRanged = false;
    }

    void CreateEffect(Vector3 pos)
    {
    	Instantiate(spellEffect, pos, Quaternion.identity);
    }
}

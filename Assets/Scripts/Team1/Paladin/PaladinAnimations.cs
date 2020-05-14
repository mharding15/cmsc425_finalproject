using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaladinAnimations : MonoBehaviour
{
    public GameObject flames;

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
        if (Input.GetKey(KeyCode.UpArrow)){
            SetAnimBools(WALK);
        } else if (Input.GetKey(KeyCode.W)){
            SetAnimBools(RUN);
        } else if (Input.GetKey(KeyCode.S)){
            SetAnimBools(ATTACK);
            CreateFlames(new Vector3(0f, 0f, 0f));
        // this will be if the character's HP goes to 0 in the actual game
        } else if (Input.GetKey(KeyCode.D)){
            SetAnimBools(DIE);
        } else if (Input.GetKey(KeyCode.H)){
            SetAnimBools(HIT);
        } else {
            SetAnimBools(IDLE);
        }
    }

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

    public void CreateFlames(Vector3 pos)
    {
        Instantiate(flames, pos, Quaternion.identity);
    }

}

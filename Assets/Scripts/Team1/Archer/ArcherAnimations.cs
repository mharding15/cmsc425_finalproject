using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherAnimations : MonoBehaviour
{
    public GameObject arrow_prefab;
    private Animator _animator;
    private bool _isIdle, _isWalking, _isRunning, _isShooting, _isDying, _isHit, _isDelaying;
    private int IDLE = 0, 
                WALK = 1,
                RUN = 2, 
                SHOOT = 3,
                DIE = 4,
                HIT = 5;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        SetAnimBools(IDLE);
        _isDelaying = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow)){
            SetAnimBools(WALK);
        } else if (Input.GetKey(KeyCode.W)){
            SetAnimBools(RUN);
        } else if (Input.GetKey(KeyCode.S)){
            SetAnimBools(SHOOT);
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
                _isShooting = true;
                ShootArrow();
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
        _animator.SetBool("isShooting", _isShooting);
        _animator.SetBool("isDying", _isDying);
        _animator.SetBool("isHit", _isHit);
    }

    void SetAllToFalse()
    {
        _isIdle = false;
        _isWalking = false;
        _isRunning = false;
        _isShooting = false;
        _isDying = false;
        _isHit = false;
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

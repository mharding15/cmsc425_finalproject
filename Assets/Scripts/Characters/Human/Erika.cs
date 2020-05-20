using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Erika : Unit
{
    public GameObject arrow_prefab;
    private bool _isDelaying;

    protected bool _isRanged, _attackModeRanged;
    protected int RANGED_ATTACK = 6;
    public int rangedDamage;

    void Awake()
    {
        base.Start();
        SetAnimBools(IDLE);
        _isDelaying = false;
        SetStats();
        longRange = 12f;
        isEnemy = false;
    }

    void SetStats()
    {
        speed = 15;
        reaction = 29;
        hp = 13;
        ac = 8;

        meleeDamage = 3;
        rangedDamage = 2;
    }

    // Update is called once per frame
    void Update()
    {
        _animator.SetBool("isRanged", false);
        if (!_moving){
            SetAnimBools(IDLE);
        }
        // if this is the GameObject of the character whose turn it is
        if (isCurrent){
            CheckCameraMovement();
            print("In Update and name is: " + gameObject.name);
            // if M is pressed then the character should start moving (or at least we know that moving is what the character wants to do)
                // maybe there should be a message that says you are in walk mode, and if you click M again it removes it.
            if(Input.GetKey(KeyCode.M)){
                print("*** And M was pressed");
                EnterMoveMode();
            } else if (Input.GetKey(KeyCode.Z)){
                print("*** AND Z was pressed");
                EnterMeleeMode();
            } else if(Input.GetKey(KeyCode.R)){
                print("*** AND R was pressed");
                EnterRangedMode();
            }

            // if in melee mode, then need to make the goal equal to the target's position
            if ((_attackModeMelee || _attackModeRanged) && target != null){
                goal = target.transform.position;
                _goalSet = true;
            }

            // if getting the first part of the path
            if (!_goalSet && path.Count != 0){
                goal = path[0];
                _goalSet = true;
            }

            // rotate towards the goal (if there is a goal)
            if (_rotating && _goalSet){
                // if have not gotten the rotation angle, get it
                if (!_gotPhi){
                    GetRotationAngle(goal);
                } else {
                    // rotate a little bit towards the target
                    transform.Rotate(new Vector3(0f, phi, 0f));
                    // finished rotating, now can either move or attack or whatever
                    print("@@@ Done rotating");
                    _rotating = false;
                    if (_moveMode){
                        _moving = true;
                        startPos = transform.position;
                        SetAnimBools(WALK);
                    } else if (_attackModeMelee){
                        print("@@@ and calling MeleeAttack");
                        MeleeAttack();
                    } else if (_attackModeRanged){
                        print("@@@ and calling RangedAttack");
                        RangedAttack();
                    }
                    sumRotationTime = 0f;
                }
            }

            // if the user has indicated that they want to move (pressed M) and a target has not been established, then don't know where to go.
            if (!_rotating && _moving && goal != null){
                // move a little bit towards the target
                transform.Translate(Vector3.forward * speed * .25f * Time.deltaTime);
                // if within a distance of 2 of the target, stop moving and go to the next character's turn.
                float distToGoal = Distance(transform.position, goal);
                if (distToGoal < .1f){
                    // or if the distance travelled is greater than or equal to this character's speed, should also stop
                    pathIdx++;
                    if (pathIdx < path.Count){
                        goal = path[pathIdx];
                        _moving = false;
                        _rotating = true;
                        _gotPhi = false;
                        sumRotationTime = 0f;
                    } else {
                        ResetValuesAndNext();
                    }
                }
            }
        }
    }

    new public void EnterRangedMode()
    {
        print("!!! entering Ranged Attack mode");
        cl.SetModeText("Ranged Attack");
        _attackModeRanged = true;
        _rotating = true;
        _gotPhi = false;
        _goalSet = false;
    }

    public void RangedAttack()
    {
        print("$$$ Name: " + gameObject.name + " is in RangedAttack.");
        //Unit opponentUnit = GetOpponentUnit();
        int damageBonus = 0;
        // need to check if this attack hits or not
        int roll = UnityEngine.Random.Range(1, 20);
        print("$$$ and the attack roll was: " + roll);
        // critical hit
        if (roll == 20){
            damageBonus = 2;
        } 

        print("Opponent's ac is: " + targetUnit.ac);
        if (roll >= targetUnit.ac){
            print("$$$ Attack hit!");
            SetAnimBools(RANGED_ATTACK);
            cl.SetTurnResultText("Attack Hit!");
            StartOpponentGettingHit(rangedDamage + damageBonus, 4.5f);
        } else {
            cl.SetTurnResultText("Attack Missed");
        }

        // delay while the animation is going and then call Next()
        StartCoroutine(DelayForAnimation(2f));
    }

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

    new void SetAllToFalse()
    {
        base.SetAllToFalse();
        _isRanged = false;
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
        GameObject arrow = Instantiate(arrow_prefab, new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), transform.rotation);
        Rigidbody arrow_rb = arrow.GetComponent<Rigidbody>();
        //yield again for .5 seconds because the archer pauses before actually shooting the arrow
        yield return new WaitForSeconds(.5f);
        arrow_rb.velocity = transform.forward * 15f;
        _isDelaying = false;
    }

    new void OnTriggerEnter(Collider other){
        print("Are we in this trigger?");
    }
}

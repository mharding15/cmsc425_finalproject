using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedUnit : Unit
{
	protected bool _isRanged, _attackModeRanged;
	protected int RANGED_ATTACK = 6;

    public int rangedDamage;

	public GameObject spellEffect;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        longRange = 12f;
        SetAnimBools(IDLE, new Vector3(0f,0f,0f));
    }

    void Update()
    {
        _animator.SetBool("isRanged", false);
        if (!_moving){
            SetAnimBools(IDLE);
        }
        // if this is the GameObject of the character whose turn it is
        if (isCurrent){
            print("In Update and name is: " + gameObject.name);
            if(Input.GetKey(KeyCode.M)){
                print("*** And M was pressed");
                EnterMoveMode();
            } else if (Input.GetKey(KeyCode.Z)){
                print("*** AND A was pressed");
                EnterMeleeMode();
            }else if(Input.GetKey(KeyCode.R)){
                print("*** AND R was pressed");
                EnterRangedMode();
                cl.SetModeText("Ranged Attack");
            }

            // if in melee mode, then need to make the goal equal to the target's position
            if ((_attackModeMelee || _attackModeRanged) && target != null){
                goal = target.transform.position;
                _goalSet = true;
            }

            // if getting the first part of the path
            if (!_goalSet && path.Count != 0){
                print("111 Setting the goal, character: " + gameObject.name);
                goal = path[0];
                _goalSet = true;
            }

            // rotate towards the goal (if there is a goal)
            if (_rotating && _goalSet){
                print("222 Rotating and GoalSet, character: " + gameObject.name);
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
                        RangedAttack(target.transform.position);
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
                    // Maybe I should have a Reset() method that does all of this.
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
        _attackModeRanged = true;
        _rotating = true;
        _gotPhi = false;
        _goalSet = false;
    }

    new void SetMeleeBool()
    {
        SetAnimBools(MELEE, new Vector3(0f,0f,0f));
    }

    public void RangedAttack(Vector3 pos)
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
            cl.SetTurnResultText("Attack Hit!");
            SetAnimBools(RANGED_ATTACK, pos);
            StartOpponentGettingHit(rangedDamage + damageBonus);
        } else {
            cl.SetTurnResultText("Attack Missed");
        }

        // delay while the animation is going and then call Next()
        StartCoroutine(DelayForAnimation(2f));
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
                print("((( Are we just doing this multiple times??? Character: " + gameObject.name);
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

    new protected void ResetValuesAndNext()
    {
        print("$$$ In resetValues and Next");
        _moving = false;
        _moveMode = false;
        _attackModeMelee = false;
        _attackModeRanged = false;
        target = null;
        SetAnimBools(IDLE);
        cl.Next();
    }

    new void SetAllToFalse()
    {
        print("((( Is _isRanged being set to false??? Why would it not be...");
    	base.SetAllToFalse();
    	_isRanged = false;
    }

    void CreateEffect(Vector3 pos)
    {
    	Instantiate(spellEffect, pos, Quaternion.identity);
    }
}

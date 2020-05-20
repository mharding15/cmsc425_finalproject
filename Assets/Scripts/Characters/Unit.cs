using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{
    // *** STATS *** //

    // These stats will be modified based on what class the user pics for this character
    public int hp;
    public int ac;

    public int cunning;
    public int perception;
    public int reaction;
    public int speed;
    public int strength;
    public int will;

    // this should be isEnemy from YOUR perspective, not any of the characters' perspective. For your teammates this will be false.
    public bool isEnemy;

    public int healingPotionCount;
    public int meleeDamage;
    public float meleeRange;
    public float longRange;

    // *** OTHER VARIABLES *** //

    public Camera camera;

    public Text modeText;

    public List<Vector3> path {set; get;}
    protected int pathIdx;
    protected Vector3 goal;
    protected bool _goalSet;
    protected Vector3 startPos;

    public GameObject target {set; get;}
    public Unit targetUnit {set; get;}

    public bool isCurrent {set; get;}
    // whenever you press a certain button ('M' for moving, 'A' for attacking) this script will enter that mode
    protected bool _moveMode, _attackModeMelee;
    protected bool _moving, _rotating, _gotPhi;
    protected float sumRotationTime, phi;

    protected CombatLoop cl;

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
        meleeRange = 2f;
        longRange = 0f;
        healingPotionCount = 2;
        sumRotationTime = 0f;
        isCurrent = false;
        _rotating = false;
        _gotPhi = false;
        cl = GameObject.FindWithTag("Manager").GetComponent<CombatLoop>();
        pathIdx = 0;
        _goalSet = false;
        goal = new Vector3(0f,0f,0f);

        path = new List<Vector3>();
    }

    // Update is called once per frame
    void Update()
    {
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
                cl.SetModeText("Move");
            } else if (Input.GetKey(KeyCode.Z)){
                print("*** AND A was pressed");
                EnterMeleeMode();
                cl.SetModeText("Melee Attack");
            }

            // if in melee mode, then need to make the goal equal to the target's position
            if (_attackModeMelee && target != null){
                goal = target.transform.position;
            }

            // if getting the first part of the path
            if (!_goalSet && path.Count != 0){
                // adding this to test if it will make it more smooth??
                    // not sure if this actually helped...
                if (path.Count > 1){
                    pathIdx++;
                    goal = path[pathIdx];
                    // goal = path[0];
                }
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
                    
                    print("@@@ Done rotating");
                    _rotating = false;
                    if (_moveMode){
                        _moving = true;
                        startPos = transform.position;
                        SetAnimBools(WALK);
                    } else if (_attackModeMelee){
                        print("@@@ and calling MeleeAttack");
                        MeleeAttack();
                    } 
                    sumRotationTime = 0f;
                }
            }

            // if the user has indicated that they want to move (pressed M) and a target has not been established, then don't know where to go.
            if (_moving && goal != null){
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

    protected void CheckCameraMovement()
    {
        if (Input.GetKey(KeyCode.RightArrow)){
            transform.Rotate(new Vector3(0f, 30f * Time.deltaTime, 0f));
        } else if (Input.GetKey(KeyCode.LeftArrow)){
            print("666 and left arrow was pressed...");
            transform.Rotate(new Vector3(0f, -30f * Time.deltaTime, 0f));
        }
    }
    
    //wasnt sure I wanted to mess with your use in Combat Loop
    public void MoveTo(int x, int y)
    {
    	print(_name + " is moving...");
    	transform.position += new Vector3(x, 0, y);
    }

    // if the target destination is already selected, then the character will start moving, if not then once a target is selected they will move.
    public void EnterMoveMode()
    {
        print("!!! entering MOVE mode, character: " + gameObject.name);
        _moveMode = true;
        _rotating = true;
        _gotPhi = false;
        _goalSet = false;
    }

    public void EnterMeleeMode()
    {
        print("!!! entering Melee Attack mode, character: " + gameObject.name);
        _attackModeMelee = true;
        _rotating = true;
        _gotPhi = false;
    }

    public void EnterRangedMode()
    {
        print("Ah, fuck, so we are entering this ranged mode????");
    }

    // *** ACTIONS *** //

    // this method is for the character who is doing the attacking
    public void MeleeAttack()
    {
        print("$$$ Name: " + gameObject.name + " is in MeleeAttack.");
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
            cl.SetTurnResultText("Attack Hit!");
            SetMeleeBool();
            StartOpponentGettingHit(meleeDamage + damageBonus);
        } else {
            print("$$$ Attack missed...");
            cl.SetTurnResultText("Attack Missed");
        }

        // delay while the animation is going and then call Next()
        StartCoroutine(DelayForAnimation(2f));
    }

    // this is necessary because in the RangedUnit class, this needs to have a "pos" with it.
    protected void SetMeleeBool()
    {
        SetAnimBools(MELEE);
    }

    // need to delay a bit because some characters an a big wind up on their melee attacks
    protected void StartOpponentGettingHit(int damage)
    {
        float delay = .5f;
        if (gameObject.name == "Bruno"){
            delay = 1f;
        } else if (gameObject.name == "Maria"){
            delay = .5f;
        } else if (gameObject.name == "Panos"){
            delay = .5f;
        } else if (gameObject.name == "Nightshade"){
            delay = 1f;
        } else if (gameObject.name == "Mulok"){
            delay = 1f;
        } 
        StartCoroutine(DelayOpponentGettingHit(delay, damage));
    }

    protected void StartOpponentGettingHit(int damage, float time) 
    {
        StartCoroutine(DelayOpponentGettingHit(time, damage));
    }

    // this method is called when someone is doing damage to this character (so not when it's this character's turn)
    public void GetHit(int damage)
    {
        print("### In " + gameObject.name + " and damage is being taken. Starting hp is " + hp);
        SetAnimBools(HIT);
        hp -= damage;
        print("### In " + gameObject.name + " and new hp is: " + hp);
        // if the character is killed by this attack
        if (hp <= 0){
            hp = 0;
            // when the character's hp is 0 or below need to tell the combat loop that the character is dying.
            cl.SetDying(gameObject.name);
            // if the character is able to get back up, I guess I should add an animation for getting up.
            SetAnimBools(DIE);
        } else {
            StartCoroutine(DelayBackToIdle(.5f));
        }
    }

    public void TakePotion()
    {
        cl.SetTurnResultText("Potion taken");
        if (healingPotionCount > 0){
            healingPotionCount--;
            hp += 5;
        }
        // just go to the next character (or the same character if there are any more actions left)
        ResetValuesAndNext();
    }

    // *** ACTION HELPERS *** ///

    protected float Distance(Vector3 p1, Vector3 p2)
    {
        float x_diff = p1.x - p2.x;
        float z_diff = p1.z - p2.z;
        float d_sq = x_diff * x_diff + z_diff * z_diff;
        return Mathf.Sqrt(d_sq);
    }

    protected void GetRotationAngle(Vector3 point)
    {
        Vector3 newPoint = new Vector3(point.x - transform.position.x, 0f, point.z - transform.position.z);

        // get the rotation of the player
        float theta = 0f;
        if (transform.eulerAngles.y != 0f){
            theta = 360f - transform.eulerAngles.y;
        }

        // rotate the new point by this much so that the player's rotation is essentially 0
        Vector3 rotatedPoint = RotatePoint(theta, newPoint.x, newPoint.z);

        // get the amount to rotate
        phi = Vector3.Angle(Vector3.forward, rotatedPoint);

        // get which direction to rotate because phi is always between 0 and 180 degrees
        if (rotatedPoint.x < 0){
            phi *= -1;
        }

        _gotPhi = true;
    }

    protected Vector3 RotatePoint(float theta, float x, float z)
    {
        float[] rotationMatrix = new float[4];
        rotationMatrix[0] = Mathf.Cos(DegsToRads(theta));
        rotationMatrix[1] = Mathf.Sin(DegsToRads(theta));
        rotationMatrix[2] = -1f * rotationMatrix[1];
        rotationMatrix[3] = rotationMatrix[0];

        float rotated_x = rotationMatrix[0] * x + rotationMatrix[1] * z;
        float rotated_z = rotationMatrix[2] * x + rotationMatrix[3] * z;

        return new Vector3(rotated_x, 0f, rotated_z);
    }

    protected float DegsToRads(float theta)
    {
        return (theta * Mathf.PI)/180f;
    }

    protected float RadsToDegs(float theta)
    {
        return (theta * 180f)/Mathf.PI;
    }

    // *** ANIMATIONS *** //

     // this method can be called from outside of this script to set the animation for the character
    public void SetAnimBools(int state)
    {
        SetAllToFalse();

        switch(state){
            case 0:
                _isIdle = true;
                print("%%% Setting the IDLE animation for: " + gameObject.name);
                break;
            case 1:
                _isWalking = true;
                break;
            case 2:
                _isRunning = true;
                break;
            case 3:
                _isMelee = true;
                print("%%% Setting the MELEE animation for: " + gameObject.name);
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

    // testing clicking on the characters

    // So when this character is clicked, this GameObject will be passed to that script so the "target" can be set to this object. 
    protected void OnMouseUp()
    {
        print("Click detected on: " + gameObject.name);

        GameObject clickingObject = cl.GetCurrentObject();
        Unit clickingUnit = cl.GetCurrentUnit();
        clickingUnit.path = new List<Vector3>();
        clickingUnit.path.Add(transform.position);

        clickingUnit.target = gameObject;
        clickingUnit.targetUnit = this;
        cl.SetTargetText(gameObject.name);

        print("And the character who is clicking is: " + clickingObject.name);
    }

    protected void ResetValuesAndNext()
    {
        print("$$$ In resetValues and Next");
        _moving = false;
        _moveMode = false;
        _attackModeMelee = false;
        target = null;
        SetAnimBools(IDLE);
        cl.Next();
    }

    // need to delay so that the Next() character won't get called until the animation is done
    protected IEnumerator DelayForAnimation(float time)
    {
        //yield on a new YieldInstruction that waits for 2.5 seconds.
        yield return new WaitForSeconds(time);
        ResetValuesAndNext();
    }

    protected IEnumerator DelayBackToIdle(float time)
    {
        //yield on a new YieldInstruction that waits for 2.5 seconds.
        yield return new WaitForSeconds(time);
        print("^^^ Delay over, going back to IDEL now: " + gameObject.name);
        SetAnimBools(IDLE);
    }

    protected IEnumerator DelayOpponentGettingHit(float time, int damage)
    {
        yield return new WaitForSeconds(time);
        targetUnit.GetHit(damage);
    }

    // *** When the arrow hits this player *** //

    // void OnTriggerEnter(Collider other)
    // {
    //     print("))))(((( In the OnTrigger, so should be doing damage now.");
    //     if (other.gameObject.CompareTag("Arrow"))
    //     {
    //         GetHit(3);
    //         Destroy(other.gameObject);
    //     }
    // }
}

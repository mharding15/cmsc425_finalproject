using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	public bool isEnemy;

    public int healingPotionCount;

	// *** OTHER VARIABLES *** //

    public bool isCurrent {set; get;}
    public GameObject target {set; get;}
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
        healingPotionCount = 2;
        sumRotationTime = 0f;
        isCurrent = false;
        _rotating = false;
        _gotPhi = false;
        cl = GameObject.FindWithTag("Manager").GetComponent<CombatLoop>();
    }

    // Update is called once per frame
    void Update()
    {
        // if this is the GameObject of the character whose turn it is
        if (isCurrent){
            print("In Update and name is: " + _name);
            // if M is pressed then the character should start moving (or at least we know that moving is what the character wants to do)
                // maybe there should be a message that says you are in walk mode, and if you click M again it removes it.
            if(Input.GetKey(KeyCode.M)){
                print("*** And M was pressed");
                _rotating = true;
                _gotPhi = false;
            } 

            // rotate towards the goal (if there is a goal)
            if (_rotating && target != null){
                print("*((*(( target name is: " + target.name);
                // if have not gotten the rotation angle, get it
                if (!_gotPhi){
                    GetRotationAngle(target.transform.position);
                } else {
                    transform.Rotate(new Vector3(0f, phi, 0f) * Time.deltaTime);
                    sumRotationTime += Time.deltaTime;
                    if(sumRotationTime >= 1f){
                        _rotating = false;
                        _moving = true;
                        sumRotationTime = 0f;
                        SetAnimBools(WALK);
                    }
                }
            }

            // if the user has indicated that they want to move (pressed M) and a target has not been established, then don't know where to go.
            if (!_rotating && _moving && target != null){
                // move a little bit towards the target
                transform.Translate(Vector3.forward * speed * .5f * Time.deltaTime);
                // if within a distance of 2 of the target, stop moving and go to the next character's turn.
                if (Distance(transform.position, target.transform.position) < 2f){
                    // or if the distance travelled is greater than or equal to this character's speed, should also stop
                    _moving = false;
                    target = null;
                    SetAnimBools(IDLE);
                    cl.Next();
                }
            }
        }
    }
    
    //wasnt sure I wanted to mess with your use in Combat Loop
    public void MoveTo(int x, int y)
    {
    	print(_name + " is moving...");
    	transform.position = new Vector3(x, 0, y);
    }

    // *** ACTIONS *** //

    public void MeleeAttack()
    {
    	// basically just call the attack animation
    	SetAnimBools(MELEE);
    	// will probably need to set it back to IDLE here, but will test that later
    		// might have to have a set delay time or something, although probably not since the animation has exit time
    }

    public void TakePotion()
    {
        if (healingPotionCount > 0){
            healingPotionCount--;
            hp += 5;
        }
    }


    // *** ACTION HELPERS *** ///

    float Distance(Vector3 p1, Vector3 p2)
    {
        float x_diff = p1.x - p2.x;
        float z_diff = p1.z - p2.z;
        float d_sq = x_diff * x_diff + z_diff * z_diff;
        return Mathf.Sqrt(d_sq);
    }

    void GetRotationAngle(Vector3 point)
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

    Vector3 RotatePoint(float theta, float x, float z)
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

    float DegsToRads(float theta)
    {
        return (theta * Mathf.PI)/180f;
    }

    float RadsToDegs(float theta)
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

    // testing clicking on the characters

    // So when this character is clicked, this GameObject will be passed to that script so the "target" can be set to this object. 
    void OnMouseUp()
    {
        print("Click detected on: " + _name);

        GameObject clickingObject = cl.GetCurrentObject();
        string clickingName = cl.GetCurrentName();

        print("And the character who is clicking is: " + clickingName);

        if (clickingName == "Bruno"){
            clickingObject.GetComponent<Bruno>().target = gameObject;
        } else if (clickingName == "Erika"){
            clickingObject.GetComponent<Erika>().target = gameObject;
        } else if (clickingName == "Maria"){
            clickingObject.GetComponent<Maria>().target = gameObject;
        }else if (clickingName == "Panos") {
            clickingObject.GetComponent<Panos>().target = gameObject;
        } else if (clickingName == "Ganfaul"){
            clickingObject.GetComponent<Ganfaul>().target = gameObject;
        } else if (clickingName == "Nightshade"){
            clickingObject.GetComponent<Nightshade>().target = gameObject;
        } else if (clickingName == "Warrok"){
            clickingObject.GetComponent<Warrok>().target = gameObject;
        } else if (clickingName == "Mulok"){
            clickingObject.GetComponent<Mulok>().target = gameObject;
        } else if (clickingName == "Vurius"){
            clickingObject.GetComponent<Vurius>().target = gameObject;
        } else if (clickingName == "Zontog"){
            clickingObject.GetComponent<Zontog>().target = gameObject;
        } else {
            print("Whattt, none of these characters was clicking???");
        }
    }

}

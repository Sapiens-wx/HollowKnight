using System.Collections;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    public BoxCollider2D bc;
    public float gravity, maxFallSpd;
    public float invincibleTime;
    [Header("Movement")]
    public float xspd;
    [Header("Jump")]
    public KeyCode jumpKey;
    public float jumpHeight;
    public float jumpInterval;
    public float coyoteTime;
    [Header("Wall Jump")]
    public float climbDist;
    public float onWallYSpd; //y speed when on a wall
    public float wallJumpXSpd; //x speed when make a wall jump
    public float wallJumpXSpdInterval; //
    [Header("Dash")]
    public float dashBuffTime;
    public float dashDist;
    public float[] dashPercents;
    [Header("Counter")]
    public float counterBufferTime;
    [Header("Throw")]
    public float throwBufferTime;
    [Header("Ground Check")]
    public Vector2 leftBot;
    public Vector2 rightBot;
    public LayerMask groundLayer;
    [Header("Ceiling Check")]
    public Vector2 leftTop;
    public Vector2 rightTop;

    [HideInInspector] public Rigidbody2D rgb;
    [HideInInspector] public Animator animator;

    //inputs
    [HideInInspector] public int inputx;

    [HideInInspector] public static PlayerCtrl inst;
    [HideInInspector] public Vector2 v; //velocity
    [HideInInspector] public bool hittable;
    [HideInInspector] public bool onGround, prevOnGround;
    [HideInInspector] public float jumpKeyDown;
    [HideInInspector] public bool jumpKeyUp;
    [HideInInspector] public Vector2 climbTop, climbBot;
    [HideInInspector] public bool onWall, wallJumping;
    [HideInInspector] public Coroutine wallJumpCoro;
    [HideInInspector] public bool dashing, canDash;
    [HideInInspector] public float dashKeyDown;
    [HideInInspector] public float yspd;
    [HideInInspector] public int dir;
    [HideInInspector] public float counterKeyDown;
    [HideInInspector] public float throwKeyDown, throwChargeStartTime;
    [HideInInspector] public bool throwKeyUp;
    public int Dir{
        get=>dir;
        set{
            dir=value;
            leftTop.x*=-1;
            rightTop.x*=-1;
            leftBot.x*=-1;
            rightBot.x*=-1;
            transform.localScale=new Vector3(dir,1,1);
            climbTop.x*=-1;
            climbBot.x*=-1;
            return;
        }
    }
    void OnValidate(){
        climbTop=new Vector2(climbDist, bc.offset.y+bc.size.y/2);
        climbBot=new Vector2(climbDist, bc.offset.y-bc.size.y/2);
    }
    void OnDrawGizmosSelected(){
        Gizmos.color=Color.green;
        //ground check
        Gizmos.DrawLine((Vector2)transform.position+leftBot, (Vector2)transform.position+rightBot);
        //ceiling check
        Gizmos.DrawLine((Vector2)transform.position+leftTop, (Vector2)transform.position+rightTop);
        //jump height
        Gizmos.DrawLine(transform.position+new Vector3(-.2f,0,0),transform.position+new Vector3(.2f,0,0));
        Gizmos.DrawLine(transform.position,transform.position+new Vector3(0,jumpHeight,0));
        Gizmos.DrawLine(transform.position+new Vector3(-.2f,jumpHeight,0),transform.position+new Vector3(.2f,jumpHeight,0));
        //wall jump
        Gizmos.DrawLine((Vector2)transform.position+climbTop, (Vector2)transform.position+climbBot);
    }
    void Awake(){
        inst=this;
    }
    // Start is called before the first frame update
    void Start()
    {
        hittable=true;
        Dir=-1;
        jumpKeyDown=-100;
        counterKeyDown=-100;
        throwKeyDown=-100;
        dashKeyDown=-100;
        rgb=GetComponent<Rigidbody2D>();
        animator=GetComponent<Animator>();
        yspd=jumpHeight/jumpInterval-0.5f*gravity*jumpInterval;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.O))//counter
            counterKeyDown=Time.time;
        else if(Input.GetKeyDown(KeyCode.J))//throw
            throwKeyDown=Time.time;
        else if(Input.GetKeyDown(KeyCode.L))//dash
            dashKeyDown=Time.time;
        else if(Input.GetKeyDown(jumpKey))
            jumpKeyDown=Time.time;
        else if(Input.GetKeyUp(jumpKey))
            jumpKeyUp=true;
        
        if(Input.GetKeyUp(KeyCode.J))//throw end charge
            throwKeyUp=true;
    }
    void FixedUpdate(){
        HandleInputs();
        CheckOnGround();
        /*
        CheckWall();
        Movement();
        Jump();
        CeilingCheck();
        ApplyGravity();
        */
        UpdateVelocity();
    }
    #region hit
    void OnTriggerStay2D(Collider2D collider){
        if(hittable && collider.gameObject.layer==8){ //if is enemy
            animator.SetTrigger("hit");
        }
    }
    #endregion
    void HandleInputs(){
        inputx=(int)Input.GetAxisRaw("Horizontal");
    }
    void UpdateVelocity(){
        rgb.velocity=v;
    }
    void CheckOnGround(){
        prevOnGround=onGround;
        onGround = Physics2D.OverlapArea((Vector2)transform.position+leftBot, (Vector2)transform.position+rightBot, groundLayer);
        if(onGround)
            canDash=true;
    }
    void CheckWall(){
        onWall = Physics2D.OverlapArea((Vector2)transform.position+climbBot, (Vector2)transform.position+climbTop, groundLayer);
    }
    IEnumerator WallJumpCounter(){
        wallJumping=true;
        yield return new WaitForSeconds(wallJumpXSpdInterval);
        wallJumping=false;
        wallJumpCoro=null;
    }
}

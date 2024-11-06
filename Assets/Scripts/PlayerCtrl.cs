using System.Collections;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    [SerializeField] BoxCollider2D bc;
    public float gravity, maxFallSpd;
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
    public float dashDist;
    public float[] dashPercents;
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
    [HideInInspector] public bool onGround, prevOnGround;
    [HideInInspector] public float jumpKeyDown;
    [HideInInspector] public bool jumpKeyUp;
    [HideInInspector] public Vector2 climbTop, climbBot;
    [HideInInspector] public bool onWall, wallJumping;
    [HideInInspector] public Coroutine wallJumpCoro;
    [HideInInspector] public bool dashing, dashKeyDown, canDash;
    [HideInInspector] public float yspd;
    [HideInInspector] public int dir;
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
        Dir=-1;
        jumpKeyDown=-100;
        rgb=GetComponent<Rigidbody2D>();
        animator=GetComponent<Animator>();
        yspd=jumpHeight/jumpInterval-0.5f*gravity*jumpInterval;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(jumpKey))
            jumpKeyDown=Time.time;
        else if(Input.GetKeyUp(jumpKey))
            jumpKeyUp=true;
        if(Input.GetKeyDown(KeyCode.L))
            dashKeyDown=true;
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
    void HandleInputs(){
        inputx=(int)Input.GetAxisRaw("Horizontal");
    }
    void UpdateVelocity(){
        rgb.velocity=v;
    }
    void ApplyGravity(){
        if(dashing) return;
        if(onGround){
            if(!prevOnGround && v.y<0) //on ground enter
                v.y=0;
        }//if player is not wall jumping, is on wall, and is pressing the button the opposite dir of [dir], then the player should cling on the wall
        else if(!wallJumping&&onWall&&v.y<=0&&((dir==1 && Input.GetKey(KeyCode.A)) || (dir==-1 && Input.GetKey(KeyCode.D)))){
            v.y=onWallYSpd;
        }
        else if(v.y>=maxFallSpd)
            v.y+=gravity*Time.fixedDeltaTime;
    }
    void Movement(){
        //dash
        if(onGround || onWall) canDash=true;
        if(dashKeyDown){
            dashKeyDown=false;
            if(canDash){
                canDash=false;
                StartCoroutine(Dash());
            }
            return;
        }
        if(dashing) return;
        float x=Input.GetAxisRaw("Horizontal");
        if(!wallJumping){
            v.x=x*xspd;
        }
        //change direction
        if(x!=0 && x!=-dir){
            Dir=-(int)x;
        }
        //play animation
        animator.SetBool("run", x!=0);
    }
    IEnumerator Dash(){
        dashing=true;
        v.y=0;
        WaitForFixedUpdate wait=new WaitForFixedUpdate();
        //frame 1: anticipate, 2: 0.6*dist, 3: 0.8*dist, 4: 0.9*dist
        yield return wait;
        float dashSpd=dashDist/Time.fixedDeltaTime;
        for(int i=0;i<dashPercents.Length;++i){
            v.x=dir<=0?dashPercents[i]*dashSpd:-dashPercents[i]*dashSpd;
            yield return wait;
        }
        dashing=false;
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

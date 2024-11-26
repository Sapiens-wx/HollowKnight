using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    public Animator swordAnimator;
    public BoxCollider2D bc;
    public SpriteRenderer spr;
    public float gravity, maxFallSpd;
    public float keyDownBuffTime;
    [Header("Movement")]
    public float xspd;
    [Header("Jump")]
    public KeyCode jumpKey;
    public float jumpHeight;
    public float jumpInterval;
    public float coyoteTime;
    [Header("Wall Jump")]
    public float climbDist;
    public float wallCheckYPadding;
    public float onWallYSpd; //y speed when on a wall
    public float wallJumpXSpd; //x speed when make a wall jump
    public float wallJumpXSpdInterval; //
    [Header("Dash")]
    public float dashDist;
    public float[] dashPercents;
    public float dashInterval;
    [Header("Attack")]
    public float downSlashJumpSpd;
    public float slashInterval;
    [Header("Throw")]
    public Vector2 throwNailOffset;
    public float maxThrowChargeTime;
    [Header("Skill")]
    public float skillDist;
    public float dashToNailSpd;
    [Header("Ground Check")]
    public Vector2 leftBot;
    public Vector2 rightBot;
    [Header("Ceiling Check")]
    public Vector2 leftTop;
    public Vector2 rightTop;
    [Header("Hit")]
    public float invincibleTime;
    public float hitAnimDuration, counterAnimDuration;
    public float timeStopInterval;
    [Header("Recover")]
    public float recoverInterval;

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
    [HideInInspector] public bool onWall;
    [HideInInspector] public bool dashing, canDash;
    [HideInInspector] public float dashKeyDown, allowDashTime;
    [HideInInspector] public int dashDir;
    [HideInInspector] public float yspd;
    [HideInInspector] public int dir;
    [HideInInspector] public float counterKeyDown;
    [HideInInspector] public float throwKeyDown, throwChargeTime;
    [HideInInspector] public bool throwKeyUp;
    [HideInInspector] public bool attack_down; //whether is in attack_down state
    [HideInInspector] public float attackKeyDown; //whether is in attack_down state
    [HideInInspector] public float allowSlashTime;
    [HideInInspector] public float skillKeyDown; 
    //recover
    [HideInInspector] public float recoverKeyDown;
    [HideInInspector] public bool recoverKeyUp;
    [HideInInspector] public MaterialPropertyBlock matPB;
    [HideInInspector] public Sequence hitAnim, counterAnim, invincibleAnim;
    [HideInInspector] public Collider2D hitBy;
    public event Action OnPlayerHit;
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
            throwNailOffset.x*=-1;
            wallJumpXSpd*=-1;
            UniCamBehav.inst.ChangeXOffset(-dir);
            return;
        }
    }
    void OnValidate(){
        climbTop=new Vector2(climbDist, bc.offset.y+bc.size.y/2-wallCheckYPadding);
        climbBot=new Vector2(climbDist, bc.offset.y-bc.size.y/2+wallCheckYPadding);
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
        //throw
        Gizmos.DrawWireSphere(transform.position+(Vector3)throwNailOffset, .3f);
    }
    void Awake(){
        inst=this;
    }
    // Start is called before the first frame update
    void Start()
    {
        OnValidate();
        hittable=true;
        Dir=-1;
        jumpKeyDown=-100;
        counterKeyDown=-100;
        throwKeyDown=-100;
        dashKeyDown=-100;
        attackKeyDown=-100;
        skillKeyDown=-100;
        recoverKeyDown=-100;

        allowDashTime=0;
        allowSlashTime=0;

        rgb=GetComponent<Rigidbody2D>();
        animator=GetComponent<Animator>();
        yspd=jumpHeight/jumpInterval-0.5f*gravity*jumpInterval;

        //hit animation
        matPB=new MaterialPropertyBlock();
        spr.GetPropertyBlock(matPB);
        matPB.SetFloat("_whiteAmount", .5f);
        spr.SetPropertyBlock(matPB);

        //invincible anim
        invincibleAnim=DOTween.Sequence();
        invincibleAnim.SetAutoKill(false);
        invincibleAnim.AppendCallback(()=>{
            hittable=false;
        });
        invincibleAnim.Append(DOTween.To(()=>matPB.GetFloat("_whiteAmount"), (val)=>{
            spr.GetPropertyBlock(matPB);
            matPB.SetFloat("_whiteAmount",val);
            spr.SetPropertyBlock(matPB);
        }, 0, hitAnimDuration).SetLoops(Mathf.RoundToInt(invincibleTime/hitAnimDuration), LoopType.Yoyo).SetEase(Ease.InOutQuad));
        invincibleAnim.AppendCallback(()=>{
            hittable=true;
            spr.GetPropertyBlock(matPB);
            matPB.SetFloat("_whiteAmount",.5f);
            spr.SetPropertyBlock(matPB);
        });
        invincibleAnim.Pause();

        hitAnim=DOTween.Sequence();
        hitAnim.SetAutoKill(false);
        hitAnim.AppendCallback(()=>{
            spr.GetPropertyBlock(matPB);
            matPB.SetFloat("_whiteAmount",0);
            spr.SetPropertyBlock(matPB);
            hittable=false;
        });
        //hitAnim.AppendCallback(()=>StartCoroutine(PauseForSeconds(.5f)));
        hitAnim.AppendInterval(0.01f);
        hitAnim.Append(DOTween.To(()=>matPB.GetFloat("_whiteAmount"), (val)=>{
            spr.GetPropertyBlock(matPB);
            matPB.SetFloat("_whiteAmount",val);
            spr.SetPropertyBlock(matPB);
        }, .5f, counterAnimDuration));
        hitAnim.AppendCallback(()=>invincibleAnim.Restart());
        hitAnim.Pause();

        //counter animation
        counterAnim=DOTween.Sequence();
        counterAnim.SetAutoKill(false);
        counterAnim.AppendCallback(()=>{
            spr.GetPropertyBlock(matPB);
            matPB.SetFloat("_whiteAmount",1);
            spr.SetPropertyBlock(matPB);
            CameraCtrl.inst.ScreenShakeCM();
            //Time.timeScale=.1f;
        });
        counterAnim.AppendCallback(()=>StartCoroutine(PauseForSeconds(timeStopInterval)));
        counterAnim.AppendInterval(.01f);
        //counterAnim.AppendCallback(()=>Time.timeScale=1);
        counterAnim.Append(DOTween.To(()=>matPB.GetFloat("_whiteAmount"), (val)=>{
            spr.GetPropertyBlock(matPB);
            matPB.SetFloat("_whiteAmount",val);
            spr.SetPropertyBlock(matPB);
        }, .5f, counterAnimDuration));
        counterAnim.Pause();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))//recover
            recoverKeyDown=Time.time;
        else if(Input.GetKeyDown(KeyCode.O))//counter
            counterKeyDown=Time.time;
        else if(Input.GetKeyDown(KeyCode.U)) //skill
            skillKeyDown=Time.time;
        else if(Input.GetKeyDown(KeyCode.I)){//throw
            throwKeyDown=Time.time;
            throwKeyUp=false;
        }
        else if(Input.GetKeyDown(KeyCode.L))//dash
            dashKeyDown=Time.time;
        else if(Input.GetKeyDown(jumpKey))
            jumpKeyDown=Time.time;
        else if(Input.GetKeyUp(jumpKey))
            jumpKeyUp=true;
        else if(Input.GetKeyDown(KeyCode.J)){
            attackKeyDown=Time.time;
        }
        
        if(Input.GetKeyUp(KeyCode.I))//throw end charge
            throwKeyUp=true;
        if(Input.GetKeyUp(KeyCode.E)){//recover
            recoverKeyUp=true;
        }
    }
    void FixedUpdate(){
        HandleInputs();
        CheckOnGround();
        /*
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
    void CheckOnGround(){
        prevOnGround=onGround;
        onGround = Physics2D.OverlapArea((Vector2)transform.position+leftBot, (Vector2)transform.position+rightBot, GameManager.inst.groundLayer);
        if(onGround)
            canDash=true;
    }
    void CheckWall(){
        onWall = Physics2D.OverlapArea((Vector2)transform.position+climbBot, (Vector2)transform.position+climbTop, GameManager.inst.groundLayer);
    }
    IEnumerator PauseForSeconds(float sec){
        Time.timeScale=0;
        yield return new WaitForSecondsRealtime(sec);
        Time.timeScale=1;
    }
    void OnTriggerStay2D(Collider2D collider){
        //if is enemy, player is hit
        if(PlayerCtrl.inst.hittable && GameManager.IsLayer(GameManager.inst.enemyLayer, collider.gameObject.layer)){ 
            PlayerCtrl.inst.animator.SetTrigger("hit");
            hitBy=collider;
            OnPlayerHit?.Invoke();
        }
    }
}

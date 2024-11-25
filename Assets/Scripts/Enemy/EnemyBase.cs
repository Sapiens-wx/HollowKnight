using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] internal int maxHealth;
    [Header("Hit Anim")]
    [SerializeField] float hitAnimDuration;
    [SerializeField] SpriteRenderer spr;
    [Header("Ground Check")]
    MaterialPropertyBlock matPB;

    [HideInInspector] public Animator animator;
    [HideInInspector] public Rigidbody2D rgb;
    [HideInInspector] public Collider2D bc;
    int dir;
    float hitTime;
    public int Dir{
        get=>dir;
        set{
            dir=value;
            if(dir==1)
                transform.localScale=new Vector3(Mathf.Abs(transform.localScale.x),transform.localScale.y, transform.localScale.z);
            else
                transform.localScale=new Vector3(-Mathf.Abs(transform.localScale.x),transform.localScale.y, transform.localScale.z);
        }
    }
    internal int curHealth;
    //ground check
    [HideInInspector] public bool onGround, prevOnGround;

    Sequence s;
    internal virtual void Start(){
        hitTime=-100;
        Dir=1;

        animator=GetComponent<Animator>();
        rgb=GetComponent<Rigidbody2D>();
        bc=GetComponent<Collider2D>();

        curHealth=maxHealth;
        matPB=new MaterialPropertyBlock();
        spr.GetPropertyBlock(matPB);
        matPB.SetFloat("_whiteAmount", .5f);
        spr.SetPropertyBlock(matPB);

        s=DOTween.Sequence();
        s.SetAutoKill(false);
        s.Append(DOTween.To(()=>matPB.GetFloat("_whiteAmount"),(val)=>{
            spr.GetPropertyBlock(matPB);
            matPB.SetFloat("_whiteAmount",val);
            spr.SetPropertyBlock(matPB);
        }, 1, hitAnimDuration).SetLoops(2, LoopType.Yoyo));
        s.Pause();
    }
    public virtual void Hit(int damage){
        curHealth-=damage;
        if (curHealth < 0) {
            animator.SetTrigger("die");
        }
    }
    void OnTriggerEnter2D(Collider2D collider){
        if(GameManager.IsLayer(GameManager.inst.playerSwordLayer, collider.gameObject.layer) && Time.time-.23f>hitTime){ //if is sword layer
            hitTime=Time.time;
            Hit(1);
            s.Restart();
            //gain energy to the player
            PlayerBar.inst.CurEnergy+=1;
        }
    }
    internal void CheckOnGround(){
        prevOnGround=onGround;
        Bounds bounds=bc.bounds;
        Vector2 leftBot=bounds.min;
        Vector2 rightBot=leftBot;
        rightBot.x+=bounds.size.x*.9f;
        leftBot.x+=bounds.size.x*.1f;
        onGround = Physics2D.OverlapArea(leftBot,rightBot,GameManager.inst.groundLayer);
    }
}

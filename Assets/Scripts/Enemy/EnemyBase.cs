using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class EnemyBase : MonoBehaviour
{
    public EnemyType enemyType;
    public GameObject damageBox;
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
    Vector2 spawnPos;
    //ground check
    [HideInInspector] public bool onGround, prevOnGround;
    //camRoom
    [HideInInspector] public CamRoom associatedCamRoom;

    Sequence s;
    internal virtual void Start(){
        associatedCamRoom = CamRoom.WhichCamRoom(transform.position);

        hitTime=-100;
        Dir=1;

        animator=GetComponent<Animator>();
        rgb=GetComponent<Rigidbody2D>();
        bc=GetComponent<Collider2D>();

        spawnPos=transform.position;
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
    public virtual void Hit(){
        switch(PlayerCtrl.inst.lastAttackType){
            case PlayerCtrl.AttackType.SlashHorizontal: 
            case PlayerCtrl.AttackType.SlashUp:
            case PlayerCtrl.AttackType.SlashDown:
            case PlayerCtrl.AttackType.Other:
                curHealth-=1;
                break;
            case PlayerCtrl.AttackType.Counter:
                curHealth-=2;
                break;
            case PlayerCtrl.AttackType.Throw:
                curHealth-=3;
                break;
        }
        if (curHealth <= 0) {
            damageBox.SetActive(false);
            animator.SetTrigger("die");
            OnDead();
        }
    }
    internal virtual void OnDead(){
    }
    void OnTriggerEnter2D(Collider2D collider){
        if(GameManager.IsLayer(GameManager.inst.playerSwordLayer, collider.gameObject.layer) && Time.time-.23f>hitTime){ //if is sword layer
            if(curHealth<=0) return;
            hitTime=Time.time;
            Hit();
            s.Restart();
            //gain energy to the player
            switch(enemyType){
                case EnemyType.Enemy:
                case EnemyType.Soul:
                    PlayerBar.inst.CurEnergy+=1;
                    break;
            }
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
    public virtual void Recover(){
        if(curHealth<=0)
            animator.SetTrigger("idle");
        curHealth=maxHealth;
        damageBox.SetActive(true);
        transform.position=spawnPos;
    }
    public enum EnemyType{
        Enemy,
        Switch,
        Soul
    }
}

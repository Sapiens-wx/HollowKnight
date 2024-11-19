using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;

public class Enemy1 : EnemyBase
{
    [Header("Walk")]
    public float restInterval, restIntervalRange;
    public float walkSpd;
    public float walkInterval, walkIntervalRange;
    [Header("Attack")]
    public float attackInterval;
    public float runSpd;
    [Header("Hit")]
    public float hitInterval;
    public float hitDist;
    [Header("Detect")]
    public Bounds detectBounds;

    public static Enemy1 inst;
    //---walking---
    //xmin and max of the walking ground
    [HideInInspector] public float xmin, xmax;
    [HideInInspector] public float walkUntilTime;
    //---detect---
    [HideInInspector] public bool seesPlayer, prevSeesPlayer;
    void OnDrawGizmosSelected(){
        Gizmos.DrawWireCube(transform.position+detectBounds.center, detectBounds.size);
    }
    void Awake(){
        inst=this;
    }
    void FixedUpdate(){
        CheckOnGround();
        if(!prevOnGround && onGround){ //landing
            UpdateGroundXMinMax();
        }
    }
    public void UpdateGroundXMinMax(){
        Bounds bounds=bc.bounds;
        Vector2 leftBot=bounds.min;
        Vector2 rightBot=leftBot;
        rightBot.x+=bounds.size.x;
        Vector2 leftTop=leftBot;
        leftTop.y+=bounds.size.y;
        Vector2 rightTop=bounds.max;
        Collider2D ground=Physics2D.OverlapArea(leftBot+new Vector2(bounds.size.x*.1f,0),rightBot+new Vector2(-bounds.size.x*.1f,0),GameManager.inst.groundLayer);
        if(ground==null) return;
        xmin=ground.bounds.min.x;
        xmax=ground.bounds.max.x;
        //check if there is any obstacles
        Vector2 offset=new Vector2(0,.1f);
        RaycastHit2D leftTopHit=Physics2D.Raycast(leftTop-offset, Vector2.left, Mathf.Clamp(leftTop.x-xmin,0,float.MaxValue), GameManager.inst.groundLayer);
        RaycastHit2D leftBotHit=Physics2D.Raycast(leftBot+offset, Vector2.left, Mathf.Clamp(leftTop.x-xmin,0,float.MaxValue), GameManager.inst.groundLayer);
        RaycastHit2D rightTopHit=Physics2D.Raycast(rightTop-offset, Vector2.right, Mathf.Clamp(xmax-rightTop.x,0,float.MaxValue), GameManager.inst.groundLayer);
        RaycastHit2D rightBotHit=Physics2D.Raycast(rightBot+offset, Vector2.right, Mathf.Clamp(xmax-rightTop.x,0,float.MaxValue), GameManager.inst.groundLayer);
        if(leftTopHit==true) xmin=leftTopHit.point.x;
        if(leftBotHit==true) xmin=leftBotHit.point.x;
        if(rightTopHit==true) xmax=rightTopHit.point.x;
        if(rightBotHit==true) xmax=rightBotHit.point.x;
    }
    public void UpdateSeesPlayer(){
        prevSeesPlayer=seesPlayer;
        seesPlayer=Physics2D.OverlapArea(detectBounds.min+transform.position, detectBounds.max+transform.position, GameManager.inst.playerLayer);
    }
    public override void Hit(int damage)
    {
        base.Hit(damage);
        //judge whether it is hit from top or horizontally
        if(Mathf.Abs(PlayerCtrl.inst.transform.position.y-transform.position.y)<.8f){ //horizontal
            animator.SetTrigger("hit_horizontal");
        } else{
            animator.SetTrigger("hit_vertical");
        }
    }
}

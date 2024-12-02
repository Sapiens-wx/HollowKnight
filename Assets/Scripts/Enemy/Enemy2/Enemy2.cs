using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : EnemyBase
{
    [SerializeField] Bounds detectBounds, attackBounds;
    public float patrolDist, patrolFrq;
    public float verticalDist, verticalFrq;
    public float chaseSpd;
    [Header("Attack")]
    public float attackInterval;
    public float bulletSpd;
    public float bulletAngle;
    [Header("Hit")]
    public float hitStateDuration;
    public float hitDist;

    [HideInInspector] public float allowAttackTime;
    void OnDrawGizmosSelected(){
        Gizmos.DrawWireCube(transform.position+detectBounds.center, detectBounds.size);
        Gizmos.color=Color.red;
        Gizmos.DrawWireCube(transform.position+attackBounds.center, attackBounds.size);
        Gizmos.color=Color.blue;
        Gizmos.DrawLine(new Vector2(transform.position.x-patrolDist, transform.position.y), new Vector2(transform.position.x+patrolDist, transform.position.y));
    }
    // Start is called before the first frame update
    internal override void Start()
    {
        base.Start();
        associatedCamRoom.onPlayerExitRoom+=Recover;
        allowAttackTime=0;
        EnemyManager.inst.RegisterEnemy(this);
    }

    public bool DetectPlayer(){
        if(CamRoom.activeRoom!=associatedCamRoom) return false;
        Vector2 min=detectBounds.min+transform.position;
        Vector2 max=detectBounds.max+transform.position;
        Vector2 pos=PlayerCtrl.inst.transform.position;
        return !(min.x>pos.x||min.y>pos.y||max.x<pos.x||max.y<pos.y);
    }
    public override void Hit()
    {
        base.Hit();
        float spd=hitDist/hitStateDuration;
        switch(PlayerCtrl.inst.lastAttackType){
            case PlayerCtrl.AttackType.Throw:
            case PlayerCtrl.AttackType.SlashHorizontal: 
                rgb.velocity=new Vector2(PlayerCtrl.inst.dir==-1?spd:-spd,0);
                break;
            case PlayerCtrl.AttackType.Counter:
            case PlayerCtrl.AttackType.Other:
            case PlayerCtrl.AttackType.SlashUp:
                rgb.velocity=((Vector2)transform.position-(Vector2)PlayerCtrl.inst.transform.position-PlayerCtrl.inst.bc.offset).normalized*spd;
                break;
            case PlayerCtrl.AttackType.SlashDown:
                rgb.velocity=new Vector2(0,-spd);
                break;
        }
        animator.SetTrigger("hit");
    }
    public bool InsideAttackBounds(){
        Vector2 min=attackBounds.min+transform.position;
        Vector2 max=attackBounds.max+transform.position;
        Vector2 pos=PlayerCtrl.inst.transform.position;
        return !(min.x>pos.x||min.y>pos.y||max.x<pos.x||max.y<pos.y);
    }
    public override void Recover()
    {
        base.Recover();
        rgb.gravityScale=0;
    }
}

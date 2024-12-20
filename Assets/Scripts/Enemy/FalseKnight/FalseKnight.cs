using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FalseKnight : EnemyBase
{
    [Header("run")]
    public float runSpd;
    [Header("jump")]
    public float jumpHeight;
    public float jumpInterval;
    public float leftx, rightx, closeToPlayerLimit;
    [Header("attack")]
    public Vector2 bulletInstPos;
    [Header("Lock Room")]
    [SerializeField] GameObject[] doors;

    public static FalseKnight inst;
    //jump/jump_attack state
    [HideInInspector] public float jumpToX;
    [HideInInspector] public bool canJumpAndAttack;
    [HideInInspector] public float closestDistToPlayer;
    private void OnDrawGizmosSelected()
    {
        //bounds
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(new Vector2(leftx, -10), new Vector2(rightx, -10));
        Gizmos.DrawWireSphere((Vector2)transform.position+bulletInstPos, .5f);
    }
    private void Awake() {
        inst = this;
    }
    internal override void Start()
    {
        base.Start();
        associatedCamRoom.onPlayerEnterRoom+=()=>{
            if(curHealth>0){
                SFX.Play(SFX.inst.fsm);
                if(doors!=null){
                    foreach(GameObject go in doors){
                        go.SetActive(true);
                    }
                }
            }
        };
		associatedCamRoom.onPlayerExitRoom+=()=>{
            if(SFX.inst.audioSource.clip!=SFX.inst.bgm)
                SFX.Play(SFX.inst.bgm);
			if(doors!=null){
				foreach(GameObject go in doors){
                    go.SetActive(false);
                }
			}
			if(curHealth>0){ //if the player dies, but the boss did not, then recover the boss
				Recover();
			}
		};
        Dir=1;
        closestDistToPlayer=bc.offset.x+bc.bounds.extents.x+PlayerCtrl.inst.bc.offset.x+PlayerCtrl.inst.bc.bounds.extents.x;
    }
    internal override void OnDead()
    {
        if(doors!=null){
            foreach(GameObject go in doors){
                go.SetActive(false);
            }
        }
    }
    private void FixedUpdate()
    {
        CheckOnGround();
    }
}

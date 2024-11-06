using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PStateBase : StateMachineBehaviour
{
    internal PlayerCtrl player;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(player==null) player=PlayerCtrl.inst;
    }
    //execution order 
    /*
    void FixedUpdate(){
        CheckWall();
        Movement();
        Jump();
        CheckOnGround();
        CeilingCheck();
        ApplyGravity();
        UpdateVelocity();
    }*/
    internal void UpdateVelocity(){
        player.rgb.velocity=player.v;
    }
    virtual internal void ApplyGravity(){
        if(player.onGround){
            if(!player.prevOnGround && player.v.y<0) //on ground enter
                player.v.y=0;
        }//if player is not wall jumping, is on wall, and is pressing the button the opposite dir of [dir], then the player should cling on the wall
        else if(player.v.y>=player.maxFallSpd)
            player.v.y+=player.gravity*Time.fixedDeltaTime;
    }
    virtual internal void Movement(){
        player.v.x=player.xspd*player.inputx;
        //change direction
        if(player.inputx!=0 && player.inputx!=-player.dir){
            player.Dir=-player.inputx;
        }
    }
    virtual internal void Dash(){
        //dash
        if(player.dashKeyDown){
            player.dashKeyDown=false;
            if(player.canDash){
                player.canDash=false;
                player.animator.SetTrigger("dash");
            }
        }
    }
    internal IEnumerator DashAnim(){
        player.v.y=0;
        WaitForFixedUpdate wait=new WaitForFixedUpdate();
        float dashSpd=player.dashDist/Time.fixedDeltaTime;
        for(int i=0;i<player.dashPercents.Length;++i){
            player.v.x=player.dir<=0?player.dashPercents[i]*dashSpd:-player.dashPercents[i]*dashSpd;
            yield return wait;
        }
        player.animator.SetTrigger("dash_recover");
    }
    virtual internal void CheckOnGround(){
        player.prevOnGround=player.onGround;
        player.onGround = Physics2D.OverlapArea((Vector2)player.transform.position+player.leftBot, (Vector2)player.transform.position+player.rightBot, player.groundLayer);
    }
    internal void CeilingCheck(){
        if(Physics2D.OverlapArea((Vector2)player.transform.position+player.leftTop, (Vector2)player.transform.position+player.rightTop, player.groundLayer)){
            if(player.v.y>0) player.animator.SetTrigger("jump_down");
        }
    }
    virtual internal void Jump(){
        if(Time.time-player.jumpKeyDown<=player.coyoteTime){
            player.jumpKeyDown=-100;
            if(player.onGround){ //ground jump
                player.v.y=player.yspd;
            } else if(player.onWall){ //wall jump
                player.v.y=player.yspd;
                player.v.x=player.dir*player.wallJumpXSpd;
                player.wallJumpCoro = player.StartCoroutine(WallJumpCounter());
            }
        }
        if(player.jumpKeyUp){
            player.jumpKeyUp=false;
            if(player.wallJumpCoro!=null){
                player.StopCoroutine(player.wallJumpCoro);
                player.wallJumpCoro=null;
                player.wallJumping=false;
            }
            if(player.v.y>0){
                player.v.y=0;
			}
        }
    }
    virtual internal void CheckWall(){
        player.onWall = Physics2D.OverlapArea((Vector2)player.transform.position+player.climbBot, (Vector2)player.transform.position+player.climbTop, player.groundLayer);
    }
    virtual internal IEnumerator WallJumpCounter(){
        player.wallJumping=true;
        yield return new WaitForSeconds(player.wallJumpXSpdInterval);
        player.wallJumping=false;
        player.wallJumpCoro=null;
    }
}

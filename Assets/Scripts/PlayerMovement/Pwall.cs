using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class Pwall : PStateBase
{
    Coroutine coro;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        coro = player.StartCoroutine(m_FixedUpdate());
        player.v.x=0;
        player.canDash=true;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player.StopCoroutine(coro);
        coro=null;
    }
    IEnumerator m_FixedUpdate(){
        WaitForFixedUpdate wait=new WaitForFixedUpdate();
        while(true){
            CheckWall();
            Jump();
            Dash();
            ApplyGravity();
            yield return wait;
        }
    }
    internal override void Jump(){
        if(!player.onWall){
            player.animator.SetTrigger(player.onGround?"idle":"jump_down");
        }
        else if(player.onGround)
            player.animator.SetTrigger("idle");
        else if(Time.time-player.jumpKeyDown<=player.coyoteTime){
            player.jumpKeyDown=-100;
            player.jumpKeyUp=false;
            player.animator.SetTrigger("wall_jump_up");
        } else if(player.inputx==player.Dir){ // off the wall
            player.Dir=-player.inputx;
            player.animator.SetTrigger("jump_down");
        }
    }
    internal override void ApplyGravity(){
        player.v.y=player.onWallYSpd;
    }
    internal override void Dash()
    {
        if(Time.time-player.dashKeyDown<=player.keyDownBuffTime){
            player.dashKeyDown=-100;
            if(player.canDash){
                player.canDash=false;
                player.dashDir=-player.Dir; //explicitly set dash direction
                player.animator.SetTrigger("dash");
            }
        }
    }
}

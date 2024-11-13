using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class Pwall_jump_up : PStateBase
{
    Coroutine coro, delayMoveCoro;
    bool canMove;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        canMove=false;
        player.v.x=player.wallJumpXSpd;
        player.v.y=player.yspd;

        coro = player.StartCoroutine(m_FixedUpdate());
        delayMoveCoro=player.StartCoroutine(DelayMovement());
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
        if(delayMoveCoro!=null){
            player.StopCoroutine(delayMoveCoro);
            delayMoveCoro=null;
        }
    }
    IEnumerator m_FixedUpdate(){
        WaitForFixedUpdate wait=new WaitForFixedUpdate();
        while(true){
            Attack();
            if(canMove) Movement();
            Jump();
            Dash();
            ApplyGravity();
            CeilingCheck();
            yield return wait;
        }
    }
    override internal void ApplyGravity(){
        player.v.y+=player.gravity*Time.fixedDeltaTime;
    }
    override internal void Jump(){
        if(player.v.y<=0 || player.jumpKeyUp){
            player.jumpKeyUp=false;
            player.v.y=0;
            player.animator.SetTrigger("jump_down");
        }
    }
    IEnumerator DelayMovement(){
        yield return new WaitForSeconds(player.wallJumpXSpdInterval);
        canMove=true;
        delayMoveCoro=null;
    }
}

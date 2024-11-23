using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pattack_down : PStateBase
{
    Coroutine coro;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        coro = player.StartCoroutine(m_FixedUpdate());
        player.attack_down=true;
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
        player.attack_down=false;
    }
    IEnumerator m_FixedUpdate(){
        WaitForFixedUpdate wait=new WaitForFixedUpdate();
        while(true){
            Movement();
            Jump();
            ApplyGravity();
            yield return wait;
        }
    }
    override internal void Movement(){
        player.v.x=player.xspd*player.inputx;
        //change direction
        if(player.inputx!=0 && player.inputx!=-player.dir){
            player.Dir=-player.inputx;
        }
    }
    internal override void Jump()
    {
        if(player.jumpKeyUp){
            player.jumpKeyUp=false;
            player.v.y=0;
            //player.animator.SetTrigger("jump_down");
        }
    }
    override internal void ApplyGravity(){
        if(player.v.y>=player.maxFallSpd)
            player.v.y+=player.gravity*Time.fixedDeltaTime;
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class Pthrow_charge : PStateBase
{
    Coroutine coro;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        coro = player.StartCoroutine(m_FixedUpdate());
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
            if(player.throwKeyUp){
                player.throwKeyUp=false;
                player.throwChargeTime=Time.time-player.throwChargeTime;
                if(player.throwChargeTime>player.maxThrowChargeTime)
                    player.throwChargeTime=player.maxThrowChargeTime;
                player.animator.SetTrigger("throw");
            }
            yield return wait;
        }
    }
}

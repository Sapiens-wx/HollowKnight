using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class Pcounter : PStateBase
{
    Coroutine coro, setBoolCoro;
    public float canCounterAttackTime;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        player.v.x=0;
        animator.SetBool("counter_attack", true);
        coro = player.StartCoroutine(m_FixedUpdate());
        setBoolCoro=player.StartCoroutine(SetBoolDelay());
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
        if(setBoolCoro!=null){
            player.StopCoroutine(setBoolCoro);
            setBoolCoro=null;
        }
    }
    IEnumerator m_FixedUpdate(){
        WaitForFixedUpdate wait=new WaitForFixedUpdate();
        while(true){
            yield return wait;
        }
    }
    IEnumerator SetBoolDelay(){
        yield return new WaitForSeconds(canCounterAttackTime);
        player.animator.SetBool("counter_attack", false);
        setBoolCoro=null;
    }
}

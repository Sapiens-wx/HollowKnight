using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class E2_hit : E2StateBase
{
    Coroutine coro, hitCoro;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        coro = enemy.StartCoroutine(m_FixedUpdate());
        hitCoro=enemy.StartCoroutine(DelayExit());
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy.StopCoroutine(coro);
        coro=null;
        if(hitCoro!=null){
            enemy.StopCoroutine(hitCoro);
            hitCoro=null;
        }
    }
    IEnumerator DelayExit(){
        yield return new WaitForSeconds(enemy.hitStateDuration);
        enemy.animator.SetTrigger("idle");
    }
    IEnumerator m_FixedUpdate(){
        WaitForFixedUpdate wait=new WaitForFixedUpdate();
        while(true){
            yield return wait;
        }
    }
}
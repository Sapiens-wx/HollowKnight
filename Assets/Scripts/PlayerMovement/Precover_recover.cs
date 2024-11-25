using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class Precover_recover : PStateBase
{
    Coroutine coro;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        PlayerBar.inst.CurEnergy--;
        PlayerBar.inst.SetCurHealth(PlayerBar.inst.CurHealth+1, null);
        if(PlayerBar.inst.CurHealth==PlayerBar.inst.MaxHealth || !PlayerBar.inst.CanConsume())
            animator.SetTrigger("idle");
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
            if(player.recoverKeyUp){
                player.recoverKeyUp=false;
                player.animator.SetTrigger("idle");
            }
            yield return wait;
        }
    }
}

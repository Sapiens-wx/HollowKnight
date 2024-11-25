using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class Precover : PStateBase
{
    Coroutine coro, recoverCoro;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        player.v.x=0;
        player.recoverKeyUp=false;
        PlayerBar.inst.CurEnergy--;
        recoverCoro=player.StartCoroutine(RecoverAnim());
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
        if(recoverCoro!=null){
            player.StopCoroutine(recoverCoro);
            recoverCoro=null;
        }
    }
    IEnumerator RecoverAnim(){
        yield return new WaitForSeconds(player.recoverInterval);
        player.animator.SetTrigger("recover_recover");
        recoverCoro=null;
    }
    IEnumerator m_FixedUpdate(){
        WaitForFixedUpdate wait=new WaitForFixedUpdate();
        while(true){
            if(player.recoverKeyUp){
                Debug.Log("recoverkeyup, set trigger idle");
                player.recoverKeyUp=false;
                player.animator.SetTrigger("idle");
            }
            yield return wait;
        }
    }
}

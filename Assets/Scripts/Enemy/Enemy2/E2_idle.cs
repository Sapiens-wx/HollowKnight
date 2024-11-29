using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class E2_idle : E2StateBase
{
    Coroutine coro;
    Coroutine patrolCoro;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        try{
        coro = enemy.StartCoroutine(m_FixedUpdate());
        } catch(System.Exception) {
            
        }
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
    }
    IEnumerator m_FixedUpdate(){
        WaitForFixedUpdate wait=new WaitForFixedUpdate();
        while(true){
            //patrol
            HorizontalMovement();
            VerticalMovement();

            //detect player
            if(enemy.DetectPlayer()){
                if(Mathf.Sign(PlayerCtrl.inst.transform.position.x-enemy.transform.position.x)!=enemy.Dir){
                    enemy.animator.SetTrigger("turn");
                }
                enemy.animator.SetTrigger("chase");
            }
            yield return wait;
        }
    }
}

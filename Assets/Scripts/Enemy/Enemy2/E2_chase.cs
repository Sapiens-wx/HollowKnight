using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class E2_chase : E2StateBase
{
    Coroutine coro, chaseCoro;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        coro = enemy.StartCoroutine(m_FixedUpdate());
        chaseCoro=enemy.StartCoroutine(Chase());
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
        if(chaseCoro!=null){
            enemy.StopCoroutine(chaseCoro);
            chaseCoro=null;
        }
    }
    IEnumerator Chase(){
        WaitForSeconds wait=new WaitForSeconds(.4f);
        while(true){
            Vector2 dir=((Vector2)PlayerCtrl.inst.transform.position-(Vector2)enemy.transform.position).normalized;
            enemy.rgb.velocity=dir*enemy.chaseSpd;
            if(CamRoom.activeRoom!=enemy.associatedCamRoom) enemy.animator.SetTrigger("idle");
            if(Mathf.Sign(PlayerCtrl.inst.transform.position.x-enemy.transform.position.x)!=enemy.Dir){
                enemy.animator.SetTrigger("turn");
                enemy.animator.SetTrigger("chase");
            }
            yield return wait;
        }
    }
    IEnumerator m_FixedUpdate(){
        WaitForFixedUpdate wait=new WaitForFixedUpdate();
        while(true){
            if(enemy.InsideAttackBounds()){
                TryAttack();
            }
            yield return wait;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class E1_idle : E1StateBase
{
    Coroutine coro;
    Coroutine toWalkCoro;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        coro = enemy.StartCoroutine(m_FixedUpdate());
        toWalkCoro=enemy.StartCoroutine(RandWalk());
        enemy.rgb.velocity=new Vector2(0, enemy.rgb.velocity.y);
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
        if(toWalkCoro!=null){
            enemy.StopCoroutine(toWalkCoro);
            toWalkCoro=null;
        }
    }
    IEnumerator RandWalk(){
        WaitForFixedUpdate wait=new WaitForFixedUpdate();
        while(!enemy.onGround){
            yield return wait;
        }
        yield return new WaitForSeconds(UnityEngine.Random.Range(enemy.restInterval-enemy.restIntervalRange/2, enemy.restInterval+enemy.restIntervalRange/2));
        enemy.walkUntilTime=Time.time+Random.Range(enemy.walkInterval-enemy.walkIntervalRange/2, enemy.walkInterval+enemy.walkIntervalRange/2);
        enemy.animator.SetTrigger("walk");
        toWalkCoro=null;
    }
    IEnumerator m_FixedUpdate(){
        WaitForFixedUpdate wait=new WaitForFixedUpdate();
        while(true){
            DetectPlayer();
            yield return wait;
        }
    }
}

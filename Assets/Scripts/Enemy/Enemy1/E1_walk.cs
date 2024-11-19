using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class E1_walk : E1StateBase
{
    Coroutine coro;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        coro = enemy.StartCoroutine(m_FixedUpdate());
        enemy.rgb.velocity=new Vector2(enemy.Dir==1?enemy.walkSpd:-enemy.walkSpd, enemy.rgb.velocity.y);
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
        enemy.rgb.velocity=new Vector2(0, enemy.rgb.velocity.y);
    }
    IEnumerator m_FixedUpdate(){
        WaitForFixedUpdate wait=new WaitForFixedUpdate();
        while(true){
            DetectPlayer();
            //if reach walk time, then stop
            if(Time.time>=enemy.walkUntilTime){
                enemy.animator.SetTrigger("idle");
            }
            //if reach the edge, then turn and continue walking
            if(enemy.transform.position.x>enemy.xmax-.5f){
                Vector3 pos=enemy.transform.position;
                pos.x=enemy.xmax-.5f;
                enemy.transform.position=pos;
                enemy.animator.SetTrigger("turn");
                enemy.animator.SetTrigger("walk");
            }
            else if(enemy.transform.position.x<enemy.xmin+.5f){
                Vector3 pos=enemy.transform.position;
                pos.x=enemy.xmin+.5f;
                enemy.transform.position=pos;
                enemy.animator.SetTrigger("turn");
                enemy.animator.SetTrigger("walk");
            }
            yield return wait;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class E1_hit_horizontal : E1StateBase
{
    Coroutine coro;
    float exitTime;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        if(enemy.Dir==1)
            enemy.rgb.velocity=new Vector2(-enemy.hitDist/enemy.hitInterval, enemy.rgb.velocity.y);
        else
            enemy.rgb.velocity=new Vector2(enemy.hitDist/enemy.hitInterval, enemy.rgb.velocity.y);
        exitTime=Time.time+enemy.hitInterval;
        coro = enemy.StartCoroutine(m_FixedUpdate());
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
            if(Time.time>=exitTime){
                enemy.animator.SetTrigger("idle");
            }
            yield return wait;
        }
    }
}

using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEditor;
using UnityEngine;

public class FKjump_attack : FKStateBase
{
    Coroutine coro;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        coro = knight.StartCoroutine(m_FixedUpdate());
        Jump();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        knight.StopCoroutine(coro);
        coro=null;
        knight.rgb.velocity=Vector2.zero;
    }
    IEnumerator m_FixedUpdate(){
        WaitForFixedUpdate wait=new WaitForFixedUpdate();
        while(true){
            if (knight.onGround && knight.rgb.velocity.y<=0)
            {
                knight.animator.SetTrigger("jump_land");
            }
            yield return wait;
        }
    }
    void Jump()
    {
        float g = knight.rgb.gravityScale * 9.8f;
        knight.rgb.velocity = new Vector2(knight.jumpToX / knight.jumpInterval, knight.jumpHeight / knight.jumpInterval * 2 + .25f * g * knight.jumpInterval);
    }
}

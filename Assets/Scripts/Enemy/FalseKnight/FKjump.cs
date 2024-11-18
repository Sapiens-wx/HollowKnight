using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEditor;
using UnityEngine;

public class FKjump : FKStateBase
{
    Coroutine coro;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        Jump();
        coro = knight.StartCoroutine(m_FixedUpdate());
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
        knight.rgb.gravityScale=1;
    }
    IEnumerator m_FixedUpdate(){
        WaitForFixedUpdate wait=new WaitForFixedUpdate();
        while(true){
            if (knight.onGround && knight.rgb.velocity.y<=0)
            {
                knight.rgb.velocity = Vector2.zero;
                CameraCtrl.inst.StartCoroutine(CameraCtrl.inst.ScreenShake());
                knight.animator.SetTrigger("jump_land");
                if(knight.canJumpAndAttack && Random.Range(0,2)==0)
                    knight.animator.SetTrigger("attack");
            }
            yield return wait;
        }
    }
    void Jump()
    {
        float g = 8*knight.jumpHeight/(knight.jumpInterval*knight.jumpInterval);
        knight.rgb.gravityScale=g/9.8f;
        //knight.rgb.velocity = new Vector2((knight.jumpToX-knight.transform.position.x) / knight.jumpInterval, knight.jumpHeight / knight.jumpInterval * 2 + .25f * g * knight.jumpInterval);
        knight.rgb.velocity = new Vector2((knight.jumpToX-knight.transform.position.x) / knight.jumpInterval, 4*knight.jumpHeight/knight.jumpInterval);
    }
}

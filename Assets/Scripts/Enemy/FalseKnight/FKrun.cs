using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class FK : FKStateBase
{
    Coroutine coro;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        coro = knight.StartCoroutine(m_FixedUpdate());
        knight.rgb.velocity = new Vector2(knight.dir == 1 ? knight.runSpd : -knight.runSpd, knight.rgb.velocity.y);
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
    }
    IEnumerator m_FixedUpdate(){
        WaitForFixedUpdate wait=new WaitForFixedUpdate();
        while(true){
            if (Mathf.Abs(knight.transform.position.x - PlayerCtrl.inst.transform.position.x) <= knight.closeToPlayerLimit)
            {
                knight.rgb.velocity = Vector2.zero;
                knight.animator.SetTrigger("idle");
            }
            yield return wait;
        }
    }
}

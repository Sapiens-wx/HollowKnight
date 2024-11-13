using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class Pskill : PStateBase
{
    Coroutine coro;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        coro = player.StartCoroutine(m_FixedUpdate());
        player.v=Vector2.zero;
        player.StartCoroutine(SkillThrow());
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
            yield return wait;
        }
    }
    IEnumerator SkillThrow(){
        RaycastHit2D hit = Physics2D.Raycast(player.transform.position, new Vector2(-player.Dir, 0), player.skillDist, 1<<7);
        if(hit.collider==null){
            yield return new WaitForSeconds(0.66666f);
            yield break;
        }
        yield return new WaitForSeconds(5*.0833333f);
        NailBehav.inst.transform.position=hit.point;
        NailBehav.inst.Show();
        for(int i=2;i>-1;--i){
            NailBehav.inst.SetSprite(i);
            yield return new WaitForSeconds(.083333333f);
        }
        player.animator.SetTrigger("dashToNail");
    }
}

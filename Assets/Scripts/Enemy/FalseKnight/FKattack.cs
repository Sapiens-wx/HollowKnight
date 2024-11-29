using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class FKattack : FKStateBase
{
    Coroutine coro;
    public float delayBulletTime;
    float activateBulletTime;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        coro = knight.StartCoroutine(m_FixedUpdate());
        activateBulletTime=delayBulletTime+Time.time;
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
            if(Time.time>=activateBulletTime){
                FK_Bullet.Activate((Vector2)knight.transform.position+new Vector2(knight.Dir*knight.bulletInstPos.x,knight.bulletInstPos.y), knight.Dir);
                activateBulletTime=float.MaxValue;
            }
            yield return wait;
        }
    }
}

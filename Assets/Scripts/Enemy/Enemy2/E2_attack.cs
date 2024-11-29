using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class E2_attack : E2StateBase
{
    Coroutine coro;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        coro = enemy.StartCoroutine(m_FixedUpdate());
        Vector2 dir=((Vector2)PlayerCtrl.inst.transform.position-(Vector2)enemy.transform.position).normalized;
        float cos=Mathf.Cos(enemy.bulletAngle*Mathf.Deg2Rad), sin=Mathf.Sin(enemy.bulletAngle*Mathf.Deg2Rad);
        Vector2 dir1=new Vector2(dir.x*cos-dir.y*sin,dir.x*sin+dir.y*cos);
        Vector2 dir2=new Vector2(dir.x*cos+dir.y*sin,-dir.x*sin+dir.y*cos);
        Bullet.InstantiateB(enemy.transform.position, dir, enemy.bulletSpd);
        Bullet.InstantiateB(enemy.transform.position, dir1, enemy.bulletSpd);
        Bullet.InstantiateB(enemy.transform.position, dir2, enemy.bulletSpd);
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
            yield return wait;
        }
    }
}

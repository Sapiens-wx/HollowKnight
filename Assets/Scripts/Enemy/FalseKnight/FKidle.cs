using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class FKidle : FKStateBase
{
    public float actionWaitTime, actionWaitTimeRange;
    Coroutine coro, randActCoro;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        coro = knight.StartCoroutine(m_FixedUpdate());
        randActCoro = knight.StartCoroutine(RandomAction());
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
        if (randActCoro != null)
            knight.StopCoroutine(randActCoro);
    }
    IEnumerator RandomAction()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(actionWaitTime - actionWaitTimeRange / 2, actionWaitTime + actionWaitTimeRange / 2));
        float distToX = Mathf.Abs(PlayerCtrl.inst.transform.position.x - knight.transform.position.x);
        if (distToX > knight.closeToPlayerLimit) //run towards the player
            knight.animator.SetTrigger("run");
        else { //jump
            switch(Random.Range(0, 3))
            {
                //back jump
                case 0:
                    knight.canJumpAndAttack = true;
                    knight.jumpToX = GetBackwardJumpPos(distToX);
                    knight.animator.SetTrigger("jump");
                    Debug.Log("jump backward");
                    break;
                //jump, must attack
                case 1:
                    knight.canJumpAndAttack = false;
                    knight.jumpToX = knight.transform.position.x;
                    knight.animator.SetTrigger("jump_attack");
                    Debug.Log("jump here");
                    break;
                //forward jump
                case 2:
                    //attack or not attack
                    knight.canJumpAndAttack = false;
                    knight.jumpToX = GetForwardJumpPos(distToX);
                    knight.animator.SetTrigger(Random.Range(0, 2) == 0 ? "jump" : "jump_attack");
                    Debug.Log("jump forward");
                    break;
            }
        }
        randActCoro = null;
    }
    float GetForwardJumpPos(float distToPlayer)
    {
        //far from player, should jump closer to the player.
        if (knight.dir == 1) //left side
            return Random.Range(Mathf.Max(knight.leftx, PlayerCtrl.inst.transform.position.x - knight.closeToPlayerLimit), Mathf.Min(knight.rightx, PlayerCtrl.inst.transform.position.x + knight.closeToPlayerLimit));
        else //right side
            return Random.Range(Mathf.Max(knight.leftx, PlayerCtrl.inst.transform.position.x - knight.closeToPlayerLimit), Mathf.Min(knight.rightx, PlayerCtrl.inst.transform.position.x + knight.closeToPlayerLimit));
    }
    float GetBackwardJumpPos(float distToPlayer)
    {
        if (knight.dir == 1) { //jump to left
            float rightX = PlayerCtrl.inst.transform.position.x - knight.closeToPlayerLimit;
            if (rightX < knight.leftx)
                rightX = PlayerCtrl.inst.transform.position.x;
            return Random.Range(knight.leftx, rightX);
        } else { //jump to right
            float leftX = PlayerCtrl.inst.transform.position.x + knight.closeToPlayerLimit;
            if (leftX > knight.rightx)
                leftX = PlayerCtrl.inst.transform.position.x;
            return Random.Range(leftX, knight.rightx);
        }
    }
    IEnumerator m_FixedUpdate(){
        WaitForFixedUpdate wait=new WaitForFixedUpdate();
        while(true){
            if (Mathf.Sign(PlayerCtrl.inst.transform.position.x - knight.transform.position.x) != knight.dir)
                knight.animator.SetTrigger("turn");
            yield return wait;
        }
    }
}

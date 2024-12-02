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
        WaitForFixedUpdate wait=new WaitForFixedUpdate();
        while(knight.associatedCamRoom!=CamRoom.activeRoom)
            yield return wait;
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
                    //Debug.Log("jump backward");
                    break;
                //jump, must attack
                case 1:
                    knight.canJumpAndAttack = false;
                    knight.jumpToX = knight.transform.position.x;
                    knight.animator.SetTrigger("jump_attack");
                    //Debug.Log("jump here");
                    break;
                //forward jump
                case 2:
                    //attack or not attack
                    knight.canJumpAndAttack = false;
                    knight.jumpToX = GetForwardJumpPos(distToX);
                    knight.animator.SetTrigger(Random.Range(0, 2) == 0 ? "jump" : "jump_attack");
                    //Debug.Log("jump forward");
                    break;
            }
        }
        randActCoro = null;
    }
    float GetForwardJumpPos(float distToPlayer)
    {
        //far from player, should jump closer to the player.
        if (knight.Dir == 1) //left side
            return Random.Range(knight.transform.position.x, Mathf.Min(knight.rightx, PlayerCtrl.inst.transform.position.x-knight.closestDistToPlayer));
        else //right side
            return Random.Range(knight.transform.position.x, Mathf.Max(knight.leftx, PlayerCtrl.inst.transform.position.x+knight.closestDistToPlayer));
    }
    float GetBackwardJumpPos(float distToPlayer)
    {
        if (knight.Dir == 1) { //jump to left
            float leftX = PlayerCtrl.inst.transform.position.x-knight.closeToPlayerLimit;
            if(leftX<knight.leftx)
                leftX=knight.leftx;
            return Random.Range(leftX, knight.transform.position.x);
        } else { //jump to right
            float rightX = PlayerCtrl.inst.transform.position.x+knight.closeToPlayerLimit;
            if(rightX>knight.rightx)
                rightX=knight.rightx;
            return Random.Range(knight.transform.position.x, rightX);
        }
    }
    IEnumerator m_FixedUpdate(){
        WaitForFixedUpdate wait=new WaitForFixedUpdate();
        while(true){
            if (Mathf.Sign(PlayerCtrl.inst.transform.position.x - knight.transform.position.x) != knight.Dir)
                knight.animator.SetTrigger("turn");
            yield return wait;
        }
    }
}

using DG.Tweening;
using UnityEngine;

public class Phit : PStateBase
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        player.v.x=0;
        player.v.y=0;
        player.hitAnim.Restart();
        animator.SetTrigger(player.onGround?"hit_ground":"hit_air");
        //deal damage
        PlayerBar.inst.SetCurHealth(PlayerBar.inst.CurHealth-1, player.hitBy.gameObject);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }
}

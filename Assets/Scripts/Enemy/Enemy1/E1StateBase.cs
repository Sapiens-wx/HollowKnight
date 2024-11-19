using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class E1StateBase : StateMachineBehaviour
{
    internal Enemy1 enemy;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy=animator.GetComponent<Enemy1>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //}
    internal void DetectPlayer(){
        enemy.UpdateSeesPlayer();
        if(enemy.seesPlayer){
            enemy.animator.SetTrigger("attack");
        }
    }
}

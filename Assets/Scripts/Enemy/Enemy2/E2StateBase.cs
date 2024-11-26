using UnityEngine;

public class E2StateBase : StateMachineBehaviour
{
    internal Enemy2 enemy;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy=animator.GetComponent<Enemy2>();
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
    internal bool TryAttack(){
        enemy.rgb.velocity=Vector2.zero;
        if(Time.time>=enemy.allowAttackTime){
            enemy.allowAttackTime=Time.time+enemy.attackInterval;
            enemy.animator.SetTrigger("attack");
            return true;
        } else{
            enemy.animator.SetTrigger("wait");
            return false;
        }
    }
    internal void HorizontalMovement(){
        float w=6.2831f*enemy.patrolFrq;
        enemy.rgb.velocity=new Vector2(Mathf.Cos(w*Time.time)*enemy.patrolDist*w, enemy.rgb.velocity.y);
    }
    internal void VerticalMovement(){
        float w=6.2831f*enemy.verticalFrq;
        enemy.rgb.velocity=new Vector2(enemy.rgb.velocity.x, Mathf.Cos(w*Time.time)*enemy.verticalDist*w);
    }
}

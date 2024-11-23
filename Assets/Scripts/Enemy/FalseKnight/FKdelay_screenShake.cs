using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class FKdelay_screenShake : FKStateBase
{
    public float delayAmount;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        CameraCtrl.inst.StartCoroutine(DelayScreenShake());
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
    IEnumerator DelayScreenShake(){
        yield return new WaitForSeconds(delayAmount);
        CameraCtrl.inst.ScreenShakeCM();
    }
}

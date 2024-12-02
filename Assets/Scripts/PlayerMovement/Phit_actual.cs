using UnityEngine;

public class Phit_actual : PStateBase{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        PlayerCtrl.inst.bc.enabled=false;
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //deal damage
        PlayerBar.inst.SetCurHealth(PlayerBar.inst.CurHealth-1, player.hitBy);
        PlayerCtrl.inst.bc.enabled=true;
    }
}
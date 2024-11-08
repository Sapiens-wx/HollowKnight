using UnityEngine;

public class SwordBehav : MonoBehaviour {
    void OnCollisionEnter2D(Collision2D collider){
        if(PlayerCtrl.inst.attack_down && collider.collider.CompareTag("hittable")){
            CameraCtrl.inst.StartCoroutine(CameraCtrl.inst.ScreenShake());
            PlayerCtrl.inst.animator.SetTrigger("attack_down_jump_up");
            PlayerCtrl.inst.v.y=PlayerCtrl.inst.downSlashJumpSpd;
        }
    }
}
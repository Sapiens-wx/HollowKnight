using UnityEngine;

public class SwordBehav : MonoBehaviour {
    void OnTriggerEnter2D(Collider2D collider){
        if(PlayerCtrl.inst.attack_down && collider.CompareTag("hittable")){
            CameraCtrl.inst.StartCoroutine(CameraCtrl.inst.ScreenShake());
            PlayerCtrl.inst.animator.SetTrigger("attack_down_jump_up");
            PlayerCtrl.inst.v.y=PlayerCtrl.inst.downSlashJumpSpd;
        }
    }
}
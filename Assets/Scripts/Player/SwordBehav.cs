using UnityEngine;

public class SwordBehav : MonoBehaviour {
    void OnTriggerEnter2D(Collider2D collider){
        if(PlayerCtrl.inst.attack_down && (collider.CompareTag("hittable")||collider.CompareTag("spike"))){
            CameraCtrl.inst.ScreenShakeCM();
            PlayerCtrl.inst.animator.SetTrigger("attack_down_jump_up");
        }
    }
}
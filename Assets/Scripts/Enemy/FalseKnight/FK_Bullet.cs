using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FK_Bullet : MonoBehaviour
{
    public float spd;
    public float startScale, scaleSpd;
    public static FK_Bullet inst;
    float framedSpd, framedScaleSpd;
    void Awake(){
        inst=this;
        gameObject.SetActive(false);
    }
    public static void Activate(Vector2 pos, int dir){
        //make sure the bullet sticks to the ground
        RaycastHit2D hit=Physics2D.Raycast(pos, Vector2.down, float.MaxValue, GameManager.inst.groundLayer);
        pos=hit.point;
        pos.y+=inst.transform.localScale.y*.285f;
        
        inst.transform.position=pos;
        inst.framedSpd=Time.fixedDeltaTime*inst.spd*dir;
        inst.framedScaleSpd=Time.fixedDeltaTime*inst.scaleSpd;
        inst.transform.localScale=new Vector3(inst.startScale,inst.transform.localScale.y,inst.transform.localScale.z);
        inst.gameObject.SetActive(true);
    }
    void FixedUpdate(){
        transform.position+=new Vector3(framedSpd,0,0);
        transform.localScale+=new Vector3(framedScaleSpd,0,0);
        if(transform.position.x>FalseKnight.inst.rightx || transform.position.x<FalseKnight.inst.leftx)
            gameObject.SetActive(false);
    }
}

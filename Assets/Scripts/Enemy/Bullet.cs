using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] Rigidbody2D rgb;
    [SerializeField] GameObject damageBox;
    void OnEnable(){
        PlayerCtrl.inst.OnPlayerHit+=OnPlayerHit;
        Destroy(gameObject, 15);
    }
    void OnDisable(){
        PlayerCtrl.inst.OnPlayerHit-=OnPlayerHit;
    }
    void OnPlayerHit(){
        if(PlayerCtrl.inst.hitBy.gameObject==damageBox){ //if the player is hit by this gameObject
            Destroy(gameObject);
        }
    }
    void OnCollisionEnter2D(Collision2D collision){
        Destroy(gameObject);
    }
    public static Bullet InstantiateB(Vector3 pos, Vector2 dir, float spd){
        Bullet ret=Instantiate(GameManager.inst.bullet_enemy2.gameObject, pos, Quaternion.identity).GetComponent<Bullet>();
        ret.transform.rotation=Quaternion.AngleAxis(Vector2.SignedAngle(Vector2.left, dir), Vector3.forward);
        ret.rgb.velocity=dir*spd;
        return ret;
    }
}

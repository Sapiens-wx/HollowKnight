using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class NailBehav : MonoBehaviour
{
    [SerializeField] float maxThrowDist, throwSpd;

    public static NailBehav inst;
    Rigidbody2D rgb;
    Coroutine hitDetectCoro;
    [HideInInspector] public HitType hitType;
    void Awake(){
        inst=this;
    }
    void Start(){
        rgb=GetComponent<Rigidbody2D>();
        gameObject.SetActive(false);
    }
    public void Thrown(){
        //set position and scale
        gameObject.SetActive(true);
        transform.position=PlayerCtrl.inst.transform.position+(Vector3)PlayerCtrl.inst.throwNailOffset;
        Vector3 scale=transform.localScale;
        scale.x=Mathf.Abs(scale.x)*PlayerCtrl.inst.Dir;
        transform.localScale=scale;
        float throwDist=maxThrowDist*PlayerCtrl.inst.throwChargeTime/PlayerCtrl.inst.maxThrowChargeTime;
        float throwInterval=throwDist/throwSpd;

        Sequence s=DOTween.Sequence();
        //throw forward
        s.Append(transform.DOMoveX(transform.position.x-PlayerCtrl.inst.Dir*throwDist, throwInterval));
        //come back
        s.Append(transform.DOMoveX(transform.position.x, throwInterval/2).SetEase(Ease.Linear));
        s.AppendCallback(()=>{
            PlayerCtrl.inst.animator.SetTrigger("throw_recover");
            Hide();
            });
    }
    IEnumerator HitDetection(){
        yield break;
    }
    void OnTriggerEnter2D(Collider2D collider){
        return;
        if(hitDetectCoro!=null){
            StopCoroutine(hitDetectCoro);
            hitDetectCoro=null;
        }
        if(collider.gameObject.layer==8)
            hitType=HitType.Enemy;
        else if(collider.gameObject.layer==7)
            hitType=HitType.Wall;
        rgb.velocity=Vector2.zero;
        PlayerCtrl.inst.animator.SetTrigger("dashToNail");
    }
    public void Hide(){
        gameObject.SetActive(false);
    }
    public enum HitType{
        Enemy,
        Wall
    }
}

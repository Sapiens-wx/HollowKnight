using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class NailBehav : MonoBehaviour
{
    [SerializeField] float maxThrowDist, throwSpd;
    [SerializeField] Sprite[] sprites;

    public static NailBehav inst;
    Rigidbody2D rgb;
    SpriteRenderer spr;
    Coroutine animCoro;
    [HideInInspector] public HitType hitType;
    void Awake(){
        inst=this;
    }
    void Start(){
        rgb=GetComponent<Rigidbody2D>();
        spr=GetComponent<SpriteRenderer>();
        gameObject.SetActive(false);
    }
    public void Thrown(){
        //set position and scale
        Show();
        transform.position=PlayerCtrl.inst.transform.position+(Vector3)PlayerCtrl.inst.throwNailOffset;
        float throwDist=maxThrowDist*PlayerCtrl.inst.throwChargeTime/PlayerCtrl.inst.maxThrowChargeTime;
        float throwInterval=throwDist/throwSpd;
        //animation
        animCoro=StartCoroutine(Anim());

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
    IEnumerator Anim(){
        WaitForSeconds wait=new WaitForSeconds(.08333333f);
        int sprIdx=0;
        while(true){
            spr.sprite=sprites[sprIdx++];
            if(sprIdx>=3)
                sprIdx=0;
            yield return wait;
        }
    }
    void OnTriggerEnter2D(Collider2D collider){
        return;
    }
    public void Show(){
        Vector3 scale=transform.localScale;
        scale.x=Mathf.Abs(scale.x)*PlayerCtrl.inst.Dir;
        transform.localScale=scale;
        gameObject.SetActive(true);
    }
    public void Hide(){
        gameObject.SetActive(false);
        if(animCoro!=null){
            StopCoroutine(animCoro);
            animCoro=null;
        }
    }
    public void SetSprite(int i){
        spr.sprite=sprites[i];
    }
    public enum HitType{
        Enemy,
        Wall
    }
}

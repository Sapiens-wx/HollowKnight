using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] internal int maxHealth;
    [SerializeField] internal Animator animator;
    [SerializeField] float hitAnimDuration;
    [SerializeField] SpriteRenderer spr;
    [SerializeField] Material material;
    MaterialPropertyBlock matPB;

    internal int curHealth;

    Sequence s;
    internal virtual void Start(){
        curHealth=maxHealth;
        matPB=new MaterialPropertyBlock();
        spr.GetPropertyBlock(matPB);
        matPB.SetColor("_Color", Color.black);
        spr.SetPropertyBlock(matPB);

        s=DOTween.Sequence();
        s.SetAutoKill(false);
        s.Append(DOTween.To(()=>matPB.GetColor("_Color"),(val)=>{
            spr.GetPropertyBlock(matPB);
            matPB.SetColor("_Color",val);
            spr.SetPropertyBlock(matPB);
        }, Color.white, hitAnimDuration).SetLoops(2, LoopType.Yoyo));
        s.Pause();
    }
    public virtual void Hit(int damage){
        curHealth-=damage;
        if (curHealth < 0) {
            animator.SetTrigger("die");
        }
    }
    void OnTriggerEnter2D(Collider2D collider){
        if(collider.gameObject.layer==9){ //if is sword layer
            Hit(1);
            s.Restart();
        }
    }
}

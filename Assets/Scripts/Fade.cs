using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;

public class Fade : MonoBehaviour
{
    [SerializeField] Image img;
    [SerializeField] float duration, pauseInterval;
    public static Fade inst;
    bool fadeInProgress;
    void Awake(){
        if(inst!=null)
            Destroy(gameObject);
        else{
            DontDestroyOnLoad(gameObject);
            inst=this;
        }
    }
    public static bool BlackInOut(TweenCallback onHalfComplete, TweenCallback onComplete){
        if(inst.fadeInProgress) return false;
        inst.fadeInProgress=true;
        Sequence s=DOTween.Sequence();
        inst.img.color=new Color(0,0,0,0);
        s.Append(DOTween.To(()=>inst.img.color.a,val=>inst.img.color=new Color(inst.img.color.r,inst.img.color.g,inst.img.color.b,val), 1, inst.duration));
        if(onHalfComplete!=null)
            s.AppendCallback(onHalfComplete);
        s.AppendInterval(inst.pauseInterval);
        s.Append(DOTween.To(()=>inst.img.color.a,val=>inst.img.color=new Color(inst.img.color.r,inst.img.color.g,inst.img.color.b,val), 0, inst.duration));
        if(onComplete!=null)
            s.AppendCallback(onComplete);
        s.AppendCallback(()=>{
            inst.fadeInProgress=false;
        });
        return true;
    }
}

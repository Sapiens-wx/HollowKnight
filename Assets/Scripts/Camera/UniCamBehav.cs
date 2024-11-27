using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UniCamBehav : MonoBehaviour
{
    public static UniCamBehav inst;
    public CinemachineCameraOffset vmOffset;
    public static void ChangeXOffset(float val){
        if(inst==null) return;
        DOTween.To(()=>inst.vmOffset.m_Offset.x, (val)=>{
            inst.vmOffset.m_Offset=new Vector3(val, inst.vmOffset.m_Offset.y,inst.vmOffset.m_Offset.z);
        }, val, .5f);
    }
    void OnEnable(){
        inst=this;
    }
    void OnDisable(){
        inst=null;
    }
}

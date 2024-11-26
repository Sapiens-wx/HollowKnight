using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UniCamBehav : MonoBehaviour
{
    public static UniCamBehav inst;
    public CinemachineCameraOffset vmOffset;
    public void ChangeXOffset(float val){
        DOTween.To(()=>vmOffset.m_Offset.x, (val)=>{
            vmOffset.m_Offset=new Vector3(val, vmOffset.m_Offset.y,vmOffset.m_Offset.z);
        }, val, .5f);
    }
    void OnEnable(){
        inst=this;
    }
    void OnDisable(){
        inst=null;
    }
}

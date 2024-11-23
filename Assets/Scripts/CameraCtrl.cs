using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;
using System;

public class CameraCtrl : MonoBehaviour
{
    [SerializeField] float limitx,limity;
    [SerializeField] float lerpAmount;
    [Header("Screen Shake")]
    [SerializeField] CinemachineImpulseSource impulseSrc;
    [SerializeField] float shakeAmount;
    [SerializeField] float shakeInterval;

    public static CameraCtrl inst;
    void OnDrawGizmosSelected(){
        Gizmos.color=Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(limitx*2,limity*2,0));
    }
    Vector3 camPos;
    void Awake(){
        inst=this;
    }
    void Start()
    {
        camPos=transform.position;
    }

    void FixedUpdate(){
        //camPos.x=Mathf.Lerp(transform.position.x, PlayerCtrl.inst.transform.position.x-limitx*PlayerCtrl.inst.Dir, lerpAmount);
        //camPos.y=Mathf.Lerp(transform.position.y, PlayerCtrl.inst.transform.position.y, lerpAmount);
        //transform.position=camPos;
    }
    public void ScreenShakeCM(){
        impulseSrc.GenerateImpulse(Vector2.one);
    }
    [Obsolete]
    public IEnumerator ScreenShake(){
        WaitForFixedUpdate wait=new WaitForFixedUpdate();
        float t=0;
        while(t<shakeInterval){
            camPos.x=UnityEngine.Random.Range(-shakeAmount,shakeAmount)+transform.position.x;
            camPos.y=UnityEngine.Random.Range(-shakeAmount,shakeAmount)+transform.position.y;
            transform.position=camPos;
            t+=Time.fixedDeltaTime;
            yield return wait;
        }
    }
}

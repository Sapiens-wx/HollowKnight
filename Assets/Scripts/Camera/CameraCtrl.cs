using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;
using System;

public class CameraCtrl : MonoBehaviour
{
    CinemachineVirtualCamera vm;
    [Header("Screen Shake")]
    [SerializeField] CinemachineImpulseSource impulseSrc;

    public static CameraCtrl inst;
    void Awake(){
        inst=this;
    }

    void FixedUpdate(){
    }
    public void ScreenShakeCM(){
        impulseSrc.GenerateImpulse(Vector2.one);
    }
    public void ChangeDir(){
    }
}

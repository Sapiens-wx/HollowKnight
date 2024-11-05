using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraCtrl : MonoBehaviour
{
    [SerializeField] float limitx,limity;
    [SerializeField] float lerpAmount;
    void OnDrawGizmosSelected(){
        Gizmos.color=Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(limitx*2,limity*2,0));
    }
    Vector3 camPos;
    void Start()
    {
        camPos=transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void FixedUpdate(){
        camPos.x=Mathf.Lerp(camPos.x, PlayerCtrl.inst.transform.position.x-limitx*PlayerCtrl.inst.Dir, lerpAmount);
        camPos.y=Mathf.Lerp(camPos.y, PlayerCtrl.inst.transform.position.y, lerpAmount);
        transform.position=camPos;
    }
}

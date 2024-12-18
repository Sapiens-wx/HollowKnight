using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MovablePlatform : MonoBehaviour
{
    public float speed;

    float originalYPos;
    bool playerStay;
    void Start(){
        originalYPos=transform.position.y;
        playerStay=false;
    }
    void FixedUpdate(){
        if(playerStay){
            transform.position+=new Vector3(0,speed,0);
        } else{
            if(transform.position.y>originalYPos)
                transform.position+=new Vector3(0,-speed,0);
        }
    }
    public void OnPlayerCollide(){
        playerStay=true;
    }
    public void OnPlayerCollideExit(){
        playerStay=false;
    }
}

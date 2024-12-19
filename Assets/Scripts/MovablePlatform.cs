using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MovablePlatform : MonoBehaviour
{
    public float speed;
    public Color toColor;

    SpriteRenderer spr;
    float originalYPos;
    bool playerStay;
    Color originalColor;
    void Start(){
        spr=GetComponent<SpriteRenderer>();
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
        originalColor=spr.color;
        spr.color=toColor;
        playerStay=true;
    }
    public void OnPlayerCollideExit(){
        spr.color=originalColor;
        playerStay=false;
    }
}

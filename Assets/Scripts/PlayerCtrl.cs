using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    public Rigidbody2D rgb;
    [SerializeField] float xspd;

    Vector2 v;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void FixedUpdate(){
        Movement();
        UpdateVelocity();
    }
    void UpdateVelocity(){
        rgb.velocity=v;
    }
    void Movement(){
        float x=Input.GetAxisRaw("Horizontal");
        v.x=x*xspd;
        v.y=rgb.velocity.y;
    }
}

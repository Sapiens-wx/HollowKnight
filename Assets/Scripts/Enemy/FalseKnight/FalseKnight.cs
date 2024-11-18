using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FalseKnight : EnemyBase
{
    [Header("run")]
    public float runSpd;
    [Header("jump")]
    public float jumpHeight;
    public float jumpInterval;
    public float leftx, rightx, closeToPlayerLimit;
    [Header("Ground Check")]
    public Vector2 leftBot;
    public Vector2 rightBot;
    public LayerMask groundLayer;

    public static FalseKnight inst;
    [HideInInspector] public int dir;
    [HideInInspector] public Rigidbody2D rgb;
    //ground check
    [HideInInspector] public bool onGround, prevOnGround;
    //jump/jump_attack state
    [HideInInspector] public float jumpToX;
    [HideInInspector] public bool canJumpAndAttack;
    private void OnDrawGizmosSelected()
    {
        //ground check
        Gizmos.DrawLine((Vector2)transform.position+leftBot, (Vector2)transform.position+rightBot);
        //bounds
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(new Vector2(leftx, -10), new Vector2(rightx, -10));
    }
    private void Awake() {
        inst = this;
    }
    internal override void Start()
    {
        base.Start();
        dir = 1;
        rgb = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        CheckOnGround();
    }
    void CheckOnGround(){
        prevOnGround=onGround;
        onGround = Physics2D.OverlapArea((Vector2)transform.position+leftBot, (Vector2)transform.position+rightBot, groundLayer);
    }
}

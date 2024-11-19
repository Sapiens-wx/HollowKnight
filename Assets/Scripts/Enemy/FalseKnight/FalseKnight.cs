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

    public static FalseKnight inst;
    //jump/jump_attack state
    [HideInInspector] public float jumpToX;
    [HideInInspector] public bool canJumpAndAttack;
    private void OnDrawGizmosSelected()
    {
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
        Dir=1;
    }
    private void FixedUpdate()
    {
        CheckOnGround();
    }
}

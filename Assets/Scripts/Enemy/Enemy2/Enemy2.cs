using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : EnemyBase
{
    [SerializeField] Bounds detectBounds, attackBounds;
    public float patrolDist, patrolFrq;
    public float verticalDist, verticalFrq;
    public float chaseSpd;
    [Header("Attack")]
    public float attackInterval;
    public float bulletSpd;

    [HideInInspector] public Vector2 spawnPos;
    [HideInInspector] public float allowAttackTime;
    void OnDrawGizmosSelected(){
        Gizmos.DrawWireCube(transform.position+detectBounds.center, detectBounds.size);
        Gizmos.color=Color.red;
        Gizmos.DrawWireCube(transform.position+attackBounds.center, attackBounds.size);
        Gizmos.color=Color.blue;
        Gizmos.DrawLine(new Vector2(transform.position.x-patrolDist, transform.position.y), new Vector2(transform.position.x+patrolDist, transform.position.y));
    }
    // Start is called before the first frame update
    internal override void Start()
    {
        base.Start();
        spawnPos=transform.position;
        allowAttackTime=0;
    }

    public bool DetectPlayer(){
        Vector2 min=detectBounds.min+transform.position;
        Vector2 max=detectBounds.max+transform.position;
        Vector2 pos=PlayerCtrl.inst.transform.position;
        return !(min.x>pos.x||min.y>pos.y||max.x<pos.x||max.y<pos.y);
    }
    public bool InsideAttackBounds(){
        Vector2 min=attackBounds.min+transform.position;
        Vector2 max=attackBounds.max+transform.position;
        Vector2 pos=PlayerCtrl.inst.transform.position;
        return !(min.x>pos.x||min.y>pos.y||max.x<pos.x||max.y<pos.y);
    }
}

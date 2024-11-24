using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBar : MonoBehaviour
{
    [SerializeField] int maxHealth;
    [SerializeField] Life lifePrefab;
    [SerializeField] Transform livesGrid;

    public static PlayerBar inst;
    private int curHealth;
    List<Life> lives;
    public int MaxHealth{
        get=>maxHealth;
        set{
            maxHealth=value;
            for(int i=lives.Count;i<maxHealth;++i){
                Life go = Instantiate(lifePrefab).GetComponent<Life>();
                go.transform.SetParent(livesGrid);
                lives.Add(go);
            }
        }
    }
    public int CurHealth{
        get=>curHealth;
    }
    void Awake(){
        inst=this;
    }
    void Start(){
        lives=new List<Life>();
        MaxHealth=maxHealth;
        SetCurHealth(maxHealth, null);
    }
    void OnTriggerStay2D(Collider2D collider){
        //if is enemy, player is hit
        if(PlayerCtrl.inst.hittable && GameManager.IsLayer(GameManager.inst.enemyLayer, collider.gameObject.layer)){ 
            PlayerCtrl.inst.animator.SetTrigger("hit");
            //deal damage
            SetCurHealth(curHealth-1, collider.gameObject);
        }
    }
    public void SetCurHealth(int value, GameObject from){
        curHealth=value;
        for(int i=lives.Count-1;i>=0;--i){
            lives[i].TurnOn(i<curHealth);
        }
        //if touches spike, temporarily die
        if(!(from==null || !from.CompareTag("spike")) && curHealth>0){
            SavePoint.RevivePlayer();
        }
        //die
        if(curHealth<=0){
            SetCurHealth(maxHealth, null);
            RevivePoint.RevivePlayer();
        }
    }
}

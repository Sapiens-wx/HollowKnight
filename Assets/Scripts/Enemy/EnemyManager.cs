using UnityEngine;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour{
    public static EnemyManager inst;
    List<EnemyBase> enemies;
    void Awake(){
        inst=this;
        enemies=new List<EnemyBase>();
    }
    public void RegisterEnemy(EnemyBase enemy){
        enemies.Add(enemy);
    }
    public void RecoverEnemies(){
        foreach(EnemyBase e in enemies){
            e.Recover();
        }
    }
}
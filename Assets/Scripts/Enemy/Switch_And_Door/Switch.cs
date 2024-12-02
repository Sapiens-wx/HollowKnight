using UnityEngine;

public class Switch : EnemyBase{
    public GameObject[] connectedDoors;
    public override void Hit(){
        foreach(GameObject door in connectedDoors){
            Destroy(door);
        }
        bc.enabled=false;
    }
}
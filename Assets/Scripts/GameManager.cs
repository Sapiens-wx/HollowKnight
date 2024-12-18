using UnityEngine;

public class GameManager : MonoBehaviour{
    public LayerMask groundLayer,enemyLayer,playerSwordLayer,playerLayer,movablePlatformLayer;
    public Bullet bullet_enemy2;

    public static GameManager inst;
    void Awake(){
        inst=this;
    }
    public static bool IsLayer(LayerMask mask, int layer){
        return ((1<<layer)&mask.value)!=0;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour
{    
    [SerializeField] Bounds bounds;

    static SavePoint lastSavePoint;
    Vector2 min,max;
    void OnDrawGizmosSelected(){
        Gizmos.DrawWireCube(bounds.center+transform.position, bounds.size);
    }
    void Start(){
        min=bounds.min+transform.position;
        max=bounds.max+transform.position;
        StartCoroutine(Detection());
    }
    IEnumerator Detection(){
        yield return new WaitForSeconds(Random.Range(0.01f,0.75f));
        WaitForSeconds wait=new WaitForSeconds(.4f);
        while(true){
            Vector2 pos=PlayerCtrl.inst.transform.position;
            if(!(min.x>pos.x || max.x<pos.x || min.y>pos.y || max.y<pos.y)){
                lastSavePoint=this;
            }
            yield return wait;
        }
    }
    public static void RevivePlayer(){
        if(lastSavePoint==null){
            RevivePoint.RevivePlayer();
            return;
        }
        PlayerCtrl.inst.transform.position=lastSavePoint.transform.position+new Vector3(0,PlayerCtrl.inst.bc.bounds.extents.y-PlayerCtrl.inst.bc.offset.y,0);
    }

}

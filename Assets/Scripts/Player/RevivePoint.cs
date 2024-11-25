using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevivePoint : MonoBehaviour
{    
    [SerializeField] bool defaultRevivePoint;
    [SerializeField] Bounds bounds;

    static RevivePoint lastRevivePoint;
    bool intersecting;
    Vector2 min,max;
    void OnDrawGizmosSelected(){
        Gizmos.DrawWireCube(bounds.center+transform.position, bounds.size);
    }
    void Start(){
        if(defaultRevivePoint)
            lastRevivePoint=this;
        min=bounds.min+transform.position;
        max=bounds.max+transform.position;
        StartCoroutine(Detection());
    }
    IEnumerator Detection(){
        yield return new WaitForSeconds(Random.Range(0.01f,0.75f));
        WaitForSeconds wait=new WaitForSeconds(.4f);
        while(true){
            Vector2 pos=PlayerCtrl.inst.transform.position;
            intersecting=!(min.x>pos.x || max.x<pos.x || min.y>pos.y || max.y<pos.y);
            yield return wait;
        }
    }
    void Update(){
        if(intersecting && Input.GetKeyDown(KeyCode.E)){
            lastRevivePoint=this;
        }
    }
    public static void RevivePlayer(){
        PlayerCtrl.inst.transform.position=lastRevivePoint.transform.position+new Vector3(0,PlayerCtrl.inst.bc.bounds.extents.y-PlayerCtrl.inst.bc.offset.y,0);
    }
}
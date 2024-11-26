using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRoom : MonoBehaviour
{
    [SerializeField] Bounds bounds;
    [SerializeField] GameObject vm;
    
    public static CamRoom activeRoom;
    bool intersect, prevIntersect;
    void OnDrawGizmosSelected(){
        Gizmos.DrawWireCube(transform.position+bounds.center, bounds.size);
    }
    bool IntersectBounds(){
        Vector2 min=bounds.min+transform.position;
        Vector2 max=bounds.max+transform.position;
        Vector2 pos=PlayerCtrl.inst.transform.position;
        return !(min.x>pos.x||min.y>pos.y||max.x<pos.x||max.y<pos.y);
    }
    static void SetActiveRoom(CamRoom room){
        if(room==null){
            activeRoom=null;
        }else{
            if(activeRoom!=null)
                activeRoom.vm.SetActive(false);
            activeRoom=room;
            room.vm.SetActive(true);
        }
    }
    void FixedUpdate(){
        //if(activeRoom==null || activeRoom==this){
            prevIntersect=intersect;
            intersect=IntersectBounds();
            if(!prevIntersect && intersect){ //on enter
                SetActiveRoom(this);
            } else if(prevIntersect && !intersect){ //on exit
            }
        //}
    }
}

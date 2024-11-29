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
    Vector2 GetPosForUpdatePlayerPos(){
        return new Vector2(PlayerCtrl.inst.transform.position.x, bounds.min.y+transform.position.y);
    }
    void UpdatePlayerPos(Vector2 pos){

        RaycastHit2D hit=Physics2D.Raycast(pos, new Vector2(-PlayerCtrl.inst.Dir,0),float.MaxValue,GameManager.inst.groundLayer);
        if(!hit){
            Debug.LogError("in CamRoom::GeneratePlayerPos, ray cast did not hit");
            return;
        }
        Vector2 extents=PlayerCtrl.inst.bc.bounds.extents;
        Vector2 offset=PlayerCtrl.inst.bc.offset;
        Vector2 newPos=new Vector2(hit.point.x, 0);
        newPos+=new Vector2(
            PlayerCtrl.inst.Dir==-1?extents.x-offset.x:offset.x-extents.x,
            hit.collider.bounds.extents.y-hit.collider.offset.y+hit.transform.position.y+extents.y-offset.y);
        PlayerCtrl.inst.transform.position=newPos;
    }
    static void SetActiveRoom(CamRoom room){
        if(room==null){
            activeRoom=null;
        }else{
            if(activeRoom!=null)
                activeRoom.vm.SetActive(false);
            activeRoom=room;
            //if player jumped upward to this room
            if(PlayerCtrl.inst.transform.position.y<activeRoom.bounds.min.y+activeRoom.transform.position.y+.4f){
                PlayerCtrl.inst.v=Vector2.zero;
                activeRoom.UpdatePlayerPos(activeRoom.GetPosForUpdatePlayerPos());
            }

            PlayerCtrl.inst.ReadInput=false;
            if(!Fade.BlackInOut(()=>{
                room.vm.SetActive(true);
            },()=>{
                PlayerCtrl.inst.ReadInput=true;
            })){
                room.vm.SetActive(true);
                PlayerCtrl.inst.ReadInput=true;
            }
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

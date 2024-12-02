using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRoom : MonoBehaviour
{
    [SerializeField] Bounds bounds;
    [SerializeField] GameObject vm;
    [SerializeField] bool overrideSpawnPos; //used when the player jumps up into a camRoom
    [SerializeField] Vector2 leftSpawnPos, rightSpawnPos; //should stick to the ground
    
    public static CamRoom activeRoom;
    static List<CamRoom> camRooms;
    [HideInInspector] public static bool isSwitchingRoom; //if is true, the player stops updating velocity and its velocity is set to 0
    public event System.Action onPlayerExitRoom, onPlayerEnterRoom;
    bool intersect, prevIntersect;
    void OnDrawGizmosSelected(){
        Gizmos.DrawWireCube(transform.position+bounds.center, bounds.size);
        if(overrideSpawnPos){
            Gizmos.DrawWireSphere(leftSpawnPos, .5f);
            Gizmos.DrawWireSphere(rightSpawnPos, .5f);
        }
    }
    void Awake(){
        if(camRooms==null || camRooms[0]==null) camRooms=new List<CamRoom>();
        camRooms.Add(this);
    }
    /// <summary>
    /// returns which cam room is the given position in
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public static CamRoom WhichCamRoom(Vector2 pos){
        foreach(CamRoom cr in camRooms){
            if(cr.IntersectBounds(pos)) return cr;
        }
        return null;
    }
    bool IntersectBounds(Vector2 pos){
        Vector2 min=bounds.min+transform.position;
        Vector2 max=bounds.max+transform.position;
        return !(min.x>pos.x||min.y>pos.y||max.x<pos.x||max.y<pos.y);
    }
    Vector2 GetPosForUpdatePlayerPos(){
        return new Vector2(PlayerCtrl.inst.transform.position.x, bounds.min.y+transform.position.y);
    }
    void UpdatePlayerPos(Vector2 pos){
        if(overrideSpawnPos){
            PlayerCtrl.inst.transform.position=PlayerCtrl.inst.Dir==-1?
                new Vector2(rightSpawnPos.x,rightSpawnPos.y+PlayerCtrl.inst.bc.bounds.extents.y):
                new Vector2(leftSpawnPos.x,leftSpawnPos.y+PlayerCtrl.inst.bc.bounds.extents.y);
            return;
        }
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
            activeRoom.onPlayerExitRoom?.Invoke();
            activeRoom=null;
        }else{
            CamRoom prevActiveRoom=null;
            if(activeRoom!=null){
                prevActiveRoom=activeRoom;
                prevActiveRoom.vm.SetActive(false);
            }

            activeRoom=room;
            //if player jumped upward to this room
            if(PlayerCtrl.inst.transform.position.y<activeRoom.bounds.min.y+activeRoom.transform.position.y+.4f){
                PlayerCtrl.inst.v=Vector2.zero;
                activeRoom.UpdatePlayerPos(activeRoom.GetPosForUpdatePlayerPos());
            } //if jumped downward to this room
            else if(PlayerCtrl.inst.transform.position.y>activeRoom.bounds.max.y+activeRoom.transform.position.y-.4f){

            }
            //if jumped rightward to this room
            else if(PlayerCtrl.inst.transform.position.x<activeRoom.transform.position.x)
                PlayerCtrl.inst.transform.position+=new Vector3(.5f,0,0);
            //if jumped leftward into this room
            else
                PlayerCtrl.inst.transform.position+=new Vector3(-.5f,0,0);

            PlayerCtrl.inst.ReadInput=false;
            isSwitchingRoom=true;
            if(!Fade.BlackInOut(()=>{
                room.vm.SetActive(true);
                if(prevActiveRoom!=null)
                    prevActiveRoom.onPlayerExitRoom?.Invoke();
                activeRoom.onPlayerEnterRoom?.Invoke();
                },()=>{
                PlayerCtrl.inst.ReadInput=true;
                isSwitchingRoom=false;
                }))
            {
                room.vm.SetActive(true);
                if(prevActiveRoom!=null)
                    prevActiveRoom.onPlayerExitRoom?.Invoke();
                activeRoom.onPlayerEnterRoom?.Invoke();
                PlayerCtrl.inst.ReadInput=true;
                isSwitchingRoom=false;
            }
        }
    }
    void FixedUpdate(){
        //if(activeRoom==null || activeRoom==this){
            prevIntersect=intersect;
            intersect=IntersectBounds(PlayerCtrl.inst.transform.position);
            if(!prevIntersect && intersect){ //on enter
                SetActiveRoom(this);
            } else if(prevIntersect && !intersect){ //on exit
            }
        //}
    }
}

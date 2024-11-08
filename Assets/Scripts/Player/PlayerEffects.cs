using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerEffects : MonoBehaviour
{
    [SerializeField] SpriteRenderer dashEffect;
    [SerializeField] float dashInterval;
    [SerializeField] Sprite[] dashEffectSprs;

    public static PlayerEffects inst;
    Vector3 dashEffectOffset;
    void Awake(){
        inst=this;
    }
    void Start()
    {
        //dash effect
        dashEffectOffset=dashEffect.transform.position-transform.position;
        dashEffect.gameObject.SetActive(false);
    }

    public void PlayDashEffect(){
        dashEffectOffset.x=Mathf.Abs(dashEffectOffset.x)*-PlayerCtrl.inst.dir;
        dashEffect.transform.localScale=new Vector3(PlayerCtrl.inst.dir,1,1);
        dashEffect.transform.position=transform.position+dashEffectOffset;
        dashEffect.gameObject.SetActive(true);
        StartCoroutine(DashEffectAnim());
    }
    IEnumerator DashEffectAnim(){
        WaitForSeconds wait=new WaitForSeconds(dashInterval);
        for(int i=0;i<dashEffectSprs.Length;++i){
            dashEffect.sprite=dashEffectSprs[i];
            yield return wait;
        }
        dashEffect.gameObject.SetActive(false);
    }
}

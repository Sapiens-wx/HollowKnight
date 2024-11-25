using UnityEngine;
using UnityEngine.UI;

public class Energy : MonoBehaviour
{
    [SerializeField] Image indicator;
    public void TurnOn(bool val){
        indicator.gameObject.SetActive(val);
        if(val)
            indicator.color=Color.white;
    }
    public void HalfOn(){
        indicator.gameObject.SetActive(true);
        indicator.color=new Color(.5f,.5f,.5f,1);
    }
}

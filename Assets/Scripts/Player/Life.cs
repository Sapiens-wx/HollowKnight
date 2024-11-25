using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Life:MonoBehaviour{
    [SerializeField] GameObject indicator;
    public void TurnOn(bool val){
        indicator.SetActive(val);
    }
}
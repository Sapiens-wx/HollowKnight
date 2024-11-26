using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBar : MonoBehaviour
{
    //!!! maxEnergy: when set in inspector, if you want maxEnergy to be 5, 
    //               set to 5. the script will initialize it to 10. maxEnergy
    //               is always storing two times the actual maxEnergy
    [SerializeField] int maxHealth, maxEnergy; 
    [SerializeField] Life lifePrefab;
    [SerializeField] Energy energyPrefab;
    [SerializeField] Transform livesGrid, energyGrid;

    public static PlayerBar inst;
    private int curHealth;
    private int curEnergy;
    List<Life> lives;
    List<Energy> energies;
    public int MaxHealth{
        get=>maxHealth;
        set{
            maxHealth=value;
            for(int i=lives.Count;i<maxHealth;++i){
                Life go = Instantiate(lifePrefab).GetComponent<Life>();
                go.transform.SetParent(livesGrid);
                lives.Add(go);
            }
        }
    }
    public int CurHealth{
        get=>curHealth;
    }
    public int MaxEnergy{
        get=>maxEnergy;
        set{
            maxEnergy=value;
            int actualMaxE=maxEnergy>>1;
            for(int i=energies.Count;i<actualMaxE;++i){
                Energy go = Instantiate(energyPrefab).GetComponent<Energy>();
                go.transform.SetParent(energyGrid);
                energies.Add(go);
            }
        }
    }
    public int CurEnergy{
        get=>curEnergy;
        set{
            if(value>maxEnergy) return;
            curEnergy=value;
            int actualE=curEnergy>>1;
            int remainder=curEnergy%2;
            for(int i=energies.Count-1;i>=0;--i){
                energies[i].TurnOn(i<actualE);
            }
            if(remainder==1)
                energies[actualE].HalfOn();
        }
    }
    void Awake(){
        inst=this;
    }
    void Start(){
        lives=new List<Life>();
        energies=new List<Energy>();
        MaxHealth=maxHealth;
        MaxEnergy=maxEnergy<<1;
        SetCurHealth(maxHealth, null);
        CurEnergy=0;
    }
    public void SetCurHealth(int value, Collider2D from){
        curHealth=value;
        for(int i=lives.Count-1;i>=0;--i){
            lives[i].TurnOn(i<curHealth);
        }
        //if touches spike, temporarily die
        if(!(from==null || !from.CompareTag("spike")) && curHealth>0){
            SavePoint.RevivePlayer();
        }
        //die
        if(curHealth<=0){
            SetCurHealth(maxHealth, null);
            RevivePoint.RevivePlayer();
        }
    }
    public bool CanConsume(){
        return curEnergy>1;
    }
    public void Consume(){
        CurEnergy-=2;
    }
}

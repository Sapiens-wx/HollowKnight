using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip bgm,fsm;
    public static SFX inst;
    void Awake(){
        inst=this;
    }
    public static void Play(AudioClip clip){
        inst.audioSource.clip=clip;
        inst.audioSource.Play();
    }
}

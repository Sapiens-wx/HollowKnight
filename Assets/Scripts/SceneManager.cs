using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager_m : MonoBehaviour
{
    public string menuScene, gameScene;
    public static SceneManager_m inst;
    void Awake(){
        if(inst!=null)
            Destroy(gameObject);
        inst=this;
    }
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    void Update(){
        if(Input.GetKeyDown(KeyCode.R)){
            LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
    public static void LoadScene(int builtIdx){
        Fade.BlackInOut(()=>{
            AsyncOperation op = SceneManager.LoadSceneAsync(builtIdx);
            op.allowSceneActivation=true;
            }, null);
    }
    public static void LoadScene(string sceneName){
        Fade.BlackInOut(()=>{
            AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
            op.allowSceneActivation=true;
            }, null);
    }
}

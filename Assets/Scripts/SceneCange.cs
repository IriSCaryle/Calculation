using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneCange : MonoBehaviour
{
    private void Awake()
    {
        Application.targetFrameRate = 60;
    }
    public void SceneCange_main()//シーン遷移
    {
        if(Application.platform == RuntimePlatform.Android)//Android版へ遷移
        {
            SceneManager.LoadScene("GameScene_forAndroid");
        }else if(Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)//PC版へ遷移
        {
            SceneManager.LoadScene("GameScene_forPC");
        }
       
    }
}

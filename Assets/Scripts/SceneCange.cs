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
    public void SceneCange_main()
    {
        if(Application.platform == RuntimePlatform.Android)
        {
            SceneManager.LoadScene("GameScene_forAndroid");
        }else if(Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
        {
            SceneManager.LoadScene("GameScene_forPC");
        }
       
    }
}

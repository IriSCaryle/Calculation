using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{

 
    [SerializeField] GameObject TutorialObject;
    
    // Start is called before the first frame update
    void Start()
    {
        TutorialObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnclickCloseButton()
    {
        TutorialObject.SetActive(false);
    }

    public void OnclickOpenButton()
    {
        TutorialObject.SetActive(true);
    }
}

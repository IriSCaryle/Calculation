using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ReadyText : MonoBehaviour
{
    [SerializeField] Text Readytext;
    [SerializeField] GameObject StartOverray;
    [SerializeField] NumberToSprite time;
    // Start is called before the first frame update
    private void Awake()
    {
        time.countPermit = false;
        Readytext.text = "Ready?";
    }
    public void ChangeText()
    {
        Readytext.text = "Go";
    }
    public void ReadyTextOff()
    {
        Readytext.gameObject.SetActive(false);
        StartOverray.SetActive(false);
        time.countPermit = true;
    }
}

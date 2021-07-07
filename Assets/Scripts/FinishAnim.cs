using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FinishAnim : MonoBehaviour
{
    [SerializeField] Text Judgetext;
    [SerializeField] Text p1score;
    [SerializeField] Text p2score;
    [SerializeField] Main main;
    // Start is called before the first frame update
    public void SetText()
    {
        p1score.text = main.Player1Score.ToString("N0");
        p2score.text = main.Player2Score.ToString("N0");
        if(main.Player1Score > main.Player2Score)
        {
            Judgetext.text = "Player1の勝ち!";
        }else if (main.Player1Score < main.Player2Score)
        {
            Judgetext.text = "Player2の勝ち!";
        }
        else
        {
            Judgetext.text = "引き分け!";
        }
    }
}

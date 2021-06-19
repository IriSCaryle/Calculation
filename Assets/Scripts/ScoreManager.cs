using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] CanvasRenderer[] Num100000000 = new CanvasRenderer[10];
    [SerializeField] CanvasRenderer[] Num10000000 = new CanvasRenderer[10];
    [SerializeField] CanvasRenderer[] Num1000000 = new CanvasRenderer[10];
    [SerializeField] CanvasRenderer[] Num100000 = new CanvasRenderer[10];
    [SerializeField] CanvasRenderer[] Num10000 = new CanvasRenderer[10];
    [SerializeField] CanvasRenderer[] Num1000 = new CanvasRenderer[10];
    [SerializeField] CanvasRenderer[] Num100 = new CanvasRenderer[10];
    [SerializeField] CanvasRenderer[] Num10 = new CanvasRenderer[10];
    [SerializeField] CanvasRenderer[] Num1 = new CanvasRenderer[10];

    [SerializeField] CanvasRenderer Minus;


    [SerializeField] Main main;
    void Awake()
    {
        ResetNum();
    }


    void ResetNum()
    {
        for(int i = 1; i < Num1.Length; i++)
        {
            Num1[i].SetAlpha(0);
            Num10[i].SetAlpha(0);
            Num100[i].SetAlpha(0);
            Num1000[i].SetAlpha(0);
            Num10000[i].SetAlpha(0);
            Num100000[i].SetAlpha(0);
            Num1000000[i].SetAlpha(0);
            Num10000000[i].SetAlpha(0);
            Num100000000[i].SetAlpha(0);
        }
        Minus.SetAlpha(0);
    }

    public void AddScore(int score)
    {
        int[] num = new int[] { 0,0,0,0,0,0,0,0,0};
        int sum = 0;
        int length = 0;
        bool isminus=false;

      

        if(score < 0)
        {
            Debug.Log(score);
            score = score * (-1);
          
            isminus = true;
          
        }


        AllImageClear();

        while (0 < score)
        {
            Debug.Log("Length:" + length);

            sum = (score % 10);
            Debug.Log("num:" + sum);
            num[length] = sum;
            score = (score / 10);
            length++;
        }

        for(int tmp = 0; tmp < num.Length; tmp++)
        {
            Debug.Log(num[tmp]);
        }
        
        Num1[num[0]].SetAlpha(1);
        Num10[num[1]].SetAlpha(1);
        Num100[num[2]].SetAlpha(1);
        Num1000[num[3]].SetAlpha(1);
        Num10000[num[4]].SetAlpha(1);
        Num100000[num[5]].SetAlpha(1);
        Num1000000[num[6]].SetAlpha(1);
        Num10000000[num[7]].SetAlpha(1);
        Num100000000[num[8]].SetAlpha(1);
        if (isminus)
        {
            Minus.SetAlpha(1);
        }
    }

    void AllImageClear()
    {
        for (int i = 0; i < Num1.Length; i++)
        {
            Num1[i].SetAlpha(0);
            Num10[i].SetAlpha(0);
            Num100[i].SetAlpha(0);
            Num1000[i].SetAlpha(0);
            Num10000[i].SetAlpha(0);
            Num100000[i].SetAlpha(0);
            Num1000000[i].SetAlpha(0);
            Num10000000[i].SetAlpha(0);
            Num100000000[i].SetAlpha(0);
        }

        Minus.SetAlpha(0);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberToSprite : MonoBehaviour
{
    float timecount;

    [SerializeField] int startitme;
    int nowtime;
    [SerializeField] GameObject[] object_time = new GameObject[10];//画像の数字xはUnity上のElement xの数字に合わせてね

    GameObject[] time_image = new GameObject [10];//桁数に合わせて画像を保存するとこ

    public int Digit(int num)
    {
        return (num == 0) ? 1 : ((int)Mathf.Log10(num) + 1);//nowtimeの桁数を取得
    }

    void Start()
    {
        nowtime = startitme;
        InstSplite();
    }

    void Update()
    {
        timecount += Time.deltaTime;//1秒ごとに判定
        if (timecount >= 1) CountCange();
    }

    void CountCange()
    {
        nowtime--;
        InstSplite();
        timecount = 0;
    }

    void InstSplite()
    {
        if (nowtime < 0) return;

        for (int i = 0; i < Digit(nowtime); i++)
        {
            Destroy(time_image[i]);//指定桁を破壊

            int num = (int)(nowtime / Mathf.Pow(10, i)) % 10;//iの位の数値を取得

            time_image[i] = Instantiate(
                object_time[num],
                new Vector3 (i* -1,0,0),//位置は適当なので調整お願い
                Quaternion.identity
                );
        }
    }
}

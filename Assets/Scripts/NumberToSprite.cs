using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberToSprite : MonoBehaviour
{
    float timecount;

    [SerializeField] int startitme;
    int nowtime;
    int beforetime;
    [SerializeField] GameObject time_box;//これの中のSpriteRendererに画像をつける
    [SerializeField] Sprite[] sprite_time = new Sprite[10];//画像の数字xはUnity上のElement xの数字に合わせてね

    GameObject[] time_image = new GameObject [10];//桁数に合わせて画像を保存するとこ
    SpriteRenderer[] time_spriteR = new SpriteRenderer[10];

    public int Digit(int num)
    {
        return (num == 0) ? 1 : ((int)Mathf.Log10(num) + 1);//nowtimeの桁数を取得
    }

    public int BTDight(int num)
    {
        return (num == 0) ? 1 : ((int)Mathf.Log10(num) + 1);//beforetimeの桁数を取得
    }

    void Start()
    {
        nowtime = startitme;

        for(int i =0;i<Digit (nowtime); i++)
        {
            time_image[i] = Instantiate(
                time_box,
                new Vector3(i * -1, 0, 0),//位置は適当なので調整お願い
                Quaternion.identity
                );

            time_spriteR[i] = time_image[i].GetComponent<SpriteRenderer>();
        }
        InstSplite();
    }

    void Update()
    {
        timecount += Time.deltaTime;//1秒ごとに判定
        if (timecount >= 1) CountCange();
    }

    void CountCange()
    {
        beforetime = nowtime;
        nowtime--;
        if (BTDight(beforetime) != Digit(nowtime)) Destroy(time_image[BTDight(beforetime) - 1]);//使い終えた桁を削除

        InstSplite();
    }

    void InstSplite()
    {
        if (nowtime < 0) return;

        for (int i = 0; i < Digit(nowtime); i++)
        {
            int num = (int)(nowtime / Mathf.Pow(10, i)) % 10;//iの位の数値を取得

            time_spriteR[i].sprite = sprite_time[num];
        }
        timecount = 0;
    }
}

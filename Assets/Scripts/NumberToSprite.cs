using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NumberToSprite : MonoBehaviour
{
    float timecount;

    [SerializeField] GameObject time_object;
    [SerializeField] int startitme;
    int nowtime;
    // int beforetime;
    int nowsec;
    int nowmin;
    int beforesec;
    int beforemin;
    [SerializeField] GameObject time_box;//これの中のSpriteRendererに画像をつける
    [SerializeField] Sprite[] sprite_time = new Sprite[10];//画像の数字xはUnity上のElement xの数字に合わせてね

    GameObject[] time_image = new GameObject [10];//桁数に合わせて画像を保存するとこ
    Image[] time_spriteR = new Image[10];

    public bool countPermit = true;

    [SerializeField] Main main;

    [SerializeField] GameObject FinishOverray;

    [SerializeField] Animator FinishAnim;
    [SerializeField] GameObject Num001Pos;
    [SerializeField] GameObject Num010Pos;
    [SerializeField] GameObject Num100Pos;
    [SerializeField] GameObject Num1000Pos;

    public int Digit(int num)
    {
        return (num == 0) ? 1 : ((int)Mathf.Log10(num) + 1);//nowtimeの桁数を取得
    }

    public int BTDight(int num)
    {
        return (num == 0) ? 1 : ((int)Mathf.Log10(num) + 1);//beforetimeの桁数を取得
    }

    public void Stop()
    {
        countPermit = false;
    }

    public void Restart()
    {
        countPermit = true;
    }

    private void Awake()
    {
        FinishOverray.SetActive(false);
      
    }
    void Start()
    {
        nowtime = startitme;
        nowsec = nowtime % 60;
        nowmin = nowtime / 60;

        for (int i = 0; i < Digit(nowsec) + Digit (nowmin); i++)
        {
            switch (i)
            {
                case 0:
                    time_image[i] = Instantiate(
                    time_box,
                    new Vector3(Num001Pos.transform.position.x, time_object.transform.position.y, 0),//位置は適当なので調整お願い
                    Quaternion.identity,
                    time_object.transform
                    );
                    time_spriteR[i] = time_image[i].GetComponent<Image>();
                    break;
                case 1:
                    time_image[i] = Instantiate(
                    time_box,
                    new Vector3(Num010Pos.transform.position.x, time_object.transform.position.y, 0),//位置は適当なので調整お願い
                    Quaternion.identity,
                    time_object.transform
                    );
                    time_spriteR[i] = time_image[i].GetComponent<Image>();
                    break;
                case 2:
                    time_image[i] = Instantiate(
                    time_box,
                    new Vector3(Num100Pos.transform.position.x, time_object.transform.position.y, 0),//位置は適当なので調整お願い
                    Quaternion.identity,
                    time_object.transform
                    );
                    time_spriteR[i] = time_image[i].GetComponent<Image>();
                    break;
                case 3:
                    time_image[i] = Instantiate(
                    time_box,
                    new Vector3(Num1000Pos.transform.position.x, time_object.transform.position.y, 0),//位置は適当なので調整お願い
                    Quaternion.identity,
                    time_object.transform
                    );
                    time_spriteR[i] = time_image[i].GetComponent<Image>();
                    break;
                default:
                    Debug.LogError("タイマーの桁に異常があります");
                    break;
            }
            
        }
        InstSplite();
    }

    void Update()
    {
        if (!countPermit) return;
        timecount += Time.deltaTime;//1秒ごとに判定
        if (timecount >= 1) CountCange();

      
    }

    void CountCange()
    {
        // beforetime = nowtime;
        beforemin = nowmin;
        beforesec = nowsec;
        if (nowsec <= 0)
        {
            if (nowmin <= 0) Finish();
            if (BTDight(beforemin) + BTDight(beforesec) != Digit(nowmin) + Digit(nowsec))
            {
                Destroy(time_image[BTDight(beforemin) + BTDight(beforesec) - 1]);//使い終えた桁を削除
            }
            nowmin--;
            nowsec += 59;
        }
        else nowsec--;
        //nowtime--;

        InstSplite();
    }


    void Finish()
    {
        nowsec = 0;
        countPermit = false;
        Debug.Log("終了");
        FinishOverray.SetActive(true);
        FinishAnim.SetTrigger("Finish");
     
    }

    void InstSplite()
    {
        if (nowsec+nowmin < 0) return;

        for(int j =0;j<Digit(nowmin); j++)
        {
            int nummin = (int)(nowmin / Mathf.Pow(10, j)) % 10;

            time_spriteR[j + 2].sprite = sprite_time[nummin];
        }

        for (int i = 0; i < 2; i++)
        {
            int numsec = (int)(nowsec / Mathf.Pow(10, i)) % 10;//iの位の数値を取得

            time_spriteR[i].sprite = sprite_time[numsec];
        }
        timecount = 0;
    }

    public void OnClickFinishButton()
    {
        SceneManager.LoadScene("title");
    }
}

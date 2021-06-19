using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class PlayerBlock : MonoBehaviour,IDropHandler,IDragHandler,IEndDragHandler,IPointerDownHandler,IPointerUpHandler
{
    [Header("プロックの種類")]
    public PlayerBlocks playerblocks;
    [Header("ブロックの数字")]
    public int Number;//このブロックの数値
    [Header("縦")]
    public int vertical;//縦座標
    [Header("横")]
    public int horizontal;//横座標
    [Header("各桁の数字イメージ")]
    [SerializeField] CanvasRenderer[] Num001 = new CanvasRenderer[10];
    [SerializeField] CanvasRenderer[] Num010 = new CanvasRenderer[10];
    [SerializeField] CanvasRenderer[] Num100 = new CanvasRenderer[10];
    [Header("最大値の数字イメージ")]
    [SerializeField] CanvasRenderer Max900;//赤文字9 百桁目
    [SerializeField] CanvasRenderer Max090;//赤文字9 十桁目
    [SerializeField] CanvasRenderer Max009;//赤文字9 一桁目
    [Header("ターゲットイメージ")]
    public CanvasRenderer TargetOverray;

    [Header("ハイライト用イメージ")]
    public Image Highlightimage;

    [Header("スクリプト")]
    [SerializeField] Main main;

    [Header("初期位置")]
    public Vector3 InitPos;//初期位置


    bool up = false;
    bool down = false;
    bool left = false;
    bool right = false;
    public enum PlayerBlocks//誰のブロックかの判別
    {
        Player1 =1,
        Player2 =2,
    }
    // Start is called before the first frame update
    void Start()
    {
        main = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Main>();
        TargetOverray.SetAlpha(0);
    }

    public void AssignNum()
    {
        int NumRand = Random.Range(1,51);

        Number = NumRand;

        ChangeNumImage(NumRand.ToString());
    }
    public void ChangeNumImage(string x)
    {
        AllImageClear();

        int[] num = new int[3];

        int value = 0;

        int NumLength = x.Length;

        value = int.Parse(x);

        Debug.Log("NumLength" + NumLength);


        for (int i = 0; i < NumLength; i++)
        {
            num[i] = value % 10;

            value = value / 10;


        }

        Debug.Log("0:" + num[0]);

        Debug.Log("1:" + num[1]);

        Debug.Log("2:" + num[2]);


        switch (NumLength)
        {
            case 0:

                Num001[0].SetAlpha(1);
                Num010[0].SetAlpha(1);
                Num100[0].SetAlpha(1);

                break;
            case 1:

                Num001[num[0]].SetAlpha(1);
                Num010[0].SetAlpha(1);
                Num100[0].SetAlpha(1);
                break;

            case 2:

                Num001[num[0]].SetAlpha(1);
                Num010[num[1]].SetAlpha(1);
                Num100[0].SetAlpha(1);
                break;
            case 3:

                Num001[num[0]].SetAlpha(1);

                Num010[num[1]].SetAlpha(1);

                Num100[num[2]].SetAlpha(1);

                break;
            default:


                break;

        }





    }

    void AllImageClear()
    {
        for (int i = 0; i < Num001.Length; i++)
        {
            Num001[i].SetAlpha(0);
        }
        for (int i = 0; i < Num010.Length; i++)
        {
            Num010[i].SetAlpha(0);
        }
        for (int i = 0; i < Num100.Length; i++)
        {
            Num100[i].SetAlpha(0);
        }

        Max009.SetAlpha(0);
        Max090.SetAlpha(0);

        Max900.SetAlpha(0);
    }

    public void NumMax()
    {
        AllImageClear();
        Max009.SetAlpha(1);
        Max090.SetAlpha(1);
        Max900.SetAlpha(1);

    }

    public void OnPointerDown(PointerEventData data)
    {
        up = false;
        down = false;
        left = false;
        right = false;

       

        main.allImagesUnHighlight();

        Highlightimage.color = new Color(0, 0, 0, 0);

        (up,down,left,right)= main.SerchAround_CanCalc(vertical, horizontal,(int)playerblocks);


    }

    public void OnPointerUp(PointerEventData data)
    {
        
        main.allImagesHighlight();
        Highlightimage.color = new Color(0, 0, 0, 0);
        main.ResetTarget(vertical, horizontal);
    }
    public void OnDrag(PointerEventData data)
    {
        Debug.Log("scrollData:" + data.delta);

        //右
        if (data.delta.x > 10 && right)
        {

            main.ResetTarget(vertical, horizontal);
            main.SetTarget(vertical, horizontal, "Right");

        }
        else if (data.delta.x < -10 && left)//左
        {

            main.ResetTarget(vertical, horizontal);
            main.SetTarget(vertical, horizontal, "Left");
        }
        //上
        if (data.delta.y > 10 && up)
        {
            main.ResetTarget(vertical, horizontal);
            main.SetTarget(vertical, horizontal, "Up");
        }
        else if (data.delta.y < -10 && down)//下
        {

            main.ResetTarget(vertical, horizontal);
            main.SetTarget(vertical, horizontal, "Down");
        }

    }

    public void OnDrop(PointerEventData data)
    {
       

        switch (data.pointerDrag.gameObject.tag)
        {
            case "CalcBlock":
                Debug.Log("Dropを検出");
                Debug.LogError("演算子ブロックのドロップが検出されました");

                break;

            case "PlayerBlock":
                Debug.Log("Dropを検出");
                PlayerBlock playerBlock = data.pointerDrag.gameObject.GetComponent<PlayerBlock>();

                main.OnDropReminder(playerBlock.vertical,playerBlock.horizontal, vertical, horizontal);

                break;
        }

    }
    public void OnEndDrag(PointerEventData data)
    {
        main.ResetTarget(vertical, horizontal);
    }




    // Update is called once per frame
    void Update()
    {
        
    }



}

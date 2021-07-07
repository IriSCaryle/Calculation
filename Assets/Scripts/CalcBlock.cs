using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class CalcBlock : MonoBehaviour, IDropHandler ,IDragHandler,IEndDragHandler,IPointerDownHandler,IPointerUpHandler
{
    
   
    [Header("プロックの種類")]
    public CalcBlocks calcBlocks;
    [Header("ブロックの数字")]
    public int Number;
    [Header("縦")]
    public int vertical;
    [Header("横")]
    public int horizontal;
    [Header("演算子イメージ")]
    [SerializeField] CanvasRenderer[] calcImages = new CanvasRenderer[4];
    [Header("各桁の数字イメージ")]
    [SerializeField] CanvasRenderer[] Num001 = new CanvasRenderer[10];
    [SerializeField] CanvasRenderer[] Num010 = new CanvasRenderer[10];
    [SerializeField] CanvasRenderer[] Num100 = new CanvasRenderer[10];
    [Header("減算記号イメージ")]
    [SerializeField] CanvasRenderer minusImage;
    [Header("ハイライト用イメージ")]
    public Image Highlightimage;
    [Header("ターゲットイメージ")]
    public CanvasRenderer TargetOverray;
    [Header("スクリプト")]
    [SerializeField] Main main;
    [Header("初期位置")]
    public Vector3 InitPos;//初期位置
    
    bool up;
    bool down;
    bool left;
    bool right;
    public enum CalcBlocks//演算子の種類
    {
        add =0,
        sub =1,
        mult=2,
        div =3,
    }
    // Start is called before the first frame update
    void Start()
    {
        main = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Main>();
        TargetOverray.SetAlpha(0);//カーソルの非表示
    }
    public void AssignColc()//演算子の数字を生成
    {
        int CalcRand = Random.Range(0,4);//演算子をランダムに算出
        int NumRand = Random.Range(-10,20);//数字をランダムに算出

        if (NumRand == 0)//0の場合1にする
        {
            NumRand += 1;
        }
        Number = NumRand;
        ChangeNumImage(NumRand.ToString());
        ChangeCalcImage(CalcRand);
    }
    public void ChangeNumImage(string x)//ブロックの数字のイメージを表示する
    {
        int[] num = new int[3];
        int value = 0;
        bool isminus = false;
        
        AllNumImageClear();
        if (x.Contains("-"))
        {
            x = x.Replace("-", "");
            isminus = true;
        }
        if (isminus)
        {
            minusImage.SetAlpha(1);
        }
        int NumLength = x.Length;
        value = int.Parse(x);
        Debug.Log("NumLength" + NumLength);
        for (int i = 0; i < NumLength; i++)//桁を取得
        {
            num[i] = value % 10;
            value = value / 10;
            
        }
        Debug.Log("0:"+ num[0]);
        Debug.Log("1:"+ num[1]);
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
    void AllNumImageClear()//全ての数字イメージを非表示
    {
        for(int i = 0; i < Num001.Length; i++)
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
        minusImage.SetAlpha(0);
    }
    public void ChangeCalcImage(int x)//指定した演算子イメージを表示
    {
        AllCalcImageClear();
        calcImages[x].SetAlpha(1);
        switch (x)
        {
            case 0:
                calcBlocks = CalcBlocks.add;
                break;
            case 1:
                calcBlocks = CalcBlocks.sub;
              
                break;
            case 2:
                calcBlocks = CalcBlocks.mult;
                break;
            case 3:
                calcBlocks = CalcBlocks.div;
                break;
        }
        
    }
    void AllCalcImageClear()//全ての演算子イメージを非表示
    {
        for (int i = 0; i < calcImages.Length; i++)
        {
            calcImages[i].SetAlpha(0);
        }
            
    }
    public void OnPointerDown(PointerEventData data)//クリックしたとき
    {
        up = false;
        down = false;
        left = false;
        right = false;
        main.allImagesUnHighlight();
        Highlightimage.color = new Color(0, 0, 0, 0);
        (up, down, left, right) = main.SerchAround_CanCalc(vertical, horizontal,3);
    }
    public void OnPointerUp(PointerEventData data)//クリックを離したとき
    {
       
        main.allImagesHighlight();
        Highlightimage.color = new Color(0, 0, 0, 0);
        main.ResetTarget(vertical, horizontal);
    }
    public void OnDrag(PointerEventData data)//ドラッグしたとき（毎フレーム）
    {
        //ドラッグした方向に対してカーソルを表示する
        //右
        if (data.delta.x >5 &&right)
        {
            main.ResetTarget(vertical, horizontal);
            main.SetTarget(vertical, horizontal, "Right");
        }else if(data.delta.x <-5 && left)//左
        {
            main.ResetTarget(vertical, horizontal);
            main.SetTarget(vertical, horizontal, "Left");
        }
        //上
        if (data.delta.y >5 && up)
        {
            main.ResetTarget(vertical, horizontal);
            main.SetTarget(vertical, horizontal, "Up");
        }
        else if (data.delta.y < -5 && down)//下
        {
            main.ResetTarget(vertical, horizontal);
            main.SetTarget(vertical, horizontal, "Down");
        }
       
    }
    public void OnDrop(PointerEventData data)//別のオブジェクトにドロップしたとき
    {
       
        Debug.Log("Drop検出:" + data.pointerDrag.name);
        switch (data.pointerDrag.gameObject.tag)
        {
            case "CalcBlock":
                Debug.Log("Dropを検出");
                CalcBlock calcBlock = data.pointerDrag.GetComponent<CalcBlock>();
                main.OnDropReminder(calcBlock.vertical,calcBlock.horizontal,vertical, horizontal);
                break;
            case "PlayerBlock":
                Debug.Log("Dropを検出");
                PlayerBlock playerBlock = data.pointerDrag.GetComponent<PlayerBlock>();
                main.OnDropReminder(playerBlock.vertical,playerBlock.horizontal, vertical, horizontal);
                break;
        }
    }
    public void OnEndDrag(PointerEventData data)//別のオブジェクトにドロップせずその場で離したとき
    {
        main.ResetTarget(vertical, horizontal);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

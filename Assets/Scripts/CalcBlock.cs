using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class CalcBlock : MonoBehaviour, IDropHandler
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

    [Header("スクリプト")]
    [SerializeField] Main main;

    float minusDistance  =22;

    bool up;
    bool down;
    bool left;
    bool right;


    public enum CalcBlocks
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
    }


    public void AssignColc()
    {
        int CalcRand = Random.Range(0,4);
        int NumRand = Random.Range(-5,6);

        
        if(NumRand == 0)
        {

            NumRand += 1;
        }

        Number = NumRand;


        ChangeNumImage(NumRand.ToString());

        ChangeCalcImage(CalcRand);
    }


    public void ChangeNumImage(string x)
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


        for (int i = 0; i < NumLength; i++)
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


    void AllNumImageClear()
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


    public void ChangeCalcImage(int x)
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

    void AllCalcImageClear()
    {
        for (int i = 0; i < calcImages.Length; i++)
        {
            calcImages[i].SetAlpha(0);
        }
            
    }

    public void OnDropCalcBlock(int point,CalcBlocks calcType)
    {
        switch (calcType)
        {
            case CalcBlocks.add:



                break;

            case CalcBlocks.sub:


                break;

            case CalcBlocks.mult:


                break;

            case CalcBlocks.div:



                break;



        }



    }

    public void OnClick()
    {
        up = false;
        down = false;
        left = false;
        right = false;

        main.allImagesUnHighlight();

        Highlightimage.color = new Color(0, 0, 0, 0);

        (up, down, left, right) = main.SerchAround_CanCalc(vertical, horizontal,3);

    }

    public void OnClickCancel()
    {

        main.allImagesHighlight();
        Highlightimage.color = new Color(0, 0, 0, 0);
    }
    public void OnDrag(BaseEventData eventData)
    {


    }

    public void OnDrop(PointerEventData data)//BaseEventData eventData)
    {
        /* var data = eventData as PointerEventData;

         if (data != null)
         {
             if (data.clickCount > 1)
             {
                 Debug.Log(data.clickCount);
             }
         }
        */
        Debug.Log("Drop検出:" + data.lastPress.name);

        switch (data.lastPress.gameObject.tag)
        {
            case "CalcBlock":

                Debug.Log("Dropを検出");

                CalcBlock calcBlock = data.lastPress.gameObject.GetComponent<CalcBlock>();

                main.OnDropReminder(calcBlock.vertical.ToString() + "," + calcBlock.horizontal.ToString(), vertical.ToString() + "," + horizontal.ToString());
                break;

            case "PlayerBlock":

                Debug.Log("Dropを検出");
                PlayerBlock playerBlock = data.lastPress.gameObject.GetComponent<PlayerBlock>();

                main.OnDropReminder(playerBlock.vertical.ToString() + "," + playerBlock.horizontal.ToString(), vertical.ToString() + "," + horizontal.ToString());

                break;
        }

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

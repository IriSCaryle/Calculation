using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Main : MonoBehaviour
{
    [SerializeField] float Block_distance;

    [SerializeField] GameObject GenerateInitPos;

    [SerializeField] GameObject[] Blocks = new GameObject[4];


    [SerializeField] GameObject BlocksParent;

    [SerializeField] Text DragDroptext;

    int[,] CalculationBoard = new int[6, 6]
    {
        { 0,0,0,0,0,0 },
        { 0,0,0,0,0,0 },
        { 0,0,0,0,0,0 },
        { 0,0,0,0,0,0 },
        { 0,0,0,0,0,0 },
        { 0,0,0,0,0,0 },
    };


    GameObject[,] BlocksObjectBoard = new GameObject[6, 6];

    Image[,] BlocksImages = new Image[6, 6];

    PlayerBlock[,] playerBlocksBoard = new PlayerBlock[6, 6];

    CalcBlock[,] calcBlocksBoard = new CalcBlock[6, 6];

    public enum Turn
    {
        Player1 = 1,
        Player2 = 2,


    }


    public enum CalculationBlocks
    {
        None = 0,
        Player1Blocks = 1,
        Player2Blocks = 2,
        CalcBlocks = 3,
    }
    // Start is called before the first frame update
    void Start()
    {
        GenerateBlocks();
    }


    //ブロックの生成
    void GenerateBlocks()
    {
        int player1block = 0;

        int player2block = 0;

        int calcblock = 0;

        List<int> randnum = new List<int>();

        randnum.Add(1);
        randnum.Add(2);
        randnum.Add(3);


        for (int v = 0; v < CalculationBoard.GetLength(0); v++)
        {
            for (int h = 0; h < CalculationBoard.GetLength(1); h++)
            {
                int rand = Random.Range(0, randnum.Count);



                switch (randnum[rand])
                {
                    case 1:


                        CalculationBoard[v, h] = 1;
                        BlocksObjectBoard[v, h] = Instantiate(Blocks[1], new Vector3(GenerateInitPos.transform.position.x + Block_distance * h, GenerateInitPos.transform.position.y - Block_distance * v,0)
                        , Quaternion.identity, BlocksParent.transform);

                        playerBlocksBoard[v, h] = BlocksObjectBoard[v, h].GetComponent<PlayerBlock>();
                        BlocksImages[v, h] = playerBlocksBoard[v, h].Highlightimage;
                        playerBlocksBoard[v, h].vertical = v;
                        playerBlocksBoard[v, h].horizontal = h;
                        playerBlocksBoard[v, h].AssignNum();
                        
                        player1block++;

                        if (player1block >= 12)
                        {
                            randnum.Remove(1);
                        }

                        break;
                    case 2:
                        CalculationBoard[v, h] = 2;
                        BlocksObjectBoard[v, h] = Instantiate(Blocks[2], new Vector3(GenerateInitPos.transform.position.x + Block_distance * h, GenerateInitPos.transform.position.y - Block_distance * v,0)
                        , Quaternion.identity, BlocksParent.transform);

                        playerBlocksBoard[v, h] = BlocksObjectBoard[v, h].GetComponent<PlayerBlock>();
                        BlocksImages[v, h] = playerBlocksBoard[v, h].Highlightimage;
                        playerBlocksBoard[v, h].vertical = v;
                        playerBlocksBoard[v, h].horizontal = h;
                        playerBlocksBoard[v, h].AssignNum();

                        player2block++;

                        if (player2block >= 12)
                        {
                            randnum.Remove(2);
                        }


                        break;
                    case 3:
                        CalculationBoard[v, h] = 3;
                        BlocksObjectBoard[v, h] = Instantiate(Blocks[3], new Vector3(GenerateInitPos.transform.position.x + Block_distance * h, GenerateInitPos.transform.position.y - Block_distance * v,0)
                        , Quaternion.identity, BlocksParent.transform);

                        calcBlocksBoard[v, h] = BlocksObjectBoard[v, h].GetComponent<CalcBlock>();
                        BlocksImages[v, h] = calcBlocksBoard[v, h].Highlightimage;
                        calcBlocksBoard[v, h].vertical = v;
                        calcBlocksBoard[v, h].horizontal = h;
                        calcBlocksBoard[v, h].AssignColc();

                        calcblock++;

                        if (calcblock >= 12)
                        {
                            randnum.Remove(3);
                        }

                        break;
                }
            }
        }

        NowBoard();


    }
    //現在のボードの状況をコンソールに表示する
    public void NowBoard()
    {

        string board = "";
        for (int v = 0; v < CalculationBoard.GetLength(0); v++)
        {
            for (int h = 0; h < CalculationBoard.GetLength(1); h++)
            {

                board = board + CalculationBoard[v, h];


            }

            board = board + "\n";

        }
        Debug.Log(board);
    }

    //計算可能なブロックを検索する
    public (bool Up, bool Down, bool Left, bool Right) SerchAround_CanCalc(int vertical, int horizontal, int type)
    {
        bool up = false;
        bool down = false;
        bool left = false;
        bool right = false;

        if (type == 1 || type == 2)//プレイヤーブロックの場合
        {
            //上
            if (vertical > 0)
            {
                switch (CalculationBoard[vertical - 1, horizontal])
                {
                    case 0:

                        Debug.LogError("盤面上に空の座標があります");

                        break;
                    case 1:

                        Debug.Log("演算子ブロックではありません");

                        break;
                    case 2:

                        Debug.Log("演算子ブロックではありません");

                        break;
                    case 3:

                        up = true;
                        BlocksImages[vertical - 1, horizontal].color = new Color(0, 0, 0, 0);

                        break;
                    default:

                        Debug.LogError("例外が発生しました確認してください:" + vertical + "," + horizontal);

                        break;

                }
            }
            //下
            if (vertical < 5)
            {
                switch (CalculationBoard[vertical + 1, horizontal])
                {
                    case 0:

                        Debug.LogError("盤面上に空の座標があります");

                        break;
                    case 1:

                        Debug.Log("演算子ブロックではありません");

                        break;
                    case 2:

                        Debug.Log("演算子ブロックではありません");

                        break;
                    case 3:

                        down = true;
                        BlocksImages[vertical + 1, horizontal].color = new Color(0, 0, 0, 0);
                        break;
                    default:

                        Debug.LogError("例外が発生しました確認してください:" + vertical + "," + horizontal);

                        break;

                }
            }
            //左
            if (horizontal > 0)
            {
                switch (CalculationBoard[vertical, horizontal - 1])
                {
                    case 0:

                        Debug.LogError("盤面上に空の座標があります");

                        break;
                    case 1:

                        Debug.Log("演算子ブロックではありません");

                        break;
                    case 2:

                        Debug.Log("演算子ブロックではありません");

                        break;
                    case 3:

                        left = true;
                        BlocksImages[vertical, horizontal - 1].color = new Color(0, 0, 0, 0);
                        break;
                    default:

                        Debug.LogError("例外が発生しました確認してください:" + vertical + "," + horizontal);

                        break;

                }
            }
            //右
            if (horizontal < 5)
            {
                switch (CalculationBoard[vertical, horizontal + 1])
                {
                    case 0:

                        Debug.LogError("盤面上に空の座標があります");

                        break;
                    case 1:

                        Debug.Log("演算子ブロックではありません");

                        break;
                    case 2:

                        Debug.Log("演算子ブロックではありません");

                        break;
                    case 3:

                        right = true;
                        BlocksImages[vertical, horizontal + 1].color = new Color(0, 0, 0, 0);
                        break;
                    default:

                        Debug.LogError("例外が発生しました確認してください:" + vertical + "," + horizontal);

                        break;

                }
            }
        }
        else//演算子ブロックの場合
        {
            //上
            if (vertical > 0)
            {
                switch (CalculationBoard[vertical - 1, horizontal])
                {
                    case 0:

                        Debug.LogError("盤面上に空の座標があります");

                        break;
                    case 1:

                        Debug.Log("演算子ブロックではありません");

                        break;
                    case 2:

                        Debug.Log("演算子ブロックではありません");

                        break;
                    case 3:

                        up = true;
                        BlocksImages[vertical - 1, horizontal].color = new Color(0, 0, 0, 0);

                        break;
                    default:

                        Debug.LogError("例外が発生しました確認してください:" + vertical + "," + horizontal);

                        break;

                }
            }
            //下
            if (vertical < 5)
            {
                switch (CalculationBoard[vertical + 1, horizontal])
                {
                    case 0:

                        Debug.LogError("盤面上に空の座標があります");

                        break;
                    case 1:

                        Debug.Log("演算子ブロックではありません");

                        break;
                    case 2:

                        Debug.Log("演算子ブロックではありません");

                        break;
                    case 3:

                        down = true;
                        BlocksImages[vertical + 1, horizontal].color = new Color(0, 0, 0, 0);
                        break;
                    default:

                        Debug.LogError("例外が発生しました確認してください:" + vertical + "," + horizontal);

                        break;

                }
            }
            //左
            if (horizontal > 0)
            {
                switch (CalculationBoard[vertical, horizontal - 1])
                {
                    case 0:

                        Debug.LogError("盤面上に空の座標があります");

                        break;
                    case 1:

                        Debug.Log("演算子ブロックではありません");

                        break;
                    case 2:

                        Debug.Log("演算子ブロックではありません");

                        break;
                    case 3:

                        left = true;
                        BlocksImages[vertical, horizontal - 1].color = new Color(0, 0, 0, 0);
                        break;
                    default:

                        Debug.LogError("例外が発生しました確認してください:" + vertical + "," + horizontal);

                        break;

                }
            }
            //右
            if (horizontal < 5)
            {
                switch (CalculationBoard[vertical, horizontal + 1])
                {
                    case 0:

                        Debug.LogError("盤面上に空の座標があります");

                        break;
                    case 1:

                        Debug.Log("演算子ブロックではありません");

                        break;
                    case 2:

                        Debug.Log("演算子ブロックではありません");

                        break;
                    case 3:

                        right = true;
                        BlocksImages[vertical, horizontal + 1].color = new Color(0, 0, 0, 0);
                        break;
                    default:

                        Debug.LogError("例外が発生しました確認してください:" + vertical + "," + horizontal);

                        break;

                }
            }
        }
    
        return (up, down, left, right);

    }
    //ドラッグ＆ドロップ関連
    public void allImagesUnHighlight()
    {

        for(int v = 0; v < BlocksImages.GetLength(0); v++)
        {
            for(int h = 0; h < BlocksImages.GetLength(1); h++)
            {
                BlocksImages[v, h].color = new Color(0, 0, 0, 0.4f);
            }
        }
    }
    public void allImagesHighlight()
    {

        for (int v = 0; v < BlocksImages.GetLength(0); v++)
        {
            for (int h = 0; h < BlocksImages.GetLength(1); h++)
            {
                BlocksImages[v, h].color = new Color(0, 0, 0, 0);
            }
        }
    }
    public void OnDropReminder(int drag_v,int drag_h,int drop_v,int drop_h)
    {
        if(drag_v> drop_v)//上
        {
            DragDroptext.text = "Drag&Dropが検出 上方向:" + drag_v + "," + drag_h + "to" + drop_v + "," + drop_h;
            CalculatingBlocks(drag_v, drag_h, drop_v, drop_h);
        }
        else if(drag_v<drop_v)//下
        {
            DragDroptext.text = "Drag&Dropが検出 下方向:" + drag_v + "," + drag_h + "to" + drop_v + "," + drop_h;
            CalculatingBlocks(drag_v, drag_h, drop_v, drop_h);
        }
        else if (drag_h > drop_h)//左
        {
            DragDroptext.text = "Drag&Dropが検出 左方向:" + drag_v + "," + drag_h + "to" + drop_v + "," + drop_h;
            CalculatingBlocks(drag_v, drag_h, drop_v, drop_h);
        }
        else if(drag_h<drop_h)//右
        {
            DragDroptext.text = "Drag&Dropが検出 右方向:" + drag_v + "," + drag_h + "to" + drop_v + "," + drop_h;
            CalculatingBlocks(drag_v, drag_h, drop_v, drop_h);
        }

    }
    
    //計算関連
    public void CalculatingBlocks(int drag_v,int drag_h,int drop_v,int drop_h)
    {
        switch (BlocksObjectBoard[drag_v, drag_h].tag)
        {
            case "CalcBlock":
                if(BlocksObjectBoard[drop_v,drop_h].tag == "CalcBlock")
                {
                    CalcBlocksCalculating(drag_v,drag_h,drop_v,drop_h);
                }
                else
                {
                    Debug.Log("ドラッグ＆ドロップの順番が逆です、必ずプレイヤーブロックをドラッグしてください");
                }
                break;
            case "PlayerBlock":
                if(BlocksObjectBoard[drop_v,drop_h].tag == "CalcBlock")
                {
                    PlayerAndCalcBlocksCalculating(drag_v,drag_h,drop_v,drop_h);
                }
                else
                {
                    Debug.Log("プレイヤーブロック同士では計算できません");
                }
                break;


        }
    }
    void CalcBlocksCalculating(int drag_v,int drag_h,int drop_v,int drop_h)
    {
        int calcBlock1Number = 0;
        int calcBlock2Number = 0;
        int Operator1Number = 0;
        int Operator2Number = 0;

        int resultNumber = 0;

        calcBlock1Number = calcBlocksBoard[drag_v, drag_h].Number;

        calcBlock2Number = calcBlocksBoard[drop_v, drop_h].Number;

        Operator1Number = (int)calcBlocksBoard[drag_v, drag_h].calcBlocks;

        Operator2Number = (int)calcBlocksBoard[drop_v, drop_h].calcBlocks;

        if(Operator1Number > Operator2Number)
        {

        }else if(Operator2Number > Operator1Number)
        {

        }
        else
        {

        }
    }

    void PlayerAndCalcBlocksCalculating(int drag_v, int drag_h, int drop_v, int drop_h)
    {
        int playerBlockNumber = 0;
        int calcBlockNumber = 0;
        int OperatorNumber = 0;
        int playerNumber = 0;

        int result=0;


        playerBlockNumber = playerBlocksBoard[drag_v, drag_h].Number;

        playerNumber = (int)playerBlocksBoard[drag_v, drag_h].playerblocks;

        calcBlockNumber = calcBlocksBoard[drop_v, drop_h].Number;

        OperatorNumber = (int)calcBlocksBoard[drop_v, drop_h].calcBlocks;

        switch (OperatorNumber)//演算子
        {
            case 0://add

                result = playerBlockNumber + calcBlockNumber;
                Debug.Log("足し算:" + result);
                break;
            case 1://sub

                result = playerBlockNumber - calcBlockNumber;
                Debug.Log("引き算:" + result);
                break;
            case 2://mult

                result = playerBlockNumber * calcBlockNumber;
                Debug.Log("掛け算:" + result);
                break;
            case 3://div

                result = playerBlockNumber / calcBlockNumber;
                Debug.Log("割り算:" + result);
                break;
        }
    }
    
    //ターゲットUI関連
    public void ResetTarget(int v,int h)
    {
        
        //上
        if (v > 0)
        {
            switch (CalculationBoard[v - 1, h])
            {
                case 0:

                    Debug.LogError("何も入っていない座標があります");

                    break;
                case 1:

                    playerBlocksBoard[v - 1, h].TargetOverray.SetAlpha(0);

                    break;
                case 2:

                    playerBlocksBoard[v - 1, h].TargetOverray.SetAlpha(0);
                    break;
                case 3:
                    calcBlocksBoard[v - 1, h].TargetOverray.SetAlpha(0);

                    break;
                default:

                    Debug.LogError("例外が発生しました確認してください:" + v + "," + h);

                    break;

            }
        }
        //下
        if (v < 5)
        {
            switch (CalculationBoard[v + 1, h])
            {
                case 0:

                    Debug.LogError("何も入っていない座標があります");

                    break;
                case 1:

                    playerBlocksBoard[v + 1, h].TargetOverray.SetAlpha(0);

                    break;
                case 2:

                    playerBlocksBoard[v + 1, h].TargetOverray.SetAlpha(0);

                    break;
                case 3:

                    calcBlocksBoard[v + 1, h].TargetOverray.SetAlpha(0);
                    break;
                default:

                    Debug.LogError("例外が発生しました確認してください:" + v + "," +h);

                    break;

            }
        }
        //左
        if (h > 0)
        {
            switch (CalculationBoard[v, h - 1])
            {
                case 0:

                    Debug.LogError("何も入っていない座標があります");

                    break;
                case 1:

                    playerBlocksBoard[v , h-1].TargetOverray.SetAlpha(0);

                    break;
                case 2:

                    playerBlocksBoard[v , h-1].TargetOverray.SetAlpha(0);

                    break;
                case 3:

                    calcBlocksBoard[v , h-1].TargetOverray.SetAlpha(0);
                    break;
                default:

                    Debug.LogError("例外が発生しました確認してください:" + v + "," + h);

                    break;

            }
        }
        //右
        if (h < 5)
        {
            switch (CalculationBoard[v, h + 1])
            {
                case 0:

                    Debug.LogError("何も入っていない座標があります");

                    break;
                case 1:

                    playerBlocksBoard[v, h + 1].TargetOverray.SetAlpha(0);

                    break;
                case 2:

                    playerBlocksBoard[v, h + 1].TargetOverray.SetAlpha(0);

                    break;
                case 3:

                    calcBlocksBoard[v, h + 1].TargetOverray.SetAlpha(0);
                    break;
                default:

                    Debug.LogError("例外が発生しました確認してください:" + v + "," + h);

                    break;

            }
        }



    }

    public void SetTarget(int v ,int h,string vector)
    {
        switch (vector)
        {
            case "Up":
                if (CalculationBoard[v -1, h] ==2 || CalculationBoard[v-1, h] ==1)
                {
                    playerBlocksBoard[v-1, h].TargetOverray.SetAlpha(1);

                }else
                {
                    calcBlocksBoard[v - 1, h].TargetOverray.SetAlpha(1);
                }
                break;
            case "Down":
                if (CalculationBoard[v + 1, h] == 2 || CalculationBoard[v + 1, h] == 1)
                {
                    playerBlocksBoard[v + 1, h].TargetOverray.SetAlpha(1);
                }
                else
                {
                    calcBlocksBoard[v + 1, h].TargetOverray.SetAlpha(1);
                }
                break;
            case "Left":
                if (CalculationBoard[v, h-1] == 2 || CalculationBoard[v, h-1] == 1)
                {
                    playerBlocksBoard[v, h-1].TargetOverray.SetAlpha(1);
                }
                else
                {
                    calcBlocksBoard[v, h-1].TargetOverray.SetAlpha(1);
                }
                break;
            case "Right":
                if (CalculationBoard[v, h + 1] == 2 || CalculationBoard[v, h + 1] == 1)
                {
                    playerBlocksBoard[v, h + 1].TargetOverray.SetAlpha(1);
                }
                else
                {
                    calcBlocksBoard[v, h + 1].TargetOverray.SetAlpha(1);
                }
                break;

        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}

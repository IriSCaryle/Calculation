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

    public void OnDropReminder(string drag,string drop)
    {
        DragDroptext.text = "Drag&Dropが検出:" + drag + "to" + drop;

    }


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

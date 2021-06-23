using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Main : MonoBehaviour
{  
    //ブロック関連
    [Header("ブロックの間隔")]
    [SerializeField] float Block_distance;
    [Header("0,0のブロックの位置")]
    [SerializeField] GameObject GenerateInitPos;
    [Header("ブロックの種類")]
    [SerializeField] GameObject[] Blocks = new GameObject[4];
    [Header("ブロックが生成される親")]
    [SerializeField] GameObject BlocksParent;
    [Header("デバッグ用テキスト")]
    [SerializeField] Text DragDroptext;

    //スコア関連
    [Header("プレイヤースコア(数字)")]
    [SerializeField] int Player1Score;
    [SerializeField] int Player2Score;
    [Header("プレイヤースコア(スクリプト)")]
    [SerializeField] ScoreManager player1scoreSc;
    [SerializeField] ScoreManager player2scoreSc;

    public Turn playerTurn;
    //各ボード
    int[,] CalculationBoard = new int[6, 6] //ブロックの種類のボード(int)
    {
        { 0,0,0,0,0,0 },
        { 0,0,0,0,0,0 },
        { 0,0,0,0,0,0 },
        { 0,0,0,0,0,0 },
        { 0,0,0,0,0,0 },
        { 0,0,0,0,0,0 },
    };


    GameObject[,] BlocksObjectBoard = new GameObject[6, 6];//ブロックオブジェクトのボード

    Image[,] BlocksImages = new Image[6, 6];//各オブジェクトのImageComponentのボード

    PlayerBlock[,] playerBlocksBoard = new PlayerBlock[6, 6];//プレイヤーブロックスクリプトのボード

    CalcBlock[,] calcBlocksBoard = new CalcBlock[6, 6];//演算子ブロックスクリプトのボード

    public enum Turn//ターン
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
        Application.targetFrameRate = 60;

        GenerateBlocks();
    }


   
    void GenerateBlocks() //ブロックの生成
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

        int random = 0;

        random = Random.Range(1, 3);

        switch (random)
        {
            case 1:
                playerTurn = Turn.Player1;
                break;
            case 2:
                playerTurn = Turn.Player2;
                break;
            default:
                Debug.LogError("ターンを設定できません、指定した数字に例外が発生しました");
                break;
        }

    }
   
    public void NowBoard()//現在のボードの状況をコンソールに表示する
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
    public (bool Up, bool Down, bool Left, bool Right) SerchAround_CanCalc(int vertical, int horizontal, int type)//計算可能なブロックを検索する
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
    public void allImagesUnHighlight()//全てのブロックを暗転させる 
    {

        for(int v = 0; v < BlocksImages.GetLength(0); v++)
        {
            for(int h = 0; h < BlocksImages.GetLength(1); h++)
            {
                BlocksImages[v, h].color = new Color(0, 0, 0, 0.4f);
            }
        }
    }
    public void allImagesHighlight()//全てのブロック明転させる
    {

        for (int v = 0; v < BlocksImages.GetLength(0); v++)
        {
            for (int h = 0; h < BlocksImages.GetLength(1); h++)
            {
                BlocksImages[v, h].color = new Color(0, 0, 0, 0);
            }
        }
    }
    public void OnDropReminder(int drag_v,int drag_h,int drop_v,int drop_h)//ドロップされたことをデバックテキストに通知し計算する関数に受け渡す
    {
        if(drag_v> drop_v)//上
        {
            DragDroptext.text = "Drag&Dropが検出 上方向:" + drag_v + "," + drag_h + "to" + drop_v + "," + drop_h;
            CalculatingBlocks(drag_v, drag_h, drop_v, drop_h,"up");
        }
        else if(drag_v<drop_v)//下
        {
            DragDroptext.text = "Drag&Dropが検出 下方向:" + drag_v + "," + drag_h + "to" + drop_v + "," + drop_h;
            CalculatingBlocks(drag_v, drag_h, drop_v, drop_h,"down");
        }
        else if (drag_h > drop_h)//左
        {
            DragDroptext.text = "Drag&Dropが検出 左方向:" + drag_v + "," + drag_h + "to" + drop_v + "," + drop_h;
            CalculatingBlocks(drag_v, drag_h, drop_v, drop_h,"left");
        }
        else if(drag_h<drop_h)//右
        {
            DragDroptext.text = "Drag&Dropが検出 右方向:" + drag_v + "," + drag_h + "to" + drop_v + "," + drop_h;
            CalculatingBlocks(drag_v, drag_h, drop_v, drop_h,"right");
        }

    }
    
    //計算関連
    public void CalculatingBlocks(int drag_v,int drag_h,int drop_v,int drop_h,string vector)//ドラックしたオブジェクトとドロップしたオブジェクトの組み合わせを判別し別々の関数に渡す
    {
        switch (BlocksObjectBoard[drag_v, drag_h].tag)
        {
            case "CalcBlock":
                if(BlocksObjectBoard[drop_v,drop_h].tag == "CalcBlock")
                {
                    CalcBlocksCalculating(drag_v,drag_h,drop_v,drop_h,vector);
                }
                else
                {
                    Debug.Log("ドラッグ＆ドロップの順番が逆です、必ずプレイヤーブロックをドラッグしてください");
                }
                break;
            case "PlayerBlock":
                if(BlocksObjectBoard[drop_v,drop_h].tag == "CalcBlock")
                {
                    PlayerAndCalcBlocksCalculating(drag_v,drag_h,drop_v,drop_h,vector);
                }
                else
                {
                    Debug.Log("プレイヤーブロック同士では計算できません");
                }
                break;
        }
    }
    void CalcBlocksCalculating(int drag_v,int drag_h,int drop_v,int drop_h,string vec)//演算子同士の計算
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

        CalcBlock calcBlock = calcBlocksBoard[drop_v, drop_h];
        switch (Operator2Number)
        {
            case 0:

                resultNumber = calcBlock1Number + calcBlock2Number;
                Debug.Log("足し算:" + resultNumber + "演算子" + Operator1Number);
               
                calcBlock.ChangeNumImage(resultNumber.ToString());
                calcBlock.ChangeCalcImage(Operator2Number);
                CalcBlockReInstallBoard(drag_v, drag_h, vec);
                
                break;

            case 1:

                resultNumber = calcBlock1Number - calcBlock2Number;
                Debug.Log("引き算:" + resultNumber + "演算子" + Operator1Number);
                
                calcBlock.ChangeNumImage(resultNumber.ToString());
                calcBlock.ChangeCalcImage(Operator2Number);
                CalcBlockReInstallBoard(drag_v, drag_h, vec);
                break;

            case 2:

                resultNumber = calcBlock1Number * calcBlock2Number;
                Debug.Log("掛け算:" + resultNumber + "演算子" + Operator1Number);
                
                calcBlock.ChangeNumImage(resultNumber.ToString());
                calcBlock.ChangeCalcImage(Operator2Number);
                CalcBlockReInstallBoard(drag_v, drag_h,vec);
                break;
            case 3:

                resultNumber = calcBlock1Number / calcBlock2Number;
                Debug.Log("割り算:" + resultNumber + "演算子" + Operator1Number);
                
                calcBlock.ChangeNumImage(resultNumber.ToString());
                calcBlock.ChangeCalcImage(Operator2Number);
                CalcBlockReInstallBoard(drag_v, drag_h, vec);
                break;
            default:

                Debug.LogError("規定外の数字が入力されています 演算子ブロックを確認してください");

                break;
        }



    }

    void PlayerAndCalcBlocksCalculating(int drag_v, int drag_h, int drop_v, int drop_h,string vec)//プレイヤーブロックと演算子同士の計算
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

        PlayerBlock player = playerBlocksBoard[drag_v, drag_h].GetComponent<PlayerBlock>();

        

        switch (OperatorNumber)//演算子
        {
            case 0://add

                result = playerBlockNumber + calcBlockNumber;
                Debug.Log("足し算:" + playerBlockNumber+"+" + calcBlockNumber +"=" +result);
                AddScore((int)player.playerblocks,result);
                PlayerCalcBlockReInstallBoard(drag_v, drag_h, drop_v, drop_h, vec);

                break;
            case 1://sub

                result = playerBlockNumber - calcBlockNumber;
                Debug.Log("引き算:" + playerBlockNumber + "-" + calcBlockNumber + "=" + result);
                AddScore((int)player.playerblocks, result);
                PlayerCalcBlockReInstallBoard(drag_v, drag_h, drop_v, drop_h, vec);

                break;
            case 2://mult

                result = playerBlockNumber * calcBlockNumber;
                Debug.Log("掛け算:" + playerBlockNumber + "*" + calcBlockNumber + "=" + result);
                AddScore((int)player.playerblocks, result);
                PlayerCalcBlockReInstallBoard(drag_v, drag_h, drop_v, drop_h, vec);

                break;
            case 3://div

                result = playerBlockNumber / calcBlockNumber;
                Debug.Log("割り算:" + playerBlockNumber + "/" + calcBlockNumber + "=" + result);
                AddScore((int)player.playerblocks, result);
                PlayerCalcBlockReInstallBoard(drag_v, drag_h,drop_v,drop_h, vec);

                break;
        }
    }

    void AddScore(int playerNum,int score)//対応したプレイヤーにスコアを合算する
    {
        switch (playerNum)
        {
            case 1://player1
                score += Player1Score;
                Player1Score = score;
                player1scoreSc.AddScore(score);
                break;

            case 2://player2
                score += Player2Score;
                Player2Score = score;
                player2scoreSc.AddScore(score);
                break;
        }
    }
    

    /*-演算子同士の計算のブロック処理-*/
    void CalcBlockReInstallBoard(int drag_v,int drag_h,string vec)//ブロックを動かした向きにブロックを詰める
    {
        Vector3 initpos = BlocksObjectBoard[drag_v, drag_h].transform.position;

        int objecttype = 0;

        if (BlocksObjectBoard[drag_v, drag_h].tag == "PlayerBlock")//削除されるオブジェクトの種類を判別
        {
            objecttype = 1;
            PlayerBlock _player =  BlocksObjectBoard[drag_v, drag_h].GetComponent<PlayerBlock>();
            if((int)_player.playerblocks == 2)
            {
                objecttype = 2;
            }
        }
        else
        {
            objecttype = 3;
        }

        switch (vec)//ドラッグした向きに合わせ列を積める
        {
            case "up":
                Debug.Log(CalculationBoard.GetLength(0) - drag_v + "個選択");
                Debug.Log(BlocksObjectBoard[drag_v, drag_h].transform.position);
      
                Destroy(BlocksObjectBoard[drag_v, drag_h]);
                for (int v = 1; v < CalculationBoard.GetLength(0) - drag_v; v++)
                {

                    if (CalculationBoard[drag_v + v, drag_h] == 1 || CalculationBoard[drag_v + v, drag_h] == 2)
                    {
                       
                        Debug.Log((drag_v + v) + "," + drag_h + "を選択中");
                        PlayerBlock playerBlock = playerBlocksBoard[drag_v + v, drag_h].GetComponent<PlayerBlock>();//内部情報の変更
                        playerBlock.vertical = playerBlock.vertical - 1;//内部スクリプトの座標変数変更
                        BlocksObjectBoard[(drag_v + v) - 1, drag_h] = BlocksObjectBoard[drag_v + v, drag_h];
                        CalculationBoard[(drag_v + v) - 1, drag_h] = CalculationBoard[drag_v + v, drag_h];
                        BlocksImages[(drag_v + v) - 1, drag_h] = playerBlock.Highlightimage;
                        playerBlocksBoard[(drag_v + v) - 1, drag_h] = playerBlock;
                        BlocksObjectBoard[(drag_v + v) - 1, drag_h].transform.position = new Vector3(GenerateInitPos.transform.position.x + Block_distance * drag_h, initpos.y - Block_distance * (v - 1), 0);//画面座標変更
                      
                        Debug.Log("移動完了");
                    }
                    else if (CalculationBoard[drag_v + v, drag_h] == 3)
                    {
                      
                        Debug.Log((drag_v + v) + "," + drag_h + "を選択中");
                        CalcBlock calcBlock = calcBlocksBoard[drag_v + v, drag_h].GetComponent<CalcBlock>(); //内部情報の変更
                        calcBlock.vertical = calcBlock.vertical - 1;//内部スクリプトの座標変数変更
                        BlocksObjectBoard[(drag_v + v) - 1, drag_h] = BlocksObjectBoard[drag_v + v, drag_h];
                        CalculationBoard[(drag_v + v) - 1, drag_h] = CalculationBoard[drag_v + v, drag_h];
                        BlocksImages[(drag_v + v) - 1, drag_h] = calcBlock.Highlightimage;
                        calcBlocksBoard[(drag_v + v) - 1, drag_h] = calcBlock;
                        BlocksObjectBoard[(drag_v + v) - 1, drag_h].transform.position = new Vector3(GenerateInitPos.transform.position.x + Block_distance * drag_h, initpos.y - Block_distance * (v - 1), 0);//画面座標変更
                        
                        Debug.Log("移動完了");
                    }
                    else
                    {
                        Debug.Log("マスに何も入っていません");
                    }
                    
                }

                BlocksObjectBoard[CalculationBoard.GetLength(0)-1,drag_h] = null;//空いたマスにnullを入れます

                AddBlockBoard("up",drag_v,drag_h,objecttype);

                break;
            case "down":
                Debug.Log(drag_v + "個選択");
              
                Destroy(BlocksObjectBoard[drag_v, drag_h]);
                for (int v = 1; v <= drag_v; v++)
                {

                    if (CalculationBoard[drag_v - v, drag_h] == 1 || CalculationBoard[drag_v - v, drag_h] == 2)
                    {
                      
                        Debug.Log((drag_v - v) + "," + drag_h +"を選択中");
                        PlayerBlock playerBlock = playerBlocksBoard[drag_v - v, drag_h].GetComponent<PlayerBlock>();//内部情報の変更
                        playerBlock.vertical = playerBlock.vertical + 1;//内部スクリプトの座標変数変更
                        BlocksObjectBoard[(drag_v - v) + 1, drag_h] = BlocksObjectBoard[drag_v - v, drag_h];
                        CalculationBoard[(drag_v - v) + 1, drag_h] = CalculationBoard[drag_v - v, drag_h];
                        BlocksImages[(drag_v - v) + 1, drag_h] = playerBlock.Highlightimage;
                        playerBlocksBoard[(drag_v - v) + 1, drag_h] = playerBlock;
                        BlocksObjectBoard[(drag_v - v) + 1, drag_h].transform.position = new Vector3(GenerateInitPos.transform.position.x + Block_distance * drag_h, initpos.y - Block_distance *  (1-v), 0);//画面座標変更
                        
                        Debug.Log("移動完了");
                    }
                    else if (CalculationBoard[drag_v - v, drag_h] == 3)
                    {
                      
                        Debug.Log((drag_v - v) + "," + drag_h + "を選択中");
                        CalcBlock calcBlock = calcBlocksBoard[drag_v - v, drag_h].GetComponent<CalcBlock>();

                        //内部情報の変更
                        calcBlock.vertical = calcBlock.vertical + 1;//内部スクリプトの座標変数変更
                        BlocksObjectBoard[(drag_v - v) + 1, drag_h] = BlocksObjectBoard[drag_v - v, drag_h];
                        CalculationBoard[(drag_v - v) + 1, drag_h] = CalculationBoard[drag_v - v, drag_h];
                        BlocksImages[(drag_v - v) + 1, drag_h] = calcBlock.Highlightimage;
                        calcBlocksBoard[(drag_v - v) + 1, drag_h] = calcBlock;
                        BlocksObjectBoard[(drag_v - v) + 1, drag_h].transform.position = new Vector3(GenerateInitPos.transform.position.x + Block_distance * drag_h, initpos.y - Block_distance *  (1-v), 0);//画面座標変更
                       
                        Debug.Log("移動完了");
                    }
                    else
                    {
                        Debug.Log("マスに何も入っていません");
                    }
                  
                }

                BlocksObjectBoard[0, drag_h] = null;

                AddBlockBoard("down", drag_v, drag_h,objecttype);

                break;
            case "left":

                Debug.Log(CalculationBoard.GetLength(1) -drag_h + "個選択");
                Destroy(BlocksObjectBoard[drag_v, drag_h]);
                for (int h = 1; h < CalculationBoard.GetLength(1)- drag_h; h++)
                {   
                    if (CalculationBoard[drag_v, drag_h+h] == 1 || CalculationBoard[drag_v, drag_h+h] == 2)
                    {
                      
                        Debug.Log(drag_v+ "," + (drag_h+h) + "を選択中");
                        PlayerBlock playerBlock = playerBlocksBoard[drag_v, drag_h + h].GetComponent<PlayerBlock>();//内部情報の変更
                        playerBlock.horizontal = playerBlock.horizontal - 1;//内部スクリプトの座標変数変更
                        BlocksObjectBoard[drag_v, (drag_h + h) - 1] = BlocksObjectBoard[drag_v, drag_h + h];
                        CalculationBoard[drag_v, (drag_h + h) - 1] = CalculationBoard[drag_v, drag_h + h];
                        BlocksImages[drag_v, (drag_h + h) - 1] = playerBlock.Highlightimage;
                        playerBlocksBoard[drag_v, (drag_h + h) - 1] = playerBlock;
                        BlocksObjectBoard[drag_v, (drag_h + h) - 1].transform.position = new Vector3(initpos.x + Block_distance *  (h-1), GenerateInitPos.transform.position.y - Block_distance * drag_v, 0);//画面座標変更
                       
                        Debug.Log("移動完了");
                    }
                    else if (CalculationBoard[drag_v, drag_h+h] == 3)
                    {
                
                        Debug.Log(drag_v + "," + (drag_h + h) + "を選択中");
                        CalcBlock calcBlock = calcBlocksBoard[drag_v, drag_h + h].GetComponent<CalcBlock>();

                        //内部情報の変更
                        calcBlock.horizontal = calcBlock.horizontal - 1;//内部スクリプトの座標変数変更
                        BlocksObjectBoard[drag_v, (drag_h + h) - 1] = BlocksObjectBoard[drag_v, drag_h + h];
                        CalculationBoard[drag_v, (drag_h + h) - 1] = CalculationBoard[drag_v, drag_h + h];
                        BlocksImages[drag_v, (drag_h + h) - 1] = calcBlock.Highlightimage;
                        calcBlocksBoard[drag_v, (drag_h + h) - 1] = calcBlock;
                        BlocksObjectBoard[drag_v, (drag_h + h) - 1].transform.position = new Vector3(initpos.x + Block_distance * (h-1), GenerateInitPos.transform.position.y - Block_distance * drag_v, 0);//画面座標変更
                      
                        Debug.Log("移動完了");

                    }
                    else
                    {
                        Debug.Log("マスに何も入っていません");
                    }
                }
                BlocksObjectBoard[drag_v, CalculationBoard.GetLength(1)-1] = null;
                AddBlockBoard("left", drag_v, drag_h,objecttype);
                break;
            case "right":
                Debug.Log(drag_h + "個選択");
                Destroy(BlocksObjectBoard[drag_v, drag_h]);
                for (int h = 1; h <= drag_h; h++)
                {
                    if (CalculationBoard[drag_v, drag_h - h] == 1 || CalculationBoard[drag_v, drag_h - h] == 2)
                    {
                      
                        Debug.Log(drag_v + "," + (drag_h - h) + "を選択中");
                        PlayerBlock playerBlock = playerBlocksBoard[drag_v, drag_h - h].GetComponent<PlayerBlock>();//内部情報の変更
                        playerBlock.horizontal = playerBlock.horizontal + 1;//内部スクリプトの座標変数変更
                        BlocksObjectBoard[drag_v, (drag_h - h) + 1] = BlocksObjectBoard[drag_v, drag_h - h];
                        CalculationBoard[drag_v, (drag_h - h) + 1] = CalculationBoard[drag_v, drag_h - h];
                        BlocksImages[drag_v, (drag_h - h) + 1] = playerBlock.Highlightimage;
                        playerBlocksBoard[drag_v, (drag_h - h) + 1] = playerBlock;
                        BlocksObjectBoard[drag_v, (drag_h - h) + 1].transform.position = new Vector3(initpos.x + Block_distance *(1-h), GenerateInitPos.transform.position.y - Block_distance * drag_v, 0);//画面座標変更
                       
                        Debug.Log("移動完了");
                    }
                    else if (CalculationBoard[drag_v, drag_h - h] == 3)
                    {
                        Debug.Log(drag_v + "," + (drag_h - h) + "を選択中");
                        CalcBlock calcBlock = calcBlocksBoard[drag_v, drag_h - h].GetComponent<CalcBlock>();
                        //内部情報の変更
                        calcBlock.horizontal = calcBlock.horizontal + 1;//内部スクリプトの座標変数変更
                        BlocksObjectBoard[drag_v, (drag_h - h) + 1] = BlocksObjectBoard[drag_v, drag_h - h];
                        CalculationBoard[drag_v, (drag_h - h) + 1] = CalculationBoard[drag_v, drag_h - h];
                        BlocksImages[drag_v, (drag_h - h) + 1] = calcBlock.Highlightimage;
                        calcBlocksBoard[drag_v, (drag_h - h) + 1] = calcBlock;
                        BlocksObjectBoard[drag_v, (drag_h - h) + 1].transform.position = new Vector3(initpos.x + Block_distance * (1-h), GenerateInitPos.transform.position.y - Block_distance * drag_v, 0);//画面座標変更
                        
                        Debug.Log("移動完了");

                    }
                    else
                    {
                        Debug.Log("マスに何も入っていません");
                    }
                   
                }

                BlocksObjectBoard[drag_v,0] = null;
                AddBlockBoard("right", drag_v, drag_h,objecttype);

                break;

        }
    }


    void AddBlockBoard(string vec, int v, int h,int type)//空いたマスにブロックを生成する
    {
        switch (vec)
        {
            case "up":

                Debug.Log("上方向");
                for (int var = 0; var < CalculationBoard.GetLength(0); var++)
                {
                    
                   if(BlocksObjectBoard[var,h] == null)
                    {

                        Debug.Log("オブジェクトを生成します" +var+","+h);
                        if (type == 1)
                        {
                            CalculationBoard[var, h] = 1;
                            BlocksObjectBoard[var,h] = Instantiate(Blocks[1], new Vector3(GenerateInitPos.transform.position.x + Block_distance * h, GenerateInitPos.transform.position.y - Block_distance * var, 0)
                            , Quaternion.identity, BlocksParent.transform);
                            playerBlocksBoard[var, h] = BlocksObjectBoard[var, h].GetComponent<PlayerBlock>();
                            BlocksImages[var, h] = playerBlocksBoard[var, h].Highlightimage;
                            playerBlocksBoard[var, h].vertical = var;
                            playerBlocksBoard[var, h].horizontal = h;
                            playerBlocksBoard[var, h].AssignNum();
                        }
                        else if (type == 2)
                        {
                            CalculationBoard[var, h] = 2;
                            BlocksObjectBoard[var, h] = Instantiate(Blocks[2], new Vector3(GenerateInitPos.transform.position.x + Block_distance * h, GenerateInitPos.transform.position.y - Block_distance * var, 0)
                            , Quaternion.identity, BlocksParent.transform);
                            playerBlocksBoard[var, h] = BlocksObjectBoard[var, h].GetComponent<PlayerBlock>();
                            BlocksImages[var, h] = playerBlocksBoard[var, h].Highlightimage;
                            playerBlocksBoard[var, h].vertical = var;
                            playerBlocksBoard[var, h].horizontal = h;
                            playerBlocksBoard[var, h].AssignNum();
                        }
                        else if(type ==3)
                        {
                            CalculationBoard[var, h] = 3;
                            BlocksObjectBoard[var, h] = Instantiate(Blocks[3], new Vector3(GenerateInitPos.transform.position.x + Block_distance * h, GenerateInitPos.transform.position.y - Block_distance * var, 0)
                            , Quaternion.identity, BlocksParent.transform);
                            calcBlocksBoard[var, h] = BlocksObjectBoard[var, h].GetComponent<CalcBlock>();
                            BlocksImages[var, h] = calcBlocksBoard[var, h].Highlightimage;
                            calcBlocksBoard[var, h].vertical = var;
                            calcBlocksBoard[var, h].horizontal = h;
                            calcBlocksBoard[var, h].AssignColc();
                        }
                        else
                        {
                            Debug.LogError("不明なタイプの引数です");
                        }

                        
                    }
                    else
                    {
                        Debug.Log("オブジェクトは存在します" +var+","+h);
                    }
                }
                break;
            case "down":
                Debug.Log("下方向");
                for (int var = 0; var < CalculationBoard.GetLength(0); var++)
                {
                    if (BlocksObjectBoard[var, h] == null)
                    {
                        Debug.Log("オブジェクトを生成します" + var + "," + h);
                        if (type == 1)
                        {
                            CalculationBoard[var, h] = 1;
                            BlocksObjectBoard[var, h] = Instantiate(Blocks[1], new Vector3(GenerateInitPos.transform.position.x + Block_distance * h, GenerateInitPos.transform.position.y - Block_distance * var, 0)
                            , Quaternion.identity, BlocksParent.transform);
                            playerBlocksBoard[var, h] = BlocksObjectBoard[var, h].GetComponent<PlayerBlock>();
                            BlocksImages[var, h] = playerBlocksBoard[var, h].Highlightimage;
                            playerBlocksBoard[var, h].vertical = var;
                            playerBlocksBoard[var, h].horizontal = h;
                            playerBlocksBoard[var, h].AssignNum();
                        }
                        else if (type == 2)
                        {
                            CalculationBoard[var, h] = 2;
                            BlocksObjectBoard[var, h] = Instantiate(Blocks[2], new Vector3(GenerateInitPos.transform.position.x + Block_distance * h, GenerateInitPos.transform.position.y - Block_distance * var, 0)
                            , Quaternion.identity, BlocksParent.transform);
                            playerBlocksBoard[var, h] = BlocksObjectBoard[var, h].GetComponent<PlayerBlock>();
                            BlocksImages[var, h] = playerBlocksBoard[var, h].Highlightimage;
                            playerBlocksBoard[var, h].vertical = var;
                            playerBlocksBoard[var, h].horizontal = h;
                            playerBlocksBoard[var, h].AssignNum();
                        }
                        else if (type == 3)
                        {
                            CalculationBoard[var, h] = 3;
                            BlocksObjectBoard[var, h] = Instantiate(Blocks[3], new Vector3(GenerateInitPos.transform.position.x + Block_distance * h, GenerateInitPos.transform.position.y - Block_distance * var, 0)
                            , Quaternion.identity, BlocksParent.transform);
                            calcBlocksBoard[var, h] = BlocksObjectBoard[var, h].GetComponent<CalcBlock>();
                            BlocksImages[var, h] = calcBlocksBoard[var, h].Highlightimage;
                            calcBlocksBoard[var, h].vertical = var;
                            calcBlocksBoard[var, h].horizontal = h;
                            calcBlocksBoard[var, h].AssignColc();
                        }
                        else
                        {
                            Debug.LogError("不明なタイプの引数です");
                        }


                    }
                }
                break;

            case "left":
                Debug.Log("左方向");
                for (int hor = 0; hor < CalculationBoard.GetLength(1); hor++)
                {
                    if (BlocksObjectBoard[v, hor] == null)
                    {
                        Debug.Log("オブジェクトを生成します" + v + "," + hor);
                        if (type == 1)
                        {
                            CalculationBoard[v, hor] = 1;
                            BlocksObjectBoard[v, hor] = Instantiate(Blocks[1], new Vector3(GenerateInitPos.transform.position.x + Block_distance * hor, GenerateInitPos.transform.position.y - Block_distance * v, 0)
                            , Quaternion.identity, BlocksParent.transform);
                            playerBlocksBoard[v, hor] = BlocksObjectBoard[v, hor].GetComponent<PlayerBlock>();
                            BlocksImages[v, hor] = playerBlocksBoard[v, hor].Highlightimage;
                            playerBlocksBoard[v, hor].vertical = v;
                            playerBlocksBoard[v, hor].horizontal = hor;
                            playerBlocksBoard[v, hor].AssignNum();
                        }
                        else if (type == 2)
                        {
                            CalculationBoard[v, hor] = 2;
                            BlocksObjectBoard[v, hor] = Instantiate(Blocks[2], new Vector3(GenerateInitPos.transform.position.x + Block_distance * hor, GenerateInitPos.transform.position.y - Block_distance * v, 0)
                            , Quaternion.identity, BlocksParent.transform);
                            playerBlocksBoard[v, hor] = BlocksObjectBoard[v, hor].GetComponent<PlayerBlock>();
                            BlocksImages[v, hor] = playerBlocksBoard[v, hor].Highlightimage;
                            playerBlocksBoard[v, hor].vertical = v;
                            playerBlocksBoard[v, hor].horizontal = hor;
                            playerBlocksBoard[v, hor].AssignNum();
                        }
                        else if (type == 3)
                        {
                            CalculationBoard[v, hor] = 3;
                            BlocksObjectBoard[v, hor] = Instantiate(Blocks[3], new Vector3(GenerateInitPos.transform.position.x + Block_distance * hor, GenerateInitPos.transform.position.y - Block_distance * v, 0)
                            , Quaternion.identity, BlocksParent.transform);
                            calcBlocksBoard[v, hor] = BlocksObjectBoard[v, hor].GetComponent<CalcBlock>();
                            BlocksImages[v, hor] = calcBlocksBoard[v, hor].Highlightimage;
                            calcBlocksBoard[v, hor].vertical = v;
                            calcBlocksBoard[v, hor].horizontal = hor;
                            calcBlocksBoard[v, hor].AssignColc();
                        }
                        else
                        {
                            Debug.LogError("不明なタイプの引数です");
                        }
                    }
                }
                break;
            case "right":
                Debug.Log("右方向");
                for (int hor = 0; hor < CalculationBoard.GetLength(1); hor++)
                {
                    if (BlocksObjectBoard[v, hor] == null)
                    {
                        Debug.Log("オブジェクトを生成します" + v + "," + hor);
                        if (type == 1)
                        {
                            CalculationBoard[v, hor] = 1;
                            BlocksObjectBoard[v, hor] = Instantiate(Blocks[1], new Vector3(GenerateInitPos.transform.position.x + Block_distance * hor, GenerateInitPos.transform.position.y - Block_distance * v, 0)
                            , Quaternion.identity, BlocksParent.transform);
                            playerBlocksBoard[v, hor] = BlocksObjectBoard[v, hor].GetComponent<PlayerBlock>();
                            BlocksImages[v, hor] = playerBlocksBoard[v, hor].Highlightimage;
                            playerBlocksBoard[v, hor].vertical = v;
                            playerBlocksBoard[v, hor].horizontal = hor;
                            playerBlocksBoard[v, hor].AssignNum();
                        }
                        else if (type == 2)
                        {
                            CalculationBoard[v, hor] = 2;
                            BlocksObjectBoard[v, hor] = Instantiate(Blocks[2], new Vector3(GenerateInitPos.transform.position.x + Block_distance * hor, GenerateInitPos.transform.position.y - Block_distance * v, 0)
                            , Quaternion.identity, BlocksParent.transform);
                            playerBlocksBoard[v, hor] = BlocksObjectBoard[v, hor].GetComponent<PlayerBlock>();
                            BlocksImages[v, hor] = playerBlocksBoard[v, hor].Highlightimage;
                            playerBlocksBoard[v, hor].vertical = v;
                            playerBlocksBoard[v, hor].horizontal = hor;
                            playerBlocksBoard[v, hor].AssignNum();
                        }
                        else if (type == 3)
                        {
                            CalculationBoard[v, hor] = 3;
                            BlocksObjectBoard[v, hor] = Instantiate(Blocks[3], new Vector3(GenerateInitPos.transform.position.x + Block_distance * hor, GenerateInitPos.transform.position.y - Block_distance * v, 0)
                            , Quaternion.identity, BlocksParent.transform);
                            calcBlocksBoard[v, hor] = BlocksObjectBoard[v, hor].GetComponent<CalcBlock>();
                            BlocksImages[v, hor] = calcBlocksBoard[v, hor].Highlightimage;
                            calcBlocksBoard[v, hor].vertical = v;
                            calcBlocksBoard[v, hor].horizontal = hor;
                            calcBlocksBoard[v, hor].AssignColc();
                        }
                        else
                        {
                            Debug.LogError("不明なタイプの引数です");
                        }
                    }
                }
                break;

        }
    }

    /*-プレイヤーブロックと演算子同士の計算のブロック処理-*/
    void PlayerCalcBlockReInstallBoard(int drag_v, int drag_h,int drop_v,int drop_h, string vec)//ブロックを動かした向きにブロックを詰める
    {
        Vector3 initpos = BlocksObjectBoard[drop_v, drop_h].transform.position;

        int objecttype1 = 0;
        int objecttype2 = 0;
        if (BlocksObjectBoard[drag_v, drag_h].tag == "PlayerBlock")//削除されるオブジェクトの種類を判別
        {
            objecttype1 = 1;
            PlayerBlock _player = BlocksObjectBoard[drag_v, drag_h].GetComponent<PlayerBlock>();
            if ((int)_player.playerblocks == 2)
            {
                objecttype1 = 2;
            }
        }

        if (BlocksObjectBoard[drop_v, drop_h].tag == "CalcBlock")//削除されるオブジェクトの種類を判別
        {
            objecttype2 = 3;

        }


        switch (vec)//ドラッグした向きに合わせ列を積める
        {
            case "up":
                Debug.Log(CalculationBoard.GetLength(0) - drag_v + "個選択");
                Debug.Log(BlocksObjectBoard[drag_v, drag_h].transform.position);

                Destroy(BlocksObjectBoard[drag_v, drag_h]);
                Destroy(BlocksObjectBoard[drop_v, drop_h]);
                for (int v = 2; v < CalculationBoard.GetLength(0) - drop_v; v++)
                {

                    if (CalculationBoard[drop_v + v, drop_h] == 1 || CalculationBoard[drop_v + v, drop_h] == 2)
                    {

                        Debug.Log((drop_v + v) + "," + drop_h + "を選択中");
                        PlayerBlock playerBlock = playerBlocksBoard[drop_v + v, drop_h].GetComponent<PlayerBlock>();//内部情報の変更
                        playerBlock.vertical = playerBlock.vertical - 2;//内部スクリプトの座標変数変更
                        BlocksObjectBoard[(drop_v + v) - 2, drop_h] = BlocksObjectBoard[drop_v + v, drop_h];
                        CalculationBoard[(drop_v + v) - 2, drop_h] = CalculationBoard[drop_v + v, drop_h];
                        BlocksImages[(drop_v + v) - 2, drop_h] = playerBlock.Highlightimage;
                        playerBlocksBoard[(drop_v + v) - 2, drop_h] = playerBlock;
                        BlocksObjectBoard[(drop_v + v) - 2, drop_h].transform.position = new Vector3(GenerateInitPos.transform.position.x + Block_distance * drop_h, initpos.y - Block_distance * (v - 2), 0);//画面座標変更

                        Debug.Log("移動完了");
                    }
                    else if (CalculationBoard[drop_v + v, drop_h] == 3)
                    {

                        Debug.Log((drop_v + v) + "," + drop_h + "を選択中");
                        CalcBlock calcBlock = calcBlocksBoard[drop_v + v, drop_h].GetComponent<CalcBlock>(); //内部情報の変更
                        calcBlock.vertical = calcBlock.vertical - 2;//内部スクリプトの座標変数変更
                        BlocksObjectBoard[(drop_v + v) - 2, drop_h] = BlocksObjectBoard[drop_v + v, drop_h];
                        CalculationBoard[(drop_v + v) - 2, drop_h] = CalculationBoard[drop_v + v, drop_h];
                        BlocksImages[(drop_v + v) - 2, drop_h] = calcBlock.Highlightimage;
                        calcBlocksBoard[(drop_v + v) - 2, drop_h] = calcBlock;
                        BlocksObjectBoard[(drop_v + v) - 2, drop_h].transform.position = new Vector3(GenerateInitPos.transform.position.x + Block_distance * drop_h, initpos.y - Block_distance * (v - 2), 0);//画面座標変更

                        Debug.Log("移動完了");
                    }
                    else
                    {
                        Debug.Log("マスに何も入っていません");
                    }

                }

                BlocksObjectBoard[CalculationBoard.GetLength(0) - 2, drop_h] = null;//空いたマスにnullを入れます
                BlocksObjectBoard[CalculationBoard.GetLength(0) - 1, drop_h] = null;//空いたマスにnullを入れます
                PlayerCalcAddBlocksBoard("up", drag_v, drag_h, objecttype1, objecttype2);

                break;
            case "down":
                Debug.Log(drop_v + "個選択");

                Destroy(BlocksObjectBoard[drag_v, drag_h]);
                Destroy(BlocksObjectBoard[drop_v, drop_h]);
                for (int v = 2; v <= drop_v; v++)
                {

                    if (CalculationBoard[drop_v - v, drop_h] == 1 || CalculationBoard[drop_v - v, drop_h] == 2)
                    {

                        Debug.Log((drop_v - v) + "," + drop_h + "を選択中");
                        PlayerBlock playerBlock = playerBlocksBoard[drop_v - v, drop_h].GetComponent<PlayerBlock>();//内部情報の変更
                        playerBlock.vertical = playerBlock.vertical + 2;//内部スクリプトの座標変数変更
                        BlocksObjectBoard[(drop_v - v) + 2, drop_h] = BlocksObjectBoard[drop_v - v, drop_h];
                        CalculationBoard[(drop_v - v) + 2, drop_h] = CalculationBoard[drop_v - v, drop_h];
                        BlocksImages[(drop_v - v) + 2, drop_h] = playerBlock.Highlightimage;
                        playerBlocksBoard[(drop_v - v) + 2, drop_h] = playerBlock;
                        BlocksObjectBoard[(drop_v - v) + 2, drop_h].transform.position = new Vector3(GenerateInitPos.transform.position.x + Block_distance * drop_h, initpos.y - Block_distance * (2 - v), 0);//画面座標変更

                        Debug.Log("移動完了");
                    }
                    else if (CalculationBoard[drop_v - v, drop_h] == 3)
                    {

                        Debug.Log((drop_v - v) + "," + drop_h + "を選択中");
                        CalcBlock calcBlock = calcBlocksBoard[drop_v - v, drop_h].GetComponent<CalcBlock>();

                        //内部情報の変更
                        calcBlock.vertical = calcBlock.vertical + 2;//内部スクリプトの座標変数変更
                        BlocksObjectBoard[(drop_v - v) + 2, drop_h] = BlocksObjectBoard[drop_v - v, drop_h];
                        CalculationBoard[(drop_v - v) + 2, drop_h] = CalculationBoard[drop_v - v, drop_h];
                        BlocksImages[(drop_v - v) + 2, drop_h] = calcBlock.Highlightimage;
                        calcBlocksBoard[(drop_v - v) + 2, drop_h] = calcBlock;
                        BlocksObjectBoard[(drop_v - v) + 2, drop_h].transform.position = new Vector3(GenerateInitPos.transform.position.x + Block_distance * drop_h, initpos.y - Block_distance * (2 - v), 0);//画面座標変更

                        Debug.Log("移動完了");
                    }
                    else
                    {
                        Debug.Log("マスに何も入っていません");
                    }

                }

                BlocksObjectBoard[0, drag_h] = null;
                BlocksObjectBoard[1, drop_h] = null;
                PlayerCalcAddBlocksBoard("down", drag_v, drag_h, objecttype1, objecttype2);

                break;
            case "left":

                Debug.Log(CalculationBoard.GetLength(1) - drop_h + "個選択");
                Destroy(BlocksObjectBoard[drag_v, drag_h]);
                Destroy(BlocksObjectBoard[drop_v, drop_h]);
                for (int h = 2; h < CalculationBoard.GetLength(1) - drop_h; h++)
                {
                    if (CalculationBoard[drop_v, drop_h + h] == 1 || CalculationBoard[drop_v, drop_h + h] == 2)
                    {

                        Debug.Log(drop_v + "," + (drop_h + h) + "を選択中");
                        PlayerBlock playerBlock = playerBlocksBoard[drop_v, drop_h + h].GetComponent<PlayerBlock>();//内部情報の変更
                        playerBlock.horizontal = playerBlock.horizontal - 2;//内部スクリプトの座標変数変更
                        BlocksObjectBoard[drop_v, (drop_h + h) - 2] = BlocksObjectBoard[drop_v, drop_h + h];
                        CalculationBoard[drop_v, (drop_h + h) - 2] = CalculationBoard[drop_v, drop_h + h];
                        BlocksImages[drop_v, (drop_h + h) - 2] = playerBlock.Highlightimage;
                        playerBlocksBoard[drop_v, (drop_h + h) - 2] = playerBlock;
                        BlocksObjectBoard[drop_v, (drop_h + h) - 2].transform.position = new Vector3(initpos.x + Block_distance * (h - 2), GenerateInitPos.transform.position.y - Block_distance * drop_v, 0);//画面座標変更

                        Debug.Log("移動完了");
                    }
                    else if (CalculationBoard[drop_v, drop_h + h] == 3)
                    {

                        Debug.Log(drop_v + "," + (drop_h + h) + "を選択中");
                        CalcBlock calcBlock = calcBlocksBoard[drop_v, drop_h + h].GetComponent<CalcBlock>();

                        //内部情報の変更
                        calcBlock.horizontal = calcBlock.horizontal - 2;//内部スクリプトの座標変数変更
                        BlocksObjectBoard[drop_v, (drop_h + h) - 2] = BlocksObjectBoard[drop_v, drop_h + h];
                        CalculationBoard[drop_v, (drop_h + h) - 2] = CalculationBoard[drop_v, drop_h + h];
                        BlocksImages[drop_v, (drop_h + h) - 2] = calcBlock.Highlightimage;
                        calcBlocksBoard[drop_v, (drop_h + h) - 2] = calcBlock;
                        BlocksObjectBoard[drop_v, (drop_h + h) - 2].transform.position = new Vector3(initpos.x + Block_distance * (h - 2), GenerateInitPos.transform.position.y - Block_distance * drop_v, 0);//画面座標変更

                        Debug.Log("移動完了");

                    }
                    else
                    {
                        Debug.Log("マスに何も入っていません");
                    }
                }
                BlocksObjectBoard[drag_v, CalculationBoard.GetLength(1) - 1] = null;
                BlocksObjectBoard[drag_v, CalculationBoard.GetLength(1) - 2] = null;
                PlayerCalcAddBlocksBoard("left", drag_v, drag_h, objecttype1, objecttype2);
                break;
            case "right":
                Debug.Log(drop_h + "個選択");
                Destroy(BlocksObjectBoard[drag_v, drag_h]);
                Destroy(BlocksObjectBoard[drop_v, drop_h]);
                for (int h = 2; h <= drop_h; h++)
                {
                    if (CalculationBoard[drop_v, drop_h - h] == 1 || CalculationBoard[drop_v, drop_h - h] == 2)
                    {

                        Debug.Log(drop_v + "," + (drop_h - h) + "を選択中");
                        PlayerBlock playerBlock = playerBlocksBoard[drop_v, drop_h - h].GetComponent<PlayerBlock>();//内部情報の変更
                        playerBlock.horizontal = playerBlock.horizontal + 2;//内部スクリプトの座標変数変更
                        BlocksObjectBoard[drop_v, (drop_h - h) + 2] = BlocksObjectBoard[drop_v, drop_h - h];
                        CalculationBoard[drop_v, (drop_h - h) + 2] = CalculationBoard[drop_v, drop_h - h];
                        BlocksImages[drop_v, (drop_h - h) + 2] = playerBlock.Highlightimage;
                        playerBlocksBoard[drop_v, (drop_h - h) + 2] = playerBlock;
                        BlocksObjectBoard[drop_v, (drop_h - h) + 2].transform.position = new Vector3(initpos.x + Block_distance * (2 - h), GenerateInitPos.transform.position.y - Block_distance * drop_v, 0);//画面座標変更

                        Debug.Log("移動完了");
                    }
                    else if (CalculationBoard[drop_v, drop_h - h] == 3)
                    {
                        Debug.Log(drop_v + "," + (drop_h - h) + "を選択中");
                        CalcBlock calcBlock = calcBlocksBoard[drop_v, drop_h - h].GetComponent<CalcBlock>();
                        //内部情報の変更
                        calcBlock.horizontal = calcBlock.horizontal + 2;//内部スクリプトの座標変数変更
                        BlocksObjectBoard[drop_v, (drop_h - h) + 2] = BlocksObjectBoard[drop_v, drop_h - h];
                        CalculationBoard[drop_v, (drop_h - h) + 2] = CalculationBoard[drop_v, drop_h - h];
                        BlocksImages[drop_v, (drop_h - h) + 2] = calcBlock.Highlightimage;
                        calcBlocksBoard[drop_v, (drop_h - h) + 2] = calcBlock;
                        BlocksObjectBoard[drop_v, (drop_h - h) + 2].transform.position = new Vector3(initpos.x + Block_distance * (2 - h), GenerateInitPos.transform.position.y - Block_distance * drop_v, 0);//画面座標変更

                        Debug.Log("移動完了");

                    }
                    else
                    {
                        Debug.Log("マスに何も入っていません");
                    }

                }

                BlocksObjectBoard[drag_v, 0] = null;
                BlocksObjectBoard[drag_v, 1] = null;
                PlayerCalcAddBlocksBoard("right", drag_v, drag_h, objecttype1, objecttype2);

                break;

        }
    }


    void PlayerCalcAddBlocksBoard(string vec, int v, int h, int type1 ,int type2)//空いたマスにブロックを生成する
    {
        bool isputted =false;

        switch (vec)
        {
            case "up":

                Debug.Log("上方向");
                for (int var = 0; var < CalculationBoard.GetLength(0); var++)
                {

                    if (BlocksObjectBoard[var, h] == null)
                    {

                        Debug.Log("オブジェクトを生成します" + var + "," + h);
                        if (type1 == 1&& isputted==false)
                        {
                            CalculationBoard[var, h] = 1;
                            BlocksObjectBoard[var, h] = Instantiate(Blocks[1], new Vector3(GenerateInitPos.transform.position.x + Block_distance * h, GenerateInitPos.transform.position.y - Block_distance * var, 0)
                            , Quaternion.identity, BlocksParent.transform);
                            playerBlocksBoard[var, h] = BlocksObjectBoard[var, h].GetComponent<PlayerBlock>();
                            BlocksImages[var, h] = playerBlocksBoard[var, h].Highlightimage;
                            playerBlocksBoard[var, h].vertical = var;
                            playerBlocksBoard[var, h].horizontal = h;
                            playerBlocksBoard[var, h].AssignNum();
                            isputted = true;
                        }
                        else if (type1 == 2 && isputted == false)
                        {
                            CalculationBoard[var, h] = 2;
                            BlocksObjectBoard[var, h] = Instantiate(Blocks[2], new Vector3(GenerateInitPos.transform.position.x + Block_distance * h, GenerateInitPos.transform.position.y - Block_distance * var, 0)
                            , Quaternion.identity, BlocksParent.transform);
                            playerBlocksBoard[var, h] = BlocksObjectBoard[var, h].GetComponent<PlayerBlock>();
                            BlocksImages[var, h] = playerBlocksBoard[var, h].Highlightimage;
                            playerBlocksBoard[var, h].vertical = var;
                            playerBlocksBoard[var, h].horizontal = h;
                            playerBlocksBoard[var, h].AssignNum();
                            isputted = true;
                        }
                        else if (type1 == 3 && isputted == false)
                        {
                            CalculationBoard[var, h] = 3;
                            BlocksObjectBoard[var, h] = Instantiate(Blocks[3], new Vector3(GenerateInitPos.transform.position.x + Block_distance * h, GenerateInitPos.transform.position.y - Block_distance * var, 0)
                            , Quaternion.identity, BlocksParent.transform);
                            calcBlocksBoard[var, h] = BlocksObjectBoard[var, h].GetComponent<CalcBlock>();
                            BlocksImages[var, h] = calcBlocksBoard[var, h].Highlightimage;
                            calcBlocksBoard[var, h].vertical = var;
                            calcBlocksBoard[var, h].horizontal = h;
                            calcBlocksBoard[var, h].AssignColc();
                            isputted = true;
                        }
                        else if (type2 == 1 && isputted == true)
                        {
                            CalculationBoard[var, h] = 1;
                            BlocksObjectBoard[var, h] = Instantiate(Blocks[1], new Vector3(GenerateInitPos.transform.position.x + Block_distance * h, GenerateInitPos.transform.position.y - Block_distance * var, 0)
                            , Quaternion.identity, BlocksParent.transform);
                            playerBlocksBoard[var, h] = BlocksObjectBoard[var, h].GetComponent<PlayerBlock>();
                            BlocksImages[var, h] = playerBlocksBoard[var, h].Highlightimage;
                            playerBlocksBoard[var, h].vertical = var;
                            playerBlocksBoard[var, h].horizontal = h;
                            playerBlocksBoard[var, h].AssignNum();
                            isputted = false;
                        }
                        else if (type2 == 2 && isputted == true)
                        {
                            CalculationBoard[var, h] = 2;
                            BlocksObjectBoard[var, h] = Instantiate(Blocks[2], new Vector3(GenerateInitPos.transform.position.x + Block_distance * h, GenerateInitPos.transform.position.y - Block_distance * var, 0)
                            , Quaternion.identity, BlocksParent.transform);
                            playerBlocksBoard[var, h] = BlocksObjectBoard[var, h].GetComponent<PlayerBlock>();
                            BlocksImages[var, h] = playerBlocksBoard[var, h].Highlightimage;
                            playerBlocksBoard[var, h].vertical = var;
                            playerBlocksBoard[var, h].horizontal = h;
                            playerBlocksBoard[var, h].AssignNum();
                            isputted = false;
                        }
                        else if (type2 == 3 && isputted == true)
                        {
                            CalculationBoard[var, h] = 3;
                            BlocksObjectBoard[var, h] = Instantiate(Blocks[3], new Vector3(GenerateInitPos.transform.position.x + Block_distance * h, GenerateInitPos.transform.position.y - Block_distance * var, 0)
                            , Quaternion.identity, BlocksParent.transform);
                            calcBlocksBoard[var, h] = BlocksObjectBoard[var, h].GetComponent<CalcBlock>();
                            BlocksImages[var, h] = calcBlocksBoard[var, h].Highlightimage;
                            calcBlocksBoard[var, h].vertical = var;
                            calcBlocksBoard[var, h].horizontal = h;
                            calcBlocksBoard[var, h].AssignColc();
                            isputted = false;
                        }
                        else
                        {
                            Debug.LogError("不明なタイプの引数です");
                        }


                    }
                    else
                    {
                        Debug.Log("オブジェクトは存在します" + var + "," + h);
                    }
                }
                break;
            case "down":
                Debug.Log("下方向");
                for (int var = 0; var < CalculationBoard.GetLength(0); var++)
                {
                    if (BlocksObjectBoard[var, h] == null)
                    {
                        Debug.Log("オブジェクトを生成します" + var + "," + h);
                        if (type1 == 1 && isputted == false)
                        {
                            CalculationBoard[var, h] = 1;
                            BlocksObjectBoard[var, h] = Instantiate(Blocks[1], new Vector3(GenerateInitPos.transform.position.x + Block_distance * h, GenerateInitPos.transform.position.y - Block_distance * var, 0)
                            , Quaternion.identity, BlocksParent.transform);
                            playerBlocksBoard[var, h] = BlocksObjectBoard[var, h].GetComponent<PlayerBlock>();
                            BlocksImages[var, h] = playerBlocksBoard[var, h].Highlightimage;
                            playerBlocksBoard[var, h].vertical = var;
                            playerBlocksBoard[var, h].horizontal = h;
                            playerBlocksBoard[var, h].AssignNum();
                            isputted = true;
                        }
                        else if (type1 == 2 && isputted == false)
                        {
                            CalculationBoard[var, h] = 2;
                            BlocksObjectBoard[var, h] = Instantiate(Blocks[2], new Vector3(GenerateInitPos.transform.position.x + Block_distance * h, GenerateInitPos.transform.position.y - Block_distance * var, 0)
                            , Quaternion.identity, BlocksParent.transform);
                            playerBlocksBoard[var, h] = BlocksObjectBoard[var, h].GetComponent<PlayerBlock>();
                            BlocksImages[var, h] = playerBlocksBoard[var, h].Highlightimage;
                            playerBlocksBoard[var, h].vertical = var;
                            playerBlocksBoard[var, h].horizontal = h;
                            playerBlocksBoard[var, h].AssignNum();
                            isputted = true;
                        }
                        else if (type1 == 3 && isputted == false)
                        {
                            CalculationBoard[var, h] = 3;
                            BlocksObjectBoard[var, h] = Instantiate(Blocks[3], new Vector3(GenerateInitPos.transform.position.x + Block_distance * h, GenerateInitPos.transform.position.y - Block_distance * var, 0)
                            , Quaternion.identity, BlocksParent.transform);
                            calcBlocksBoard[var, h] = BlocksObjectBoard[var, h].GetComponent<CalcBlock>();
                            BlocksImages[var, h] = calcBlocksBoard[var, h].Highlightimage;
                            calcBlocksBoard[var, h].vertical = var;
                            calcBlocksBoard[var, h].horizontal = h;
                            calcBlocksBoard[var, h].AssignColc();
                            isputted = true;
                        }
                        else if (type2 == 1 && isputted == true)
                        {
                            CalculationBoard[var, h] = 1;
                            BlocksObjectBoard[var, h] = Instantiate(Blocks[1], new Vector3(GenerateInitPos.transform.position.x + Block_distance * h, GenerateInitPos.transform.position.y - Block_distance * var, 0)
                            , Quaternion.identity, BlocksParent.transform);
                            playerBlocksBoard[var, h] = BlocksObjectBoard[var, h].GetComponent<PlayerBlock>();
                            BlocksImages[var, h] = playerBlocksBoard[var, h].Highlightimage;
                            playerBlocksBoard[var, h].vertical = var;
                            playerBlocksBoard[var, h].horizontal = h;
                            playerBlocksBoard[var, h].AssignNum();
                            isputted = false;
                        }
                        else if (type2 == 2 && isputted == true)
                        {
                            CalculationBoard[var, h] = 2;
                            BlocksObjectBoard[var, h] = Instantiate(Blocks[2], new Vector3(GenerateInitPos.transform.position.x + Block_distance * h, GenerateInitPos.transform.position.y - Block_distance * var, 0)
                            , Quaternion.identity, BlocksParent.transform);
                            playerBlocksBoard[var, h] = BlocksObjectBoard[var, h].GetComponent<PlayerBlock>();
                            BlocksImages[var, h] = playerBlocksBoard[var, h].Highlightimage;
                            playerBlocksBoard[var, h].vertical = var;
                            playerBlocksBoard[var, h].horizontal = h;
                            playerBlocksBoard[var, h].AssignNum();
                            isputted = false;
                        }
                        else if (type2 == 3 && isputted == true)
                        {
                            CalculationBoard[var, h] = 3;
                            BlocksObjectBoard[var, h] = Instantiate(Blocks[3], new Vector3(GenerateInitPos.transform.position.x + Block_distance * h, GenerateInitPos.transform.position.y - Block_distance * var, 0)
                            , Quaternion.identity, BlocksParent.transform);
                            calcBlocksBoard[var, h] = BlocksObjectBoard[var, h].GetComponent<CalcBlock>();
                            BlocksImages[var, h] = calcBlocksBoard[var, h].Highlightimage;
                            calcBlocksBoard[var, h].vertical = var;
                            calcBlocksBoard[var, h].horizontal = h;
                            calcBlocksBoard[var, h].AssignColc();
                            isputted = false;
                        }
                        else
                        {
                            Debug.LogError("不明なタイプの引数です");
                        }


                    }
                }
                break;

            case "left":
                Debug.Log("左方向");
                for (int hor = 0; hor < CalculationBoard.GetLength(1); hor++)
                {
                    if (BlocksObjectBoard[v, hor] == null)
                    {
                        Debug.Log("オブジェクトを生成します" + v + "," + hor);
                        if (type1 == 1 && isputted == false)
                        {
                            CalculationBoard[v, hor] = 1;
                            BlocksObjectBoard[v, hor] = Instantiate(Blocks[1], new Vector3(GenerateInitPos.transform.position.x + Block_distance * hor, GenerateInitPos.transform.position.y - Block_distance * v, 0)
                            , Quaternion.identity, BlocksParent.transform);
                            playerBlocksBoard[v, hor] = BlocksObjectBoard[v, hor].GetComponent<PlayerBlock>();
                            BlocksImages[v, hor] = playerBlocksBoard[v, hor].Highlightimage;
                            playerBlocksBoard[v, hor].vertical = v;
                            playerBlocksBoard[v, hor].horizontal = hor;
                            playerBlocksBoard[v, hor].AssignNum();
                            isputted = true;
                        }
                        else if (type1 == 2 && isputted == false)
                        {
                            CalculationBoard[v, hor] = 2;
                            BlocksObjectBoard[v, hor] = Instantiate(Blocks[2], new Vector3(GenerateInitPos.transform.position.x + Block_distance * hor, GenerateInitPos.transform.position.y - Block_distance * v, 0)
                            , Quaternion.identity, BlocksParent.transform);
                            playerBlocksBoard[v, hor] = BlocksObjectBoard[v, hor].GetComponent<PlayerBlock>();
                            BlocksImages[v, hor] = playerBlocksBoard[v, hor].Highlightimage;
                            playerBlocksBoard[v, hor].vertical = v;
                            playerBlocksBoard[v, hor].horizontal = hor;
                            playerBlocksBoard[v, hor].AssignNum();
                            isputted = true;
                        }
                        else if (type1 == 3 && isputted == false)
                        {
                            CalculationBoard[v, hor] = 3;
                            BlocksObjectBoard[v, hor] = Instantiate(Blocks[3], new Vector3(GenerateInitPos.transform.position.x + Block_distance * hor, GenerateInitPos.transform.position.y - Block_distance * v, 0)
                            , Quaternion.identity, BlocksParent.transform);
                            calcBlocksBoard[v, hor] = BlocksObjectBoard[v, hor].GetComponent<CalcBlock>();
                            BlocksImages[v, hor] = calcBlocksBoard[v, hor].Highlightimage;
                            calcBlocksBoard[v, hor].vertical = v;
                            calcBlocksBoard[v, hor].horizontal = hor;
                            calcBlocksBoard[v, hor].AssignColc();
                            isputted = true;
                        }
                        else if (type2 == 1 && isputted == true)
                        {
                            CalculationBoard[v, hor] = 1;
                            BlocksObjectBoard[v, hor] = Instantiate(Blocks[1], new Vector3(GenerateInitPos.transform.position.x + Block_distance * hor, GenerateInitPos.transform.position.y - Block_distance * v, 0)
                            , Quaternion.identity, BlocksParent.transform);
                            playerBlocksBoard[v, hor] = BlocksObjectBoard[v, hor].GetComponent<PlayerBlock>();
                            BlocksImages[v, hor] = playerBlocksBoard[v, hor].Highlightimage;
                            playerBlocksBoard[v, hor].vertical = v;
                            playerBlocksBoard[v, hor].horizontal = hor;
                            playerBlocksBoard[v, hor].AssignNum();
                            isputted = false;
                        }
                        else if (type2 == 2 && isputted == true)
                        {
                            CalculationBoard[v, hor] = 2;
                            BlocksObjectBoard[v, hor] = Instantiate(Blocks[2], new Vector3(GenerateInitPos.transform.position.x + Block_distance * hor, GenerateInitPos.transform.position.y - Block_distance * v, 0)
                            , Quaternion.identity, BlocksParent.transform);
                            playerBlocksBoard[v, hor] = BlocksObjectBoard[v, hor].GetComponent<PlayerBlock>();
                            BlocksImages[v, hor] = playerBlocksBoard[v, hor].Highlightimage;
                            playerBlocksBoard[v, hor].vertical = v;
                            playerBlocksBoard[v, hor].horizontal = hor;
                            playerBlocksBoard[v, hor].AssignNum();
                            isputted = false;
                        }
                        else if (type2 == 3 && isputted == true)
                        {
                            CalculationBoard[v, hor] = 3;
                            BlocksObjectBoard[v, hor] = Instantiate(Blocks[3], new Vector3(GenerateInitPos.transform.position.x + Block_distance * hor, GenerateInitPos.transform.position.y - Block_distance * v, 0)
                            , Quaternion.identity, BlocksParent.transform);
                            calcBlocksBoard[v, hor] = BlocksObjectBoard[v, hor].GetComponent<CalcBlock>();
                            BlocksImages[v, hor] = calcBlocksBoard[v, hor].Highlightimage;
                            calcBlocksBoard[v, hor].vertical = v;
                            calcBlocksBoard[v, hor].horizontal = hor;
                            calcBlocksBoard[v, hor].AssignColc();
                            isputted = false;
                        }
                        else
                        {
                            Debug.LogError("不明なタイプの引数です");
                        }
                       

                    }
                }
                break;
            case "right":
                Debug.Log("右方向");
                for (int hor = 0; hor < CalculationBoard.GetLength(1); hor++)
                {
                    if (BlocksObjectBoard[v, hor] == null)
                    {
                        Debug.Log("オブジェクトを生成します" + v + "," + hor);
                        if (type1 == 1 && isputted == false)
                        {
                            CalculationBoard[v, hor] = 1;
                            BlocksObjectBoard[v, hor] = Instantiate(Blocks[1], new Vector3(GenerateInitPos.transform.position.x + Block_distance * hor, GenerateInitPos.transform.position.y - Block_distance * v, 0)
                            , Quaternion.identity, BlocksParent.transform);
                            playerBlocksBoard[v, hor] = BlocksObjectBoard[v, hor].GetComponent<PlayerBlock>();
                            BlocksImages[v, hor] = playerBlocksBoard[v, hor].Highlightimage;
                            playerBlocksBoard[v, hor].vertical = v;
                            playerBlocksBoard[v, hor].horizontal = hor;
                            playerBlocksBoard[v, hor].AssignNum();
                            isputted = true;
                        }
                        else if (type1 == 2 && isputted == false)
                        {
                            CalculationBoard[v, hor] = 2;
                            BlocksObjectBoard[v, hor] = Instantiate(Blocks[2], new Vector3(GenerateInitPos.transform.position.x + Block_distance * hor, GenerateInitPos.transform.position.y - Block_distance * v, 0)
                            , Quaternion.identity, BlocksParent.transform);
                            playerBlocksBoard[v, hor] = BlocksObjectBoard[v, hor].GetComponent<PlayerBlock>();
                            BlocksImages[v, hor] = playerBlocksBoard[v, hor].Highlightimage;
                            playerBlocksBoard[v, hor].vertical = v;
                            playerBlocksBoard[v, hor].horizontal = hor;
                            playerBlocksBoard[v, hor].AssignNum();
                            isputted = true;
                        }
                        else if (type1 == 3 && isputted == false)
                        {
                            CalculationBoard[v, hor] = 3;
                            BlocksObjectBoard[v, hor] = Instantiate(Blocks[3], new Vector3(GenerateInitPos.transform.position.x + Block_distance * hor, GenerateInitPos.transform.position.y - Block_distance * v, 0)
                            , Quaternion.identity, BlocksParent.transform);
                            calcBlocksBoard[v, hor] = BlocksObjectBoard[v, hor].GetComponent<CalcBlock>();
                            BlocksImages[v, hor] = calcBlocksBoard[v, hor].Highlightimage;
                            calcBlocksBoard[v, hor].vertical = v;
                            calcBlocksBoard[v, hor].horizontal = hor;
                            calcBlocksBoard[v, hor].AssignColc();
                            isputted = true;
                        }
                        else if (type2 == 1 && isputted == true)
                        {
                            CalculationBoard[v, hor] = 1;
                            BlocksObjectBoard[v, hor] = Instantiate(Blocks[1], new Vector3(GenerateInitPos.transform.position.x + Block_distance * hor, GenerateInitPos.transform.position.y - Block_distance * v, 0)
                            , Quaternion.identity, BlocksParent.transform);
                            playerBlocksBoard[v, hor] = BlocksObjectBoard[v, hor].GetComponent<PlayerBlock>();
                            BlocksImages[v, hor] = playerBlocksBoard[v, hor].Highlightimage;
                            playerBlocksBoard[v, hor].vertical = v;
                            playerBlocksBoard[v, hor].horizontal = hor;
                            playerBlocksBoard[v, hor].AssignNum();
                            isputted = false;
                        }
                        else if (type2 == 2 && isputted == true)
                        {
                            CalculationBoard[v, hor] = 2;
                            BlocksObjectBoard[v, hor] = Instantiate(Blocks[2], new Vector3(GenerateInitPos.transform.position.x + Block_distance * hor, GenerateInitPos.transform.position.y - Block_distance * v, 0)
                            , Quaternion.identity, BlocksParent.transform);
                            playerBlocksBoard[v, hor] = BlocksObjectBoard[v, hor].GetComponent<PlayerBlock>();
                            BlocksImages[v, hor] = playerBlocksBoard[v, hor].Highlightimage;
                            playerBlocksBoard[v, hor].vertical = v;
                            playerBlocksBoard[v, hor].horizontal = hor;
                            playerBlocksBoard[v, hor].AssignNum();
                            isputted = false;
                        }
                        else if (type2 == 3 && isputted == true)
                        {
                            CalculationBoard[v, hor] = 3;
                            BlocksObjectBoard[v, hor] = Instantiate(Blocks[3], new Vector3(GenerateInitPos.transform.position.x + Block_distance * hor, GenerateInitPos.transform.position.y - Block_distance * v, 0)
                            , Quaternion.identity, BlocksParent.transform);
                            calcBlocksBoard[v, hor] = BlocksObjectBoard[v, hor].GetComponent<CalcBlock>();
                            BlocksImages[v, hor] = calcBlocksBoard[v, hor].Highlightimage;
                            calcBlocksBoard[v, hor].vertical = v;
                            calcBlocksBoard[v, hor].horizontal = hor;
                            calcBlocksBoard[v, hor].AssignColc();
                            isputted = false;
                        }
                        else
                        {
                            Debug.LogError("不明なタイプの引数です");
                        }
                    }
                }
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



}

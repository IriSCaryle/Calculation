using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    [SerializeField] float Block_distance;

    [SerializeField] GameObject GenerateInitPos;

    [SerializeField] GameObject[] Blocks = new GameObject[4];


    [SerializeField] GameObject BlocksParent;

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

    PlayerBlock[,] playerBlocksBoard = new PlayerBlock[6, 6];

    CalcBlock[,] calcBlocksBoard = new CalcBlock[6, 6];




    public enum CalculationBlocks
    {
        None = 0,
        Player1Blocks =1,
        Player2Blocks =2,
        CalcBlocks =3,
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


        for (int v = 0;v < CalculationBoard.GetLength(0);v++)
        {
            for(int h = 0; h < CalculationBoard.GetLength(1); h++)
            {
                int rand = Random.Range(0, randnum.Count);

                

                switch (randnum[rand])
                {
                    case 1:

                        BlocksObjectBoard[v, h] = Instantiate(Blocks[1], new Vector2(GenerateInitPos.transform.position.x + Block_distance * h, GenerateInitPos.transform.position.y - Block_distance * v)
                        , Quaternion.identity, BlocksParent.transform);
            
                        playerBlocksBoard[v, h] = BlocksObjectBoard[v, h].GetComponent<PlayerBlock>();
                        playerBlocksBoard[v, h].vertical = v;
                        playerBlocksBoard[v, h].horizontal =h;
                        playerBlocksBoard[v, h].AssignNum();

                        player1block++;

                        if (player1block>=12)
                        {
                            randnum.Remove(1);
                        }
                       
                        break;
                    case 2:

                        BlocksObjectBoard[v, h] = Instantiate(Blocks[2], new Vector2(GenerateInitPos.transform.position.x + Block_distance * h, GenerateInitPos.transform.position.y - Block_distance * v)
                        , Quaternion.identity, BlocksParent.transform);

                        playerBlocksBoard[v, h] = BlocksObjectBoard[v, h].GetComponent<PlayerBlock>();
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
                        BlocksObjectBoard[v, h] = Instantiate(Blocks[3], new Vector2(GenerateInitPos.transform.position.x + Block_distance * h, GenerateInitPos.transform.position.y - Block_distance * v)
                        , Quaternion.identity, BlocksParent.transform);

                        calcBlocksBoard[v, h] = BlocksObjectBoard[v, h].GetComponent<CalcBlock>();
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

        


    }


    // Update is called once per frame
    void Update()
    {
        
    }
}

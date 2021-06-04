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
        
    }



    void GenerateBlocks()
    {
        for (int v = 0;v < CalculationBoard.GetLength(0);v++)
        {
            for(int h = 0; h < CalculationBoard.GetLength(1); h++)
            {
                int rand = Random.Range(1, 4);

                BlocksObjectBoard[v,h] = Instantiate(Blocks[rand], new Vector2(GenerateInitPos.transform.position.x + Block_distance * h, GenerateInitPos.transform.position.y - Block_distance * v)
                , Quaternion.identity, BlocksParent.transform);

                if(BlocksObjectBoard[v,h].gameObject.tag == "PlayerBlock")
                {
                    playerBlocksBoard[v,h] = BlocksObjectBoard[v, h].GetComponent<PlayerBlock>();
                }
                else if (BlocksObjectBoard[v, h].gameObject.tag == "CalcBlock")
                {

                    calcBlocksBoard[v, h] = BlocksObjectBoard[v, h].GetComponent<CalcBlock>();
                } 





            }
        }




    }


    // Update is called once per frame
    void Update()
    {
        
    }
}

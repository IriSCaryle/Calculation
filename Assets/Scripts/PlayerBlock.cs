using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerBlock : MonoBehaviour
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
    public enum PlayerBlocks//誰のブロックかの判別
    {
        Player1 =1,
        Player2 =2,
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



}

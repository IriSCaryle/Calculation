using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CalcBlock : MonoBehaviour
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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

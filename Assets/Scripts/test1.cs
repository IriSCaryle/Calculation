using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test1 : MonoBehaviour
{
    public int number;

    public int Digit(int num)
    {
        // Mathf.Log10(0)はNegativeInfinityを返すため、別途処理する。
        return (num == 0) ? 1 : ((int)Mathf.Log10(num) + 1);
    }

    private void Start()
    {
        Debug.Log(Digit(number));
    }
}

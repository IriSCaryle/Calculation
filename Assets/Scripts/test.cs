using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public int num;
    public int keta;

    void Start()
    {
        Debug.Log((int)(num / Mathf.Pow(10, keta - 1)) % 10);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TurnManager : MonoBehaviour
{
    [SerializeField] GameObject Player1;
    [SerializeField] GameObject Player2;
    [SerializeField] Main main;
    // Start is called before the first frame update
    void Awake()
    {
        Player1.SetActive(false);
        Player2.SetActive(false);
      
    }
    private void Start()
    {
        CheckTurn();
    }
    // Update is called once per frame
    void Update()
    {
        CheckTurn();
    }
    void CheckTurn()
    {
        switch (main.playerTurn)
        {
            case Main.Turn.Player1:
                Player1.SetActive(true);
                Player2.SetActive(false);
                break;
            case Main.Turn.Player2:
                Player2.SetActive(true);
                Player1.SetActive(false);
                break;
        }
    }
}

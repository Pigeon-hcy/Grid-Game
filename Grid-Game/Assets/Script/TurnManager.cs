using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TurnManager : MonoBehaviour
{
    [SerializeField]
    public GameManager gameManager;
    [HideInInspector]
    public enum gameStage { 
        turnStart,
        //playerRoll,
        //enemyRoll,
        playerTurn,
        enemyTurn,
        turnEnd,
        }

    public gameStage turnStage;
    [SerializeField]
    Dice[] Playerdices =
    {
        
    };

    public string currentBehave;
    [SerializeField]
    bool playerIsFinish;
    // Start is called before the first frame update
    void Start()
    {
        turnStage = gameStage.turnStart;
        
        StartCoroutine(turnFlow());


    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private IEnumerator turnFlow()
    {
        while (true)
        {
            switch (turnStage)
            {
                case gameStage.turnStart:
                    Debug.Log("TurnStart");
                    ////////////////////////////////////////
                    Debug.Log("PlayerRollDice");
                    PlayerRollDice();
                    Debug.Log(Playerdices);
                    yield return new WaitForSeconds(1f);
                    /////////////////////////////////////
                    Debug.Log("Enemy Roll dice");
                    turnStage = gameStage.playerTurn;
                    break;
                case gameStage.playerTurn:
                    Debug.Log("Player turn start");
                    while (turnStage == gameStage.playerTurn)
                    { 
                        playerMove();
                        if (playerIsFinish)
                        { 
                            turnStage = gameStage.enemyTurn;
                        }

                        yield return null;
                    }
                    //yield return new WaitUntil(playIsFinished);
                    //turnStage = gameStage.enemyTurn;
                    break;
                case gameStage.enemyTurn:
                    yield return null;
                    break;
                case gameStage.turnEnd:
                    yield return null;  
                    break;

            }
        }
    }


    private void PlayerRollDice()
    {
        for (int i = 0; i < Playerdices.Length; i++)
        {
            Playerdices[i].rollADice();
        }
    }


    private bool checkEndTurn()
    {
        bool canEndTurn = true;
        for (int i = 0; i < Playerdices.Length; i++)
        {
            if (Playerdices[i].isUsed == true)
            { 
                canEndTurn = false;
            }
        }
        //TODO enemy dice is all used?


        return canEndTurn;
    }

    private void playerMove()
    {
        playerIsFinish = false;
        Debug.Log("start if");
        if (gameManager.selectedDice != null)
        {
            Debug.Log("Pass if");
            if (gameManager.selectedDice.isUsed == false)
            {
                switch (gameManager.selectedDice.behave)
                {
                    case "Move":
                        Debug.Log("Pass case");
                        currentBehave = "Move";
                        break;
                    case "Attack":
                        Debug.Log("Pass case");
                        currentBehave = "Attack";
                        break;
                    case "Empty":
                        Debug.Log("Pass case");
                        currentBehave = "Empty";
                        break;
                }
            }
        }
    }

    public void excuteTheBehave()
    {
        currentBehave = null;
        gameManager.selectedDice.isUsed = true;
        playerIsFinish = true;
    }



}

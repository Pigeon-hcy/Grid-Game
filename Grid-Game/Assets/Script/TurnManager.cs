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

    [SerializeField]
    Dice[] EnemyDice =
    {

    };

    public string currentBehave;
    [SerializeField]
    static bool playerIsFinish;

    [SerializeField]
    public Unit[] EnemyList = new Unit[5];
    [SerializeField]
    public List<Unit> PlayerList;
    [SerializeField]
    GameObject skipButton;



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
                    EnemyRollDice();
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
                        if (gameManager.selectedDice != null && gameManager.selectedDice.isUsed == false)
                        { 
                            skipButton.SetActive(true);
                        }
                        playerMove();

                        if (playerIsFinish)
                        {
                            skipButton.SetActive(false);
                            turnStage = gameStage.enemyTurn;
                        }

                        yield return new WaitForSeconds(2f);
                    }
                    //yield return new WaitUntil(playIsFinished);
                    //turnStage = gameStage.enemyTurn;
                    break;
                case gameStage.enemyTurn:

                    
                    enemyThinkMove();
                    if (passToPlayerOrEndTheTurn() == true)
                    {
                        turnStage = gameStage.turnEnd;
                    }
                    else
                    {
                        turnStage = gameStage.playerTurn;
                    }

                    yield return new WaitForSeconds(2f);
                    break;
                case gameStage.turnEnd:
                    
                    restAllDice();
                    turnStage = gameStage.turnStart;
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

    private void EnemyRollDice()
    {
        for (int i = 0; i < EnemyDice.Length; i++)
        {
            EnemyDice[i].rollADice();
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
        turnStage = gameStage.enemyTurn;
    }

    private void enemyThinkMove()
    {
        
        for (int i = 0; i < EnemyList.Length; i++)
        {
            if (EnemyList[i] != null)
            {
                ///////////////////////////////////Attack///////////////////////////////////////////
                if (EnemyList[i].CheckForPlayer(EnemyList[i].attackRange) == true)
                {
                    for (int j = 0; j < EnemyDice.Length; j++)
                    {
                        if (EnemyDice[j].behave == "Attack" && EnemyDice[j].isUsed == false)// have attack relate dice
                        {
                            Debug.Log("Enemy try to attack");
                            int targetIndex = 0;
                            int targetHealth = int.MaxValue;
                            for (int k = 0; k < EnemyList[i].nearByPlayer.Count - 1; i++)
                            { 
                                if (EnemyList[i].nearByPlayer[k].health > targetHealth)
                                {
                                    targetHealth = EnemyList[i].nearByPlayer[k].health;
                                    targetIndex = k;
                                }
                            }
                            EnemyList[i].nearByPlayer[targetIndex].health -= EnemyList[i].attackDamage;

                            EnemyDice[j].isUsed = true;
                            return;
                        }
                    }
                }

                //////////////////////////////////Move/////////////////////////////////////
                else if (EnemyList[i].CheckForPlayer(EnemyList[i].attackRange) == false)
                {
                    for (int j = 0; j < EnemyDice.Length; j++)
                    {
                        if (EnemyDice[j].behave == "Move" && EnemyDice[j].isUsed == false)// have attack relate dice
                        {
                            while (true)
                            {
                                int randomEnemy = Random.Range(0, EnemyList.Length - 1);
                                if (EnemyList[randomEnemy] == null)
                                {
                                    randomEnemy = Random.Range(0, EnemyList.Length - 1);
                                }
                                else
                                {
                                    EnemyList[randomEnemy].EnemyMoveTo(PlayerList);
                                    break;
                                }
                            }
                            
                            Debug.Log("Enemy try to move");
                            EnemyDice[j].isUsed = true;
                            return;
                        }
                    }

                    //////////////////////////////SKIP//////////////////////////////////////////
                    for (int j = 0; j < EnemyDice.Length; j++)
                    {
                        if (EnemyDice[j].behave == "Empty" && EnemyDice[j].isUsed == false)// have attack relate dice
                        {
                            Debug.Log("skip");
                            EnemyDice[j].isUsed = true;
                            return;
                        }
                    }

                    for (int j = 0; j < EnemyDice.Length; j++)
                    {
                        if (EnemyDice[j].isUsed == false)// have attack relate dice
                        {
                            Debug.Log("skip");
                            EnemyDice[j].isUsed = true;
                            return;
                        }
                    }

                }
            }

        }
    }

    private void restAllDice()
    {
        for (int i = 0; i < Playerdices.Length; i++)
        {
            Playerdices[i].isUsed = false;
        }

        for (int i = 0; i < EnemyDice.Length; i++)
        {
            EnemyDice[i].isUsed= false;
        }
    }

    private bool passToPlayerOrEndTheTurn()
    {
        for (int i = 0; i < Playerdices.Length; i++)
        {
            if (!Playerdices[i].isUsed)
            { 
                return false;
            }
        }

        for (int i = 0; i < EnemyDice.Length; i++)
        {
            if (!EnemyDice[i].isUsed)
            {
                return false;
            }
        }

        return true;    
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
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
    public Dice[] Playerdices =
    {
        
    };

    [SerializeField]
    public Dice[] EnemyDice =
    {

    };

    public string currentBehave;
    [SerializeField]
    static bool playerIsFinish;

    [SerializeField]
    public List<Unit> EnemyList;
    [SerializeField]
    public List<Unit> PlayerList;
    [SerializeField]
    GameObject skipButton;
    [SerializeField]
    TMP_Text text;
    [SerializeField]
    GridManager gridManager;

    [SerializeField]
    string RestartScreenName;
    
    UnitLoader UnitLoader;
    [SerializeField]
    bool isenemyFinish = false;

    [SerializeField]
    AudioSource diceThrowSoundEffect;
    [SerializeField]
    AudioSource skipSoundEffect;
    [SerializeField]
    GameObject gameover;
    // Start is called before the first frame update
    void Start()
    {
        turnStage = gameStage.turnStart;
        
        StartCoroutine(turnFlow());
        

    }

    // Update is called once per frame
    void Update()
    {
        text.text = "Current Move " + currentBehave;

        if (EnemyList.Count == 0 || PlayerList.Count == 0)
        {
            StartCoroutine(GameOver());


        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            UnitLoader.Instance.Playersunits.Clear();
            UnitLoader.Instance.gameStart = false;
            SceneManager.LoadScene(0);
        }
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
                    diceThrowSoundEffect.Play();
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

                    isenemyFinish = false;
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
                    case "Effect":
                        currentBehave = "Effect";
                        break;
                    case "Charge":
                        currentBehave = "Charge";
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
        Debug.Log("Thinking move!");
        for (int i = 0; i < EnemyList.Count; i++)
        {
            Debug.Log("Do for" + i);
            if (EnemyList[i] != null)
            {
                //////////////////////////////TODO Charge/////////////////////////////////

                for (int j = 0; j < EnemyDice.Length; j++)
                {
                    if (EnemyDice[j].behave == "Charge" && EnemyDice[j].isUsed == false && isenemyFinish == false)// have attack relate dice
                    {

                        isenemyFinish = true;
                        Debug.Log("Enemy try to charge");
                        bool isCharing = true;
                        int randomIndex;

                            int randomEnemy = Random.Range(0, EnemyList.Count );
                            randomIndex = randomEnemy;
                            if (EnemyList[randomEnemy] == null)
                            {
                                randomEnemy = Random.Range(0, EnemyList.Count);
                                Debug.Log("refind");
                            }
                            else
                            {
                                Debug.Log("Enemy try to move due to charge");
                                EnemyList[randomEnemy].EnemyMoveTo(PlayerList);
                                Debug.Log("Enemy try to attack after charge");
                                StartCoroutine(checkChargeAttack(randomIndex, j));
                                EnemyList[randomEnemy].animator.SetTrigger("Attack");
                                isCharing = false;
                                break;
                            }


                        Debug.Log("Return" + i);
                        return;
                        ///////////////Attack//////////////////

                    }
                }



                ///////////////////////////////////Attack///////////////////////////////////////////
                if (EnemyList[i].CheckForPlayer(EnemyList[i].attackRange) == true)
                {
                    for (int j = 0; j < EnemyDice.Length; j++)
                    {
                        if (EnemyDice[j].behave == "Attack" && EnemyDice[j].isUsed == false && isenemyFinish == false)// have attack relate dice
                        {
                            isenemyFinish = true;
                            EnemyList[i].drawAttackRange();
                            Debug.Log("Enemy try to attack");
                            int targetIndex = 0;
                            int targetHealth = int.MaxValue;
                            for (int k = 0; k < EnemyList[i].nearByPlayer.Count - 1; k++)
                            { 
                                if (EnemyList[i].nearByPlayer[k].health > targetHealth)
                                {
                                    targetHealth = EnemyList[i].nearByPlayer[k].health;
                                    targetIndex = k;
                                }
                            }
                            EnemyList[i].animator.SetTrigger("Attack");
                            EnemyList[i].nearByPlayer[targetIndex].health -= EnemyList[i].attackDamage;
                            gridManager.ResetTile();
                            EnemyDice[j].isUsed = true;
                            Debug.Log("Return" + i);
                            return;
                            
                        }
                    }
                }

                //////////////////////////////////Move/////////////////////////////////////
                else if (EnemyList[i].CheckForPlayer(EnemyList[i].attackRange) == false)
                {
                    for (int j = 0; j < EnemyDice.Length; j++)
                    {
                        if (EnemyDice[j].behave == "Move" && EnemyDice[j].isUsed == false && isenemyFinish == false)// have attack relate dice
                        {
                            isenemyFinish = true;
                            while (true)
                            {
                                int randomEnemy = Random.Range(0, EnemyList.Count);
                                if (EnemyList[randomEnemy] == null)
                                {
                                    randomEnemy = Random.Range(0, EnemyList.Count);
                                }
                                else
                                {
                                    EnemyList[randomEnemy].EnemyMoveTo(PlayerList);
                                    break;
                                }
                            }
                            
                            Debug.Log("Enemy try to move");
                            EnemyDice[j].isUsed = true;
                            Debug.Log("Return" + i);
                            return;
                        }
                    }

                    /////////////////////////////Effect/////////////////////////////////
                    for (int j = 0; j < EnemyDice.Length; j++)
                    {
                        if (EnemyDice[j].behave == "Effect" && EnemyDice[j].isUsed == false && isenemyFinish == false)// have attack relate dice
                        {
                            isenemyFinish = true;
                            EnemyDice[j].isUsed = true;
                            int randomEnemy = Random.Range(0, EnemyList.Count );
                            if (EnemyList[randomEnemy] == null)
                            {
                                randomEnemy = Random.Range(0, EnemyList.Count );
                            }
                            else
                            {
                                EnemyList[randomEnemy].useEffect();
                                EnemyDice[j].isUsed = true;
                                break;
                            }


                            Debug.Log("Return" + i);
                            return;
                        }
                    }


                    //////////////////////////////SKIP//////////////////////////////////////////
                    for (int j = 0; j < EnemyDice.Length; j++)
                    {
                        if (EnemyDice[j].behave == "Empty" && EnemyDice[j].isUsed == false && isenemyFinish == false)// have attack relate dice
                        {
                            isenemyFinish = true;
                            Debug.Log("skip");
                            EnemyDice[j].isUsed = true;
                            skipSoundEffect.Play();
                            return;
                        }
                    }
                   



                    for (int j = 0; j < EnemyDice.Length; j++)
                    {
                        if (EnemyDice[j].isUsed == false && EnemyDice[j].behave != "Effect" && isenemyFinish == false)// have attack relate dice
                        {
                            isenemyFinish = true;
                            skipSoundEffect.Play();
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

        //for (int i = 0; i < EnemyDice.Length; i++)
        //{
        //    if (!EnemyDice[i].isUsed)
        //    {
        //        return false;
        //    }
        //}

        return true;    
    }

    private IEnumerator checkChargeAttack(int randomIndex, int diceIndex )
    {
        
        yield return new WaitForSeconds(1.5f);
        if (EnemyList[randomIndex].CheckForPlayer(1) == true && EnemyList[randomIndex].isMoving == false)
        {
            EnemyList[randomIndex].drawAttackRange();
            Debug.Log("Enemy try to attack");
            int targetIndex = 0;
            int targetHealth = int.MaxValue;
            for (int k = 0; k < EnemyList[randomIndex].nearByPlayer.Count - 1; k++)
            {
                if (EnemyList[randomIndex].nearByPlayer[k].health > targetHealth)
                {
                    targetHealth = EnemyList[randomIndex].nearByPlayer[k].health;
                    targetIndex = k;
                }
            }
            EnemyList[randomIndex].nearByPlayer[targetIndex].health -= EnemyList[randomIndex].attackDamage;

            EnemyDice[diceIndex].isUsed = true;

            gridManager.ResetTile();

            yield break;
        }
        else
        {
            Debug.Log("Enemy chargeFinsh");
            EnemyDice[diceIndex].isUsed = true;
            yield break;

        }
    }

    IEnumerator GameOver()
    {
        gameover.SetActive(true);
        yield return new WaitForSeconds(3f);
        UnitLoader.Instance.Playersunits.Clear();
        UnitLoader.Instance.gameStart = false;
        SceneManager.LoadScene(0);
    }
}
